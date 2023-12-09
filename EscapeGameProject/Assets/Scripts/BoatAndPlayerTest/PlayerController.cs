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
    public bool _isAttack;

    //
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    //  
    float attack;
    float right;
    float forward;

    // ī�޶� ��ġ ����
    private Camera playerCamera;
    //private Vector3 originalCameraLocalPosition;
    // camera ��ġ
    public Transform standingCamPos; // �� ���� �� ī�޶� ��ġ
    public Transform sittingCamPos; // ���� �� ī�޶� ��ġ
    public Transform jumpCamPos;
    //
    const float k_Half = 0.5f;

    // ĳ���� �⺻ ���
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;

    // Į
    [SerializeField]
    private GameObject knifeOnHand;

    // ����
    [SerializeField]
    private GameObject axeOnHand;

    // ���
    [SerializeField]
    private GameObject pickaxeOnHand;

    //Ƚ��
    [SerializeField]
    private GameObject torchOnHand;


    //  
    public bool isEquipping; // ���� ��
    public bool isEquipped;  // ���� ��

    // 1,2,3,4 �� Ű �ݺ��Ͽ� ���� Ƚ��
    //private bool repeatClickKnife = false;
    //private bool repeatClickAxe = false;
    //private bool repeatClicPickaxe = false;
    //private bool repeatClickTorch = false;

    // ���� ���� ���� Ȯ��, �����Կ� �������� ���� �� true. -> �����Կ���
    // �κ��丮�� ����
    public bool isKnifeInventory;
    public bool isAxeInventory;
    public bool isPickaxeInventory;
    public bool isTorchInventory;
    public bool isFoodInventory;


    // ������ Ʈ���ŷ� ����. -> �������̸� ����, ���� ���� �ƴϰ�, is~~���� true�̸� ����
    private bool knifeChangeTrigger;
    private bool axeChangeTrigger;
    private bool pickaxeChangeTrigger;
    private bool torchChangeTrigger;
    private bool foodChangeTrigger;

    // ���� �տ� �������� ����� Bool��
    private bool knifeEquipOnHand;
    private bool axeEquipOnHand;
    private bool pickaxeEquipOnHand;
    private bool torchEquipOnHand;
    private bool foodEquipOnHand;

    //����
    public bool isBlocking;

    //������
    public bool isKicking;

    //���� ���
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
        _isAttack = attackValue >0; // ���ݰ���
        if (_isAttack && m_Animator.GetBool("OnGround") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            //currentAttack++;
            isAttacking = true; // ���� ��

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


    // �÷��̾ ���� �������� ������ �ִ��� Ȯ���ϴ� �Լ�
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
            Debug.Log("NotEquipTheOthers Į ����");
        }
        if (pickaxeChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            pickaxeOnHand.SetActive(false);
            pickaxeEquipOnHand = false;
            Debug.Log("NotEquipTheOthers ��� ����");
        }
        if (torchChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            torchOnHand.SetActive(false);
            torchEquipOnHand = false;
            Debug.Log("NotEquipTheOthers Ƚ�� ����");
        }
        if (axeChangeTrigger == false)
        {
            m_Animator.SetBool("WeaponOnShoulder", true);
            m_Animator.SetTrigger("WeaponEquipTrigger");
            axeOnHand.SetActive(false);
            axeEquipOnHand = false;
            Debug.Log("NotEquipTheOthers ���� ����");
        }
        if (foodChangeTrigger == false)
        {
            ItemManager.Instance.DeleteFoodOnHand();
        }

    }
    #region ���� ����
    
        // ������ ������ �տ� �����ϴ� �ڵ�;
        //ItemManager.Instance.SetFoodOnHand(itemID);
        // �տ� �� ������ ���ִ� �ڵ�;
        //ItemManager.Instance.DeleteFoodOnHand();
        //�տ� ������ ��� ���� ��, ������ �Ծ��� �� �Ͼ�� �ϵ� (HP�� �ö󰣴ٰų�)
        //ItemManager.Instance.UseFood(itemID);
    public void EquipFood(int itemID)
    {
        //���� �տ� �ٸ� ���� ����ִٸ�, �װ� ��� ������ �տ� ���.
        // 1. �տ� ����ִ� ����? ���⸦ ����ִ´�.
        //�������� ��ȯ�ؼ�, �÷��̾� �տ� ���̴� �Լ�

        if (m_Animator.GetBool("OnGround") && isFoodInventory == true)
        {
            m_Animator.SetBool("isArmed", false);
            isEquipping = false;
            foodChangeTrigger = true;

            knifeChangeTrigger = false;
            axeChangeTrigger = false;
            pickaxeChangeTrigger = false;
            torchChangeTrigger = false;

            // ���� ���� �ֱ�
            knifeOnHand.SetActive(false);
            axeOnHand.SetActive(false);
            pickaxeOnHand.SetActive(false);
            torchOnHand.SetActive(false);

            // ���콺 ���� ��ư ������ �տ� ���� ���� �� ���� ���� ȿ�� �߻�
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
            // ���� ��ȯ

            // Į
            if(knifeChangeTrigger == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", false);
                knifeOnHand.SetActive(true);
                //knifeOnShoulder.SetActive(false);
                knifeChangeTrigger = false;

                // �տ� ����
                knifeEquipOnHand = true;  
                axeEquipOnHand = false;
                pickaxeEquipOnHand = false;
                torchEquipOnHand = false;
                Debug.Log("Į ����");
            }
            // ��ġ
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
                Debug.Log("���� ����");

            }
            // ���
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
                Debug.Log("��� ����");

            }
            // Ƚ��
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
                Debug.Log("Ƚ�� ����");
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
                Debug.Log("���� ����");
            }
            isEquipped = !isEquipped;

        }
        else
        {
            // ���� ����ֱ�

            // Į
            if (knifeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                knifeOnHand.SetActive(false);
                //knifeOnShoulder.SetActive(true);
                knifeEquipOnHand = false;
                Debug.Log("Į ����");
            }
            // ��ġ
            else if (axeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                axeOnHand.SetActive(false);
                //axeOnShoulder.SetActive(true);
                axeEquipOnHand = false;
                Debug.Log("�ظ� ����");
            }
            // ���
            else if (pickaxeEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                pickaxeOnHand.SetActive(false);
                //pickaxeOnShoulder.SetActive(true);
                pickaxeEquipOnHand = false;
                Debug.Log("��� ����");
            }
            // Ƚ��
            else if (torchEquipOnHand == true)
            {
                m_Animator.SetBool("WeaponOnShoulder", true);
                torchOnHand.SetActive(false);
                //torchOnShoulder.SetActive(true);
                torchEquipOnHand = false;
                Debug.Log("Ƚ�� ����");
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
            // �ɴ� ������ �����ϰ� ī�޶� ��ġ ����
            SitCamPosition();
        }
        else
        {
            // �� �ִ� ������ �����ϰ� ī�޶� ��ġ ����
            StandCamPosition();
        }

        m_Animator.SetBool("Crouch", _isCrouch);
    }

    // �ɴ� ������ �����մϴ�.
    private void SitCamPosition()
    {
        // ī�޶� ��ġ ����
        Camera.main.transform.position = sittingCamPos.position;
    }

    // ���� ������ �����մϴ�.
    private void StandCamPosition()
    {
        // ī�޶� ��ġ ����
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
            Debug.Log("���");
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }


}