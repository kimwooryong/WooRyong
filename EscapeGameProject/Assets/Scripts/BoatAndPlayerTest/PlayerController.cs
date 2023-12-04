using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


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

    // 무기 장착 여부 bool
    [SerializeField] bool isWeapon;
    [SerializeField] bool isKnife;

    // 장착 유무 확인 요소
    // 도끼 창 곡괭이
    [SerializeField]
    private GameObject weapons;
    [SerializeField]
    private GameObject weaponsOnShoulder;
    // 칼
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameObject knifeOnShoulder;


    public bool isEquipping;
    public bool isEquipped;


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



    private void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();
        inputManager.inputMaster.Movement.Attack.started += _ => AttackMonster();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        originalCameraLocalPosition = cameraObject.transform.localPosition;



    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        playerHasWeaponItem();

        Moving();
        Crouch();
        Sprint();

        AttackMonster();
        Equip();
        Block();
        Kick();

        Debug.Log($"장착: {isEquipped}");

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

    // 플레이어가 무기 아이템을 가지고 있는지 확인하는 함수
    public void playerHasWeaponItem()
    {
        if(weapons != null)
        {
            isWeapon = true;
            m_Animator.SetBool("isWeapon", true);
        }
        else
        {
            isWeapon= false;
            m_Animator.SetBool("isWeapon", false);
        }

        if(knife != null)
        {
            isKnife = true;
            m_Animator.SetBool("isKnife", true);
        }
        else 
        { 
            isKnife= false;
            m_Animator.SetBool("isKnife", false);
        }
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
        if (attackValue >0 && m_Animator.GetBool("OnGround") && timeSinceAttack > 0.8f)
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
        if (kickValue >0 && m_Animator.GetBool("OnGround"))
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

    private void Equip()
    {
        
        // 칼
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_Animator.GetBool("OnGround") && isKnife == true)
        {
            isEquipping = true;
            m_Animator.SetTrigger("knifeEquip");
        } 


        // 도끼 창 곡괭이
        if (Input.GetKeyDown(KeyCode.Alpha2) && m_Animator.GetBool("OnGround")  && isWeapon == true)
        {
            isEquipping = true;
            m_Animator.SetTrigger("weaponEquip");
        }
        

    }

    public void ActiveWeapon()
    {
        // 도끼 곡괭이 창
        if (!isEquipped && isWeapon == true)
        {
            weapons.SetActive(true);
            weaponsOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
            Debug.Log("도끼 장착");
        }
        else 
        {
            weapons.SetActive(false);
            weaponsOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
            Debug.Log("도끼 해제");
        }

        // 칼
        if (!isEquipped && isKnife == true)
        {
            knife.SetActive(true);
            knifeOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
            Debug.Log("칼 장착");
        }
        else 
        {
            knife.SetActive(false);
            knifeOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
            Debug.Log("칼 해제");
        }

      
    }

    private void Sprint()
    {
        float sprintValue = inputManager.inputMaster.Movement.Run.ReadValue<float>();
        _isSprint = sprintValue > 0.0f;


        if (_isSprint)
        {
            // m_Animator.SetBool("Running", true); // Running 블렌드 트리로 전환
            m_Animator.SetFloat("Speed_f", 2f); // Speed_f 값을 변경
            Debug.Log("뛰는 중");
        }
        else
        {
            // m_Animator.SetBool("Running", false); // 원래 블렌드 트리로 전환
            m_Animator.SetFloat("Speed_f", 1f); // Speed_f 값을 변경
            Debug.Log("뛰기 끝");
        }
    }




    private void Jump()
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
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }


}