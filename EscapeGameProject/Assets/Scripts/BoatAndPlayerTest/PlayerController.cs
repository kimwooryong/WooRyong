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

    // 카메라 위치 정보
    private Camera playerCamera;
    //private Vector3 originalCameraLocalPosition;
    // camera 위치
    public Transform standingCamPos; // 서 있을 때 카메라 위치
    public Transform sittingCamPos; // 앉을 때 카메라 위치
    public Transform jumpCamPos;
    //

    const float k_Half = 0.5f;

    // 캐릭터 기본 요소
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;




    // 칼
    [SerializeField]
    private GameObject knifeOnHand;
    [SerializeField]
    private GameObject knifeOnShoulder;
    // 도끼
    [SerializeField]
    private GameObject axeOnHand;
    [SerializeField]
    private GameObject axeOnShoulder;
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
    public bool isAxeInventory;
    public bool isPickaxeInventory;
    public bool isTorchInventory;


    // 누르면 트리거로 실행. -> 장착중이면 해제, 장착 중이 아니고, is~~값이 true이면 장착
    private bool knifeChangeTrigger;
    private bool axeChangeTrigger;
    private bool pickaxeChangeTrigger;
    private bool torchChangeTrigger;


    // 현재 손에 장착중인 장비의 Bool값
    private bool knifeEquipOnHand;
    private bool axeEquipOnHand;
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


    private void Awake()
    {
 
    }

    private void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();
        inputManager.inputMaster.Movement.Attack.started += _ => AttackMonster();

        playerCamera = GetComponentInChildren<Camera>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        //originalCameraLocalPosition = playerCamera.transform.localPosition;

    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        //playerHasWeaponItem();

        Moving();
        Crouch();
        //Crouch();
        Sprint();


        //Equip();
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
        AttackMonster();

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
    public void SetHasAxe(bool hasItem)
    {
        isAxeInventory = hasItem;
        m_Animator.SetBool("AxeInventory", hasItem);
    }
    public void SetHasPickaxe(bool hasItem)
    {
        isPickaxeInventory = hasItem;
        m_Animator.SetBool("PickaxeInventory", hasItem);
    }
    public void SetHasTorch(bool hasItem)
    {
        isTorchInventory = hasItem;
        m_Animator.SetBool("TorchInventory", hasItem);
    }

    public void playerHasWeaponItem()
    {
        /*
        SetHasKnife(knifeOnShoulder.activeSelf);
        SetHasAxe(axeOnShoulder.activeSelf);
        SetHasPickaxe(pickaxeOnShoulder.activeSelf);
        SetHasTouch(torchOnShoulder.activeSelf);
        */
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
        if (m_Animator.GetBool("OnGround") && isKnifeInventory == true)
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
        if (m_Animator.GetBool("OnGround") &&  isPickaxeInventory == true)
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
    public void EquipAxe()
    {
        if (m_Animator.GetBool("OnGround") &&  isAxeInventory == true)
        {
            if (repeatClick2 == false)
            {
                m_Animator.SetBool("isArmed", true);
                m_Animator.SetTrigger("axeEquipTrigger");
                isEquipping = true;
                axeChangeTrigger = true;

                repeatClick1 = false;
                repeatClick2 = true;
                repeatClick3 = false;
                repeatClick4 = false;
            }
            else if (repeatClick2 == true)
            {
                m_Animator.SetBool("AxeOnShoulder", true);
                m_Animator.SetBool("isArmed", false);
                m_Animator.SetTrigger("axeEquipTrigger");
                isEquipping = false;
                axeChangeTrigger = false;


                repeatClick1 = false;
                repeatClick2 = false;
                repeatClick3 = false;
                repeatClick4 = false;
            }
        }
    }
    public void EquipTorch()
    {
        if (m_Animator.GetBool("OnGround") && isTorchInventory == true)
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
    #region 음식 장착
    
        // 음식을 꺼내서 손에 생성하는 코드;
        //ItemManager.Instance.SetFoodOnHand(itemID);
        // 손에 든 음식을 없애는 코드;
        //ItemManager.Instance.DeleteFoodOnHand();
        //손에 음식을 들고 있을 때, 음식을 먹었을 때 일어나는 일들 (HP가 올라간다거나)
        //ItemManager.Instance.UseFood(itemID);
    public void EquipFood(int itemID)
    {
        //만약 손에 다른 것이 들려있다면, 그것 대신 음식을 손에 든다.
        // 1. 손에 들고있는 도구? 무기를 집어넣는다.
        //아이템을 소환해서, 플레이어 손에 붙이는 함수
    }
    #endregion
    private void Equip()
    {

        if (m_Animator.GetBool("OnGround"))
        {

            // 칼
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquipKnife();
            }
            // 도끼        

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquipAxe();
            }
            // 곡괭이
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EquipPickaxe();
            }
            // 횟불
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                EquipTorch();
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
            else if (axeChangeTrigger == true)
            {
                m_Animator.SetBool("AxeOnShoulder", false);
                axeOnHand.SetActive(true);
                axeOnShoulder.SetActive(false);
                axeChangeTrigger = false;
                axeEquipOnHand = true;
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
            else if (axeEquipOnHand == true)
            {
                m_Animator.SetBool("AxeOnShoulder", true);
                axeOnHand.SetActive(false);
                axeOnShoulder.SetActive(true);
                axeEquipOnHand = false;
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
        //if(jumpValue > 0)
        //{
        //    _isGrounded = false;
        //}

        if(Input.GetKeyDown(KeyCode.Space))//
        {
            if (_isGrounded)
            {            
                rb.AddForce(Vector3.up * jumpForce);
                jumpCamPosition();
                //Vector3 newPosition = playerCamera.transform.position;
                //newPosition.y = inputManager.inputMaster.Movement.Jump.ReadValue<float>() == 0 ? 1f : 1.3f;
                //playerCamera.transform.position = newPosition;
                _isJump = true;          

            }
            else
            {             
                StandCamPosition();
                _isJump = false;
                //playerCamera.transform.localPosition = originalCameraLocalPosition;
            }
        }
       

    }


    //private void HandleCrouch()
    //{
    //    float crouchValue = inputManager.inputMaster.Movement.Crouch.ReadValue<float>();
    //    _isCrouch = crouchValue > 0.0f;
    //    if (_isCrouch == true && _isGrounded == true)
    //    {
    //        StartCoroutine(CrouchStand());
    //    }
    //}

    //private IEnumerator CrouchStand()
    //{
    //    if(_isCrouch && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
    //    {
    //        yield break;
    //    }

    //    m_Animator.SetBool("Crouch", true);

    //    float timeElapsed = 0;
    //    float targetHeight = _isCrouch ? standingHeight : crouchHeight;
    //    float currentHeight = characterController.height;
    //    Vector3 targetCenter = _isCrouch ? standingCenter : crouchingCenter;
    //    Vector3 currentCenter = characterController.center;

    //    while(timeElapsed < timeToCrouch)
    //    {
    //        characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
    //        characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;

    //    }

    //    characterController.height = targetHeight;
    //    characterController.center = targetCenter;

    //    _isCrouch = !_isCrouch;

    //    m_Animator.SetBool("Crouch", false);

    //}
    public void Crouch()
    {
        float crouchValue = inputManager.inputMaster.Movement.Crouch.ReadValue<float>();
        _isCrouch = crouchValue > 0.0f;
        //_isCrouch = !_isCrouch;
        if (_isCrouch)
        {
            // 앉는 동작을 수행하고 카메라 위치 변경
            SitCamPosition();
        }
        else
        {
            // 서 있는 동작을 수행하고 카메라 위치 변경
            StandCamPosition();
        }

        m_Animator.SetBool("Crouch", _isCrouch);
    }

    // 앉는 동작을 정의합니다.
    private void SitCamPosition()
    {
        // 카메라 위치 변경
        Camera.main.transform.position = sittingCamPos.position;
    }

    // 서는 동작을 정의합니다.
    private void StandCamPosition()
    {
        // 카메라 위치 변경
        Camera.main.transform.position = standingCamPos.position;
    }

    private void jumpCamPosition()
    {
        Camera.main.transform.position = jumpCamPos.position;
    }

    /*
    private void Crouch()
    {
        float crouchValue = inputManager.inputMaster.Movement.Crouch.ReadValue<float>();
        _isCrouch = crouchValue > 0.0f;

        if (_isCrouch)
        {
            Vector3 newPosition = playerCamera.transform.position;
            newPosition.y = inputManager.inputMaster.Movement.Crouch.ReadValue<float>() == 0 ? 1f : 0.7f;
            playerCamera.transform.position = newPosition;
        }
        else
        {
            playerCamera.transform.localPosition = originalCameraLocalPosition;
        }

        m_Animator.SetBool("Crouch", _isCrouch);
    }
    */


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