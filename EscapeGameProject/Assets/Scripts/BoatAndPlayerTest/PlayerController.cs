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


    // Į
    [SerializeField]
    private GameObject knifeOnHand;
    [SerializeField]
    private GameObject knifeOnShoulder;
    // ����
    [SerializeField]
    private GameObject hammeOnHand;
    [SerializeField]
    private GameObject hammerOnShoulder;
    // ���
    [SerializeField]
    private GameObject pickaxeOnHand;
    [SerializeField]
    private GameObject pickaxeOnShoulder;
    //Ƚ��
    [SerializeField]
    private GameObject torchOnHand;
    [SerializeField]
    private GameObject torchOnShoulder;

    //  
    public bool isEquipping; // ���� ��
    public bool isEquipped;  // ���� ��

    // 1,2,3,4 �� Ű �ݺ��Ͽ� ���� Ƚ��
    private bool repeatClick1 = false;
    private bool repeatClick2 = false;
    private bool repeatClick3 = false;
    private bool repeatClick4 = false;

    // ���� ���� ���� Ȯ��, �����Կ� �������� ���� �� true. -> �����Կ���
    // �κ��丮�� ����
    public bool isKnifeInventory;
    public bool isHammerInventory;
    public bool isPickaxeInventory;
    public bool isTorchInventory;


    // ������ Ʈ���ŷ� ����. -> �������̸� ����, ���� ���� �ƴϰ�, is~~���� true�̸� ����
    private bool knifeChangeTrigger;
    private bool hammerChangeTrigger;
    private bool pickaxeChangeTrigger;
    private bool torchChangeTrigger;


    // ���� �տ� �������� ����� Bool��
    private bool knifeEquipOnHand;
    private bool hammerEquipOnHand;
    private bool pickaxeEquipOnHand;
    private bool torchEquipOnHand;

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


    // �÷��̾ ���� �������� ������ �ִ��� Ȯ���ϴ� �Լ�
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

            // Į
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
            // ����        

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
            // ���
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
            // Ƚ��
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
            // Į
            if (knifeChangeTrigger == true)
            {
                m_Animator.SetBool("KnifeOnShoulder", false);
                knifeOnHand.SetActive(true);
                knifeOnShoulder.SetActive(false);
                knifeChangeTrigger = false;
                knifeEquipOnHand = true;  // �տ� ����
                Debug.Log("Į ����");
            }
            // ��ġ
            else if (hammerChangeTrigger == true)
            {
                m_Animator.SetBool("HammerOnShoulder", false);
                hammeOnHand.SetActive(true);
                hammerOnShoulder.SetActive(false);
                hammerChangeTrigger = false;
                hammerEquipOnHand = true;
                Debug.Log("���� ����");

            }
            // ���
            else if (pickaxeChangeTrigger == true)
            {
                m_Animator.SetBool("PickaxeOnShoulder", false);
                pickaxeOnHand.SetActive(true);
                pickaxeOnShoulder.SetActive(false);
                pickaxeChangeTrigger = false;
                pickaxeEquipOnHand = true;
                Debug.Log("��� ����");

            }
            // Ƚ��
            else if (torchChangeTrigger == true)
            {
                m_Animator.SetBool("TorchOnShoulder", false);
                torchOnHand.SetActive(true);
                torchOnShoulder.SetActive(false);
                torchChangeTrigger = false;
                torchEquipOnHand = true;
                Debug.Log("Ƚ�� ����");
            }
            isEquipped = !isEquipped;

        }
        else
        {

            // Į
            if (knifeEquipOnHand == true)
            {
                m_Animator.SetBool("KnifeOnShoulder", true);
                knifeOnHand.SetActive(false);
                knifeOnShoulder.SetActive(true);
                knifeEquipOnHand = false;
                Debug.Log("Į ����");
            }
            // ��ġ
            else if (hammerEquipOnHand == true)
            {
                m_Animator.SetBool("HammerOnShoulder", true);
                hammeOnHand.SetActive(false);
                hammerOnShoulder.SetActive(true);
                hammerEquipOnHand = false;
                Debug.Log("�ظ� ����");
            }
            // ���
            else if (pickaxeEquipOnHand == true)
            {
                m_Animator.SetBool("PickaxeOnShoulder", true);
                pickaxeOnHand.SetActive(false);
                pickaxeOnShoulder.SetActive(true);
                pickaxeEquipOnHand = false;
                Debug.Log("��� ����");
            }
            // Ƚ��
            else if (torchEquipOnHand == true)
            {
                m_Animator.SetBool("TorchOnShoulder", true);
                torchOnHand.SetActive(false);
                torchOnShoulder.SetActive(true);
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
            Debug.Log("���");
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }


}