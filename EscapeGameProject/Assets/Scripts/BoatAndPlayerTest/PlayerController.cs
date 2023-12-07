using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;

public class PlayerCotroller : MonoBehaviour
{
    public InputManager inputManager;

    public Rigidbody rb;
    [Tooltip("걷는 속도")]
    public float speed = 10;
    [Tooltip("뛰어가는 속도")]
    public float runSpeed = 15;
    [Tooltip("점프하는 힘(높이)")]
    public float jumpForce = 200;

    private bool _isGrounded;
    public bool _isJump;
    private bool _isCrouch;
    private bool _isSprint;

    //
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    //  
    float attack;
    float right;
    float forward;
    float crouch;

    const float k_Half = 0.5f;

    // 캐릭터 기본 요소
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;

    // 카메라 위치 정보
    public Transform cameraObject;
    private Vector3 originalCameraLocalPosition;


    // 칼
    [SerializeField]
    private GameObject knifeOnHand;
    [SerializeField]
    private GameObject knifeOnShoulder;
    // 도끼
    [SerializeField]
    private GameObject hammeOnHand;
    [SerializeField]
    private GameObject hammerOnShoulder;
    // 곡괭이
    [SerializeField]
    private GameObject pickaxeOnHand;
    [SerializeField]
    private GameObject pickaxeOnShoulder;
    //횟불
    [SerializeField]
    private GameObject torchOnHand;
    [SerializeField]
    private GameObject torchOnShoulder;

    //  
    public bool isEquipping; // 장착 중
    public bool isEquipped;  // 장착 됨

    // 1,2,3,4 번 키 반복하여 누른 횟수
    private bool repeatClick1 = false;
    private bool repeatClick2 = false;
    private bool repeatClick3 = false;
    private bool repeatClick4 = false;

    // 무기 소지 여부 확인, 퀵슬롯에 아이템이 있을 때 true. -> 퀵슬롯에서
    // 인벤토리에 존재
    public bool isKnifeInventory;
    public bool isHammerInventory;
    public bool isPickaxeInventory;
    public bool isTorchInventory;


    // 누르면 트리거로 실행. -> 장착중이면 해제, 장착 중이 아니고, is~~값이 true이면 장착
    private bool knifeChangeTrigger;
    private bool hammerChangeTrigger;
    private bool pickaxeChangeTrigger;
    private bool torchChangeTrigger;


    // 현재 손에 장착중인 장비의 Bool값
    private bool knifeEquipOnHand;
    private bool hammerEquipOnHand;
    private bool pickaxeEquipOnHand;
    private bool torchEquipOnHand;

    //막기
    public bool isBlocking;

    //발차기
    public bool isKicking;

    //공격 요소
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    // 무기 장착
    //public GameObject[] weaponPrefabs;
    //private int weaponNum =0;

    private void Awake()
    {

    }

    private void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();
        inputManager.inputMaster.Movement.Attack.started += _ => AttackMonster();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        originalCameraLocalPosition = cameraObject.transform.localPosition;


        playerHasWeaponItem();

        if (isKnifeInventory == true)
        {
            m_Animator.SetBool("KnifeOnShoulder", true);
        }
        if (isHammerInventory == true)
        {
            m_Animator.SetBool("HammerOnShoulder", true);
        }
        if (isPickaxeInventory == true)
        {
            m_Animator.SetBool("PickaxeOnShoulder", true);
        }
        if (isTorchInventory == true)
        {
            m_Animator.SetBool("TorchOnShoulder", true);
        }


    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        //playerHasWeaponItem();

        Moving();
        Crouch();
        Sprint();

        AttackMonster();
        Equip();
        Block();
        Kick();

        
    }

    /// <summary>
    /// Input System으로 움직이는 코드
    /// </summary>
    private void Moving()
    {
        forward = inputManager.inputMaster.Movement.Forward.ReadValue<float>();
        right = inputManager.inputMaster.Movement.Right.ReadValue<float>();

        Vector3 move = transform.right * right + transform.forward * forward;
        move *= inputManager.inputMaster.Movement.Run.ReadValue<float>() == 0 ? speed : runSpeed;


        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);


        UpdateAnimator(move);


    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            _isGrounded = true;
        }


    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {

            _isGrounded = false;
        }


    }

    /* 무기교체
    public void ChangeWeapon()
    {
       

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Player를 찾기
            GameObject player = GameObject.Find("Player");

            // Player가 있다면
            if (player != null)
            {
                // Player의 자식들을 찾아 Weapon을 검색
                Transform weapon = player.transform.Find("M_Nurse_Basic/_M_Rig/DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R/Weapon");

                // Weapon이 있다면
                if (weapon != null)
                {
                    // 기존의 자식 GameObject들 삭제
                    foreach (Transform child in weapon)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    // 무기 프리팹 배열 길이에 맞춰 무기 번호 설정
                    weaponNum %= weaponPrefabs.Length;

                    // 새로운 무기의 프리팹을 인스턴스화하고 Weapon의 자식으로 설정
                    GameObject newWeapon = Instantiate(weaponPrefabs[weaponNum], weapon);
                    weaponNum++; // 다음 무기 번호로 이동
                    
                }
            }
        }


    }
    */


    public void AttackMonster()
    {
        float attackValue = inputManager.inputMaster.Movement.Attack.ReadValue<float>();
        if (attackValue > 0 && m_Animator.GetBool("OnGround") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            //currentAttack++;
            isAttacking = true;

            /*
            if (currentAttack > 3)
                currentAttack = 1;
            */

            //Reset
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            //Call Attack Triggers
            m_Animator.SetTrigger("Attack" + currentAttack);

            //Reset Timer
            timeSinceAttack = 0;
        }

    }

    //This will be used at animation event
    public void ResetAttack()
    {
        isAttacking = false;
    }

    public void Kick()
    {
        float kickValue = inputManager.inputMaster.Movement.Kick.ReadValue<float>();
        if (kickValue > 0 && m_Animator.GetBool("OnGround"))
        {
            m_Animator.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            m_Animator.SetBool("Kick", false);
            isKicking = false;
        }
    }


    // 플레이어가 무기 아이템을 가지고 있는지 확인하는 함수
    public void SetHasKnife(bool hasItem)
    {
        isKnifeInventory = hasItem;
        m_Animator.SetBool("KnifeInventory", hasItem);
    }
    public void SetHasHammer(bool hasItem)
    {
        isHammerInventory = hasItem;
        m_Animator.SetBool("HammerInventory", hasItem);
    }
    public void SetHasPickaxe(bool hasItem)
    {
        isPickaxeInventory = hasItem;
        m_Animator.SetBool("PickaxeInventory", hasItem);
    }
    public void SetHasTouch(bool hasItem)
    {
        isTorchInventory = hasItem;
        m_Animator.SetBool("TorchInventory", hasItem);
    }

    public void playerHasWeaponItem()
    {

        SetHasKnife(knifeOnShoulder.activeSelf);
        SetHasHammer(hammerOnShoulder.activeSelf);
        SetHasPickaxe(pickaxeOnShoulder.activeSelf);
        SetHasTouch(torchOnShoulder.activeSelf);


    }

    public void Equipped()
    {
        isEquipping = false;
    }

    private void Block()
    {
        float blockValue = inputManager.inputMaster.Movement.Shield.ReadValue<float>();
        if (blockValue > 0 && m_Animator.GetBool("OnGround"))
        {
            m_Animator.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            m_Animator.SetBool("Block", false);
            isBlocking = false;
        }
    }

    public void EquipKnife()
    {
        if (isKnifeInventory == true)
        {
            if (repeatClick1 == false)
            {
                m_Animator.SetBool("isArmed", true);
                m_Animator.SetTrigger("knifeEquipTrigger");
                isEquipping = true;
                knifeChangeTrigger = true;

                repeatClick1 = true;
                repeatClick2 = false;
                repeatClick3 = false;
                repeatClick4 = false;
            }
            else if (repeatClick1 == true)
            {
                m_Animator.SetBool("KnifeOnShoulder", true);
                m_Animator.SetBool("isArmed", false);
                m_Animator.SetTrigger("knifeEquipTrigger");
                isEquipping = false;
                knifeChangeTrigger = false;

                repeatClick1 = false;
                repeatClick2 = false;
                repeatClick3 = false;
                repeatClick4 = false;
            }
        }
    }
    public void EquipPickaxe()
    {

    }
    public void EquipAxe()
    {

    }
    public void EquipTorch()
    {

    }
    public void EquipFood(int itemID)
    {

    }
    private void Equip()
    {

        float changeKnifeValue = inputManager.inputMaster.Movement.ChangeKnife.ReadValue<float>();
        float changeHammerValue = inputManager.inputMaster.Movement.ChangeHammer.ReadValue<float>();
        float changePickaxeValue = inputManager.inputMaster.Movement.ChangePickaxe.ReadValue<float>();
        float changeTorchValue = inputManager.inputMaster.Movement.ChangeTorch.ReadValue<float>();

        if (m_Animator.GetBool("OnGround"))
        {

            // 칼
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                if (isKnifeInventory == true)
                {
                    if (repeatClick1 == false)
                    {
                        m_Animator.SetBool("isArmed", true);
                        m_Animator.SetTrigger("knifeEquipTrigger");
                        isEquipping = true;
                        knifeChangeTrigger = true;

                        repeatClick1 = true;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }
                    else if (repeatClick1 == true)
                    {
                        m_Animator.SetBool("KnifeOnShoulder", true);
                        m_Animator.SetBool("isArmed", false);
                        m_Animator.SetTrigger("knifeEquipTrigger");
                        isEquipping = false;
                        knifeChangeTrigger = false;

                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }


                }
            }
            // 도끼        

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (isHammerInventory == true)
                {
                    if (repeatClick2 == false)
                    {
                        m_Animator.SetBool("isArmed", true);
                        m_Animator.SetTrigger("hammerEquipTrigger");
                        isEquipping = true;
                        hammerChangeTrigger = true;

                        repeatClick1 = false;
                        repeatClick2 = true;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }
                    else if (repeatClick2 == true)
                    {
                        m_Animator.SetBool("HammerOnShoulder", true);
                        m_Animator.SetBool("isArmed", false);
                        m_Animator.SetTrigger("hammerEquipTrigger");
                        isEquipping = false;
                        hammerChangeTrigger = false;


                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }
                }

            }
            // 곡괭이
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (isPickaxeInventory == true)
                {
                    if (repeatClick3 == false)
                    {
                        m_Animator.SetBool("isArmed", true);
                        isEquipping = true;
                        m_Animator.SetTrigger("pickaxeEquipTrigger");
                        pickaxeChangeTrigger = true;


                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = true;
                        repeatClick4 = false;
                    }
                    else if (repeatClick3 == true)
                    {
                        m_Animator.SetBool("PickaxeOnShoulder", true);
                        m_Animator.SetBool("isArmed", false);
                        m_Animator.SetTrigger("pickaxeEquipTrigger");
                        isEquipping = false;
                        pickaxeChangeTrigger = false;


                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }


                }
            }
            // 횟불
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (isTorchInventory == true)
                {
                    if (repeatClick4 == false)
                    {
                        m_Animator.SetBool("isArmed", true);
                        isEquipping = true;
                        m_Animator.SetTrigger("torchEquipTrigger");
                        torchChangeTrigger = true;

                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = true;
                    }
                    else if (repeatClick4 == true)
                    {
                        m_Animator.SetBool("TorchOnShoulder", true);
                        m_Animator.SetBool("isArmed", false);
                        m_Animator.SetTrigger("torchEquipTrigger");
                        isEquipping = false;
                        pickaxeChangeTrigger = false;


                        repeatClick1 = false;
                        repeatClick2 = false;
                        repeatClick3 = false;
                        repeatClick4 = false;
                    }

                }
            }


        }



    }

    public void ActiveWeapon()
    {

        if (!isEquipped)
        {
            // 칼
            if (knifeChangeTrigger == true)
            {
                m_Animator.SetBool("KnifeOnShoulder", false);
                knifeOnHand.SetActive(true);
                knifeOnShoulder.SetActive(false);
                knifeChangeTrigger = false;
                knifeEquipOnHand = true;  // 손에 장착
                Debug.Log("칼 장착");
            }
            // 망치
            else if (hammerChangeTrigger == true)
            {
                m_Animator.SetBool("HammerOnShoulder", false);
                hammeOnHand.SetActive(true);
                hammerOnShoulder.SetActive(false);
                hammerChangeTrigger = false;
                hammerEquipOnHand = true;
                Debug.Log("도끼 장착");

            }
            // 곡괭이
            else if (pickaxeChangeTrigger == true)
            {
                m_Animator.SetBool("PickaxeOnShoulder", false);
                pickaxeOnHand.SetActive(true);
                pickaxeOnShoulder.SetActive(false);
                pickaxeChangeTrigger = false;
                pickaxeEquipOnHand = true;
                Debug.Log("곡괭이 장착");

            }
            // 횟불
            else if (torchChangeTrigger == true)
            {
                m_Animator.SetBool("TorchOnShoulder", false);
                torchOnHand.SetActive(true);
                torchOnShoulder.SetActive(false);
                torchChangeTrigger = false;
                torchEquipOnHand = true;
                Debug.Log("횟불 장착");
            }
            isEquipped = !isEquipped;

        }
        else
        {

            // 칼
            if (knifeEquipOnHand == true)
            {
                m_Animator.SetBool("KnifeOnShoulder", true);
                knifeOnHand.SetActive(false);
                knifeOnShoulder.SetActive(true);
                knifeEquipOnHand = false;
                Debug.Log("칼 해제");
            }
            // 망치
            else if (hammerEquipOnHand == true)
            {
                m_Animator.SetBool("HammerOnShoulder", true);
                hammeOnHand.SetActive(false);
                hammerOnShoulder.SetActive(true);
                hammerEquipOnHand = false;
                Debug.Log("해머 해제");
            }
            // 곡괭이
            else if (pickaxeEquipOnHand == true)
            {
                m_Animator.SetBool("PickaxeOnShoulder", true);
                pickaxeOnHand.SetActive(false);
                pickaxeOnShoulder.SetActive(true);
                pickaxeEquipOnHand = false;
                Debug.Log("곡괭이 해제");
            }
            // 횟불
            else if (torchEquipOnHand == true)
            {
                m_Animator.SetBool("TorchOnShoulder", true);
                torchOnHand.SetActive(false);
                torchOnShoulder.SetActive(true);
                torchEquipOnHand = false;
                Debug.Log("횟불 해제");
            }
            m_Animator.SetBool("isArmed", false);
            isEquipped = !isEquipped;
        }




    }

    private void Sprint()
    {
        float sprintValue = inputManager.inputMaster.Movement.Run.ReadValue<float>();
        _isSprint = sprintValue > 0.0f;


        if (_isSprint)
        {
            m_Animator.SetFloat("Speed_f", 2f); // Speed_f 값을 변경
            //Debug.Log("뛰는 중");
        }
        else
        {
            m_Animator.SetFloat("Speed_f", 1f); // Speed_f 값을 변경
            //Debug.Log("뛰기 끝");
        }
    }




    private void Jump()
    {
        //float jumpValue = inputManager.inputMaster.Movement.Jump.ReadValue<float>();

        if(Input.GetKeyDown(KeyCode.Space))//if(jumpValue > 0)
        {
            if (_isGrounded)
            {
                
                rb.AddForce(Vector3.up * jumpForce);

                Vector3 newPosition = cameraObject.transform.position;
                newPosition.y = inputManager.inputMaster.Movement.Jump.ReadValue<float>() == 0 ? 1f : 1.3f;
                cameraObject.transform.position = newPosition;

                _isJump = true;          

            }
            else
            {
                _isJump = false;
                cameraObject.transform.localPosition = originalCameraLocalPosition;
            }
        }
       

    }





    private void Crouch()
    {
        float crouchValue = inputManager.inputMaster.Movement.Crouch.ReadValue<float>();
        _isCrouch = crouchValue > 0.0f;

        if (_isCrouch)
        {
            Vector3 newPosition = cameraObject.transform.position;
            newPosition.y = inputManager.inputMaster.Movement.Jump.ReadValue<float>() == 0 ? 1f : -0.777f;
            cameraObject.transform.position = newPosition;
        }
        else
        {
            cameraObject.transform.localPosition = originalCameraLocalPosition;
        }

        m_Animator.SetBool("Crouch", _isCrouch);
    }



    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (_isGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }



    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", right, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Attack", attack, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", _isCrouch);
        m_Animator.SetBool("OnGround", _isGrounded);


        if (!_isGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);

        }
      


        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * forward;
        if (_isGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (_isGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
            Debug.Log("에어본");
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }


}