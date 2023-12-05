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
    [Tooltip("�ȴ� �ӵ�")]
    public float speed = 10;
    [Tooltip("�پ�� �ӵ�")]
    public float runSpeed = 15;
    [Tooltip("�����ϴ� ��(����)")]
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
    
    // ĳ���� �⺻ ���
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;
    
    // ī�޶� ��ġ ����
    public Transform cameraObject;
    private Vector3 originalCameraLocalPosition;

    // ���� ���� ���� Ȯ��, �����Կ� �������� ���� �� true. -> �����Կ���
    public bool isKnife;
    public bool isHammer;
    public bool isPickaxe;
    public bool isTorch;

    // Į
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameObject knifeOnShoulder;
    // ����
    [SerializeField]
    private GameObject hammer;
    [SerializeField]
    private GameObject hammerOnShoulder;
    // ���
    [SerializeField]
    private GameObject pickaxe;
    [SerializeField]
    private GameObject pickaxeOnShoulder;
    //Ƚ��
    

    // ���� �����ϰ� �ʹ�
    private bool changeKnife;
    private bool changeHammer;
    private bool changePickaxe;

    public bool isEquipping;
    public bool isEquipped;


    //����
    public bool isBlocking;

    //������
    public bool isKicking;

    //���� ���
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    // ���� ����
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
    /// Input System���� �����̴� �ڵ�
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

    // �÷��̾ ���� �������� ������ �ִ��� Ȯ���ϴ� �Լ�
    public void SetHasKnife(bool hasItem)
    {
        isKnife = hasItem;
        
    }
    public void SetHasHammer(bool hasItem)
    {
        isHammer = hasItem;
    }
    public void SetHasPickaxe(bool hasItem)
    {
        isPickaxe = hasItem;
    }
    public void SetHasTouch(bool hasItem)
    {
        isTorch = hasItem;
    }

    public void playerHasWeaponItem()
    {
        if (knifeOnShoulder.activeSelf == true)
        {
            Debug.Log("Į�� Ȱ��ȭ�� ����");
            isKnife = true;
            m_Animator.SetBool("isKnife", true);
        }
        else
        {
            Debug.Log("Į�� ��Ȱ��ȭ�� ����");
            isKnife = false;
            m_Animator.SetBool("isKnife", false);
        }

        if (hammer != null)
        {
            isHammer = true;
            m_Animator.SetBool("isHammer", true);
        }
        else
        {
            isHammer= false;
            m_Animator.SetBool("isHammer", false);
        }

        if (pickaxe != null)
        {
            isPickaxe = true;
            m_Animator.SetBool("isPickaxe", true);
        }
        else
        {
            isPickaxe = false;
            m_Animator.SetBool("isPickaxe", false);
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

    /* ���ⱳü
    public void ChangeWeapon()
    {
       

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Player�� ã��
            GameObject player = GameObject.Find("Player");

            // Player�� �ִٸ�
            if (player != null)
            {
                // Player�� �ڽĵ��� ã�� Weapon�� �˻�
                Transform weapon = player.transform.Find("M_Nurse_Basic/_M_Rig/DEF-spine/DEF-spine.001/DEF-spine.002/DEF-spine.003/DEF-shoulder.R/DEF-upper_arm.R/DEF-forearm.R/DEF-hand.R/Weapon");

                // Weapon�� �ִٸ�
                if (weapon != null)
                {
                    // ������ �ڽ� GameObject�� ����
                    foreach (Transform child in weapon)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    // ���� ������ �迭 ���̿� ���� ���� ��ȣ ����
                    weaponNum %= weaponPrefabs.Length;

                    // ���ο� ������ �������� �ν��Ͻ�ȭ�ϰ� Weapon�� �ڽ����� ����
                    GameObject newWeapon = Instantiate(weaponPrefabs[weaponNum], weapon);
                    weaponNum++; // ���� ���� ��ȣ�� �̵�
                    
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
        float changeKnifeValue = inputManager.inputMaster.Movement.ChangeKnife.ReadValue<float>();
        float changeHammerValue = inputManager.inputMaster.Movement.ChangeHammer.ReadValue<float>();
        float changePickaxeValue = inputManager.inputMaster.Movement.ChangePickaxe.ReadValue<float>();
    
        if (m_Animator.GetBool("OnGround"))
        {
            // Į
            if (isKnife == true)
            {
                if(changeKnifeValue > 0)
                {
                    
                    m_Animator.SetBool("isUnarmed", true);
                    isEquipping = true;
                    m_Animator.SetTrigger("knifeEquip");
                    changeKnife = true;
                }
 
               

            }
            // ����        
            if (isHammer == true)
            {
                if(changeHammerValue > 0)
                {
                    m_Animator.SetBool("isUnarmed", true);
                    isEquipping = true;
                    m_Animator.SetTrigger("hammerEquip");
                    changeHammer = true;
                }
 
                
               
            }
            // ���
            if (isPickaxe == true)
            {
                if(changePickaxeValue > 0)
                {
                    m_Animator.SetBool("isUnarmed", true);
                    isEquipping = true;
                    m_Animator.SetTrigger("pickaxeEquip");
                    changePickaxe = true;
                }
            

            }

          
        }



    }

    public void ActiveWeapon()
    {

        if (!isEquipped)
        {
                    
            if (isKnife == true && changeKnife == true)         // Į
            {
                knife.SetActive(true);
                knifeOnShoulder.SetActive(false);
                Debug.Log("Į ����");
          
            }
            
            if (isHammer == true && changeHammer == true)       // ��ġ
            {
                hammer.SetActive(true);
                hammerOnShoulder.SetActive(false);
                Debug.Log("���� ����");

            }
            
            if (isPickaxe == true && changePickaxe == true)     // ���
            {
                pickaxe.SetActive(true);
                pickaxeOnShoulder.SetActive(false);
                Debug.Log("��� ����");
               
            }
            isEquipped = !isEquipped;

        }
        else
        {
            
          
            if(isKnife == true)            // Į
            {
                knife.SetActive(false);
                knifeOnShoulder.SetActive(true);
                Debug.Log("Į ����");
  

            }            
            
            if(isHammer == true)         // ��ġ
            {
                hammer.SetActive(false);
                hammerOnShoulder.SetActive(true);
                Debug.Log("�ظ� ����");


            }

            if (isPickaxe == true)         // ���
            {
                pickaxe.SetActive(false);
                pickaxeOnShoulder.SetActive(true);
                Debug.Log("��� ����");
  

            }
            changeKnife = false;
            changeHammer = false;
            changePickaxe = false;
            m_Animator.SetBool("isUnarmed", false);
            isEquipped = !isEquipped;
        }



      
    }

    private void Sprint()
    {
        float sprintValue = inputManager.inputMaster.Movement.Run.ReadValue<float>();
        _isSprint = sprintValue > 0.0f;


        if (_isSprint)
        {
            m_Animator.SetFloat("Speed_f", 2f); // Speed_f ���� ����
            //Debug.Log("�ٴ� ��");
        }
        else
        {
            m_Animator.SetFloat("Speed_f", 1f); // Speed_f ���� ����
            //Debug.Log("�ٱ� ��");
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