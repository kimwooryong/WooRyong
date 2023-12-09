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
    public bool _isAttack;

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

    // 도끼
    [SerializeField]
    private GameObject axeOnHand;

    // 곡괭이
    [SerializeField]
    private GameObject pickaxeOnHand;

    //횟불
    [SerializeField]
    private GameObject torchOnHand;


    //  
    public bool isEquipping; // 장착 중
    public bool isEquipped;  // 장착 됨

    // 1,2,3,4 번 키 반복하여 누른 횟수
    //private bool repeatClickKnife = false;
    //private bool repeatClickAxe = false;
    //private bool repeatClicPickaxe = false;
    //private bool repeatClickTorch = false;

    // 무기 소지 여부 확인, 퀵슬롯에 아이템이 있을 때 true. -> 퀵슬롯에서
    // 인벤토리에 존재
    public bool isKnifeInventory;
    public bool isAxeInventory;
    public bool isPickaxeInventory;
    public bool isTorchInventory;
    public bool isFoodInventory;


    // 누르면 트리거로 실행. -> 장착중이면 해제, 장착 중이 아니고, is~~값이 true이면 장착
    private bool knifeChangeTrigger;
    private bool axeChangeTrigger;
    private bool pickaxeChangeTrigger;
    private bool torchChangeTrigger;
    private bool foodChangeTrigger;

    // 현재 손에 장착중인 장비의 Bool값
    private bool knifeEquipOnHand;
    private bool axeEquipOnHand;
    private bool pickaxeEquipOnHand;
    private bool torchEquipOnHand;
    private bool foodEquipOnHand;

    //막기
    public bool isBlocking;

    //발차기
    public bool isKicking;

    //공격 요소
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;



    private void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();
        inputManager.inputMaster.Movement.Attack.started += _ => AttackMonster();

        playerCamera = GetComponentInChildren<Camera>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

   

    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        Moving();
        Crouch();
        Sprint();

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
        _isAttack = attackValue >0; // 공격가능
        if (_isAttack && m_Animator.GetBool("OnGround") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            //currentAttack++;
            isAttacking = true; // 공격 중

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
        m_Animator.SetBool("WeaponInventory", hasItem);
    }
    public void SetHasAxe(bool hasItem)
    {
        isAxeInventory = hasItem;
        m_Animator.SetBool("WeaponInventory", hasItem);
    }
    public void SetHasPickaxe(bool hasItem)
    {
        isPickaxeInventory = hasItem;
        m_Animator.SetBool("WeaponInventory", hasItem);
    }
    public void SetHasTorch(bool hasItem)
    {
        isTorchInventory = hasItem;
        m_Animator.SetBool("WeaponInventory", hasItem);
    }

    public void SetHasFood(bool hasItem)
    {
        isFoodInventory = hasItem;
        m_Animator.SetBool("FoodInventory", hasItem) ;
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
            m_Animator.SetBool("isArmed", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            isEquipping = true;

            knifeChangeTrigger = true;
            axeChangeTrigger = false;
            pickaxeChangeTrigger = false;           
            torchChangeTrigger = false;

        }

       

    }
    public void EquipPickaxe()
    {
        if (m_Animator.GetBool("OnGround") &&  isPickaxeInventory == true)
        {
            m_Animator.SetBool("isArmed", true); 
            m_Animator.SetTrigger("WeaponEquipTrigger");
            isEquipping = true;

            knifeChangeTrigger = false;
            axeChangeTrigger = false;
            pickaxeChangeTrigger = true;
            torchChangeTrigger = false;

        }

      

    }
    public void EquipAxe()
    {
        if (m_Animator.GetBool("OnGround") &&  isAxeInventory == true)
        {
            m_Animator.SetBool("isArmed", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            isEquipping = true;

            knifeChangeTrigger = false;
            axeChangeTrigger = true;
            pickaxeChangeTrigger = false;
            torchChangeTrigger = false;      
        }

       

    }
    public void EquipTorch()
    {
        if (m_Animator.GetBool("OnGround") && isTorchInventory == true)
        {
            m_Animator.SetBool("isArmed", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            isEquipping = true;
            
            knifeChangeTrigger = false;
            axeChangeTrigger = false;
            pickaxeChangeTrigger = false;
            torchChangeTrigger = true;
        }

      
    }

    public void NotEquipTheOthers()
    {
        
        if (knifeChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            knifeOnHand.SetActive(false);
            knifeEquipOnHand = false;
            Debug.Log("NotEquipTheOthers 칼 해제");
        }
        if (pickaxeChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            pickaxeOnHand.SetActive(false);
            pickaxeEquipOnHand = false;
            Debug.Log("NotEquipTheOthers 곡괭이 해제");
        }
        if (torchChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            torchOnHand.SetActive(false);
            torchEquipOnHand = false;
            Debug.Log("NotEquipTheOthers 횟불 해제");
        }
        if (axeChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            axeOnHand.SetActive(false);
            axeEquipOnHand = false;
            Debug.Log("NotEquipTheOthers 도끼 해제");
        }
        if (foodChangeTrigger == false)
        {
            ItemManager.Instance.DeleteFoodOnHand();
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

        if (m_Animator.GetBool("OnGround") && isFoodInventory == true)
        {
            m_Animator.SetBool("isArmed", false);
            isEquipping = false;
            foodChangeTrigger = true;

            knifeChangeTrigger = false;
            axeChangeTrigger = false;
            pickaxeChangeTrigger = false;
            torchChangeTrigger = false;

            // 무기 집어 넣기
            knifeOnHand.SetActive(false);
            axeOnHand.SetActive(false);
            pickaxeOnHand.SetActive(false);
            torchOnHand.SetActive(false);

            // 마우스 왼쪽 버튼 누르면 손에 음식 삭제 및 음식 섭취 효과 발생
            if (Input.GetMouseButtonDown(0))
            {
                _isAttack = false;
                ItemManager.Instance.UseFood(itemID);
                ItemManager.Instance.DeleteFoodOnHand();
            }

          


        }
    }
    #endregion

    public void ActiveWeapon()
    {

        if (!isEquipped)
        {
            // 무기 소환

            // 칼
            if(knifeChangeTrigger == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", false);
                knifeOnHand.SetActive(true);
                //knifeOnShoulder.SetActive(false);
                knifeChangeTrigger = false;

                // 손에 장착
                knifeEquipOnHand = true;  
                axeEquipOnHand = false;
                pickaxeEquipOnHand = false;
                torchEquipOnHand = false;
                Debug.Log("칼 장착");
            }
            // 망치
            else if(axeChangeTrigger == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", false);
                axeOnHand.SetActive(true);
                //axeOnShoulder.SetActive(false);
                axeChangeTrigger = false;

                knifeEquipOnHand = false;
                axeEquipOnHand = true;
                pickaxeEquipOnHand = false;
                torchEquipOnHand = false;
                Debug.Log("도끼 장착");

            }
            // 곡괭이
            else if (pickaxeChangeTrigger == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", false);
                pickaxeOnHand.SetActive(true);
                //pickaxeOnShoulder.SetActive(false);
                pickaxeChangeTrigger = false;

                knifeEquipOnHand = false;
                axeEquipOnHand = false;
                pickaxeEquipOnHand = true;
                torchEquipOnHand = false;
                Debug.Log("곡괭이 장착");

            }
            // 횟불
            else if(torchChangeTrigger == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", false);
                torchOnHand.SetActive(true);
                //torchOnShoulder.SetActive(false);
                torchChangeTrigger = false;
                
                knifeEquipOnHand = false;
                axeEquipOnHand = false;
                pickaxeEquipOnHand = false;
                torchEquipOnHand = true;
                Debug.Log("횟불 장착");
            }    
            else if (foodChangeTrigger == true)
            {
                m_Animator.SetBool("isArmed", false);
                //foodOnHand.SetActive(true);
                //foodOnShoulder.SetActive(false);
                foodChangeTrigger = false;
                foodEquipOnHand = true;

                knifeOnHand.SetActive(false);
                axeOnHand.SetActive(false);
                pickaxeOnHand.SetActive(false);
                torchOnHand.SetActive(false);
                Debug.Log("음식 장착");
            }
            isEquipped = !isEquipped;

        }
        else
        {
            // 무기 집어넣기

            // 칼
            if (knifeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                knifeOnHand.SetActive(false);
                //knifeOnShoulder.SetActive(true);
                knifeEquipOnHand = false;
                Debug.Log("칼 해제");
            }
            // 망치
            else if (axeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                axeOnHand.SetActive(false);
                //axeOnShoulder.SetActive(true);
                axeEquipOnHand = false;
                Debug.Log("해머 해제");
            }
            // 곡괭이
            else if (pickaxeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                pickaxeOnHand.SetActive(false);
                //pickaxeOnShoulder.SetActive(true);
                pickaxeEquipOnHand = false;
                Debug.Log("곡괭이 해제");
            }
            // 횟불
            else if (torchEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                torchOnHand.SetActive(false);
                //torchOnShoulder.SetActive(true);
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (_isGrounded)
            {            
                rb.AddForce(Vector3.up * jumpForce);
                jumpCamPosition();
                _isJump = true;          
            }
            else
            {             
                StandCamPosition();
                _isJump = false;
            }
        }     
    }

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