using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    float right;
    float forward;
    float crouch;

    const float k_Half = 0.5f;
    //
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;
    //
    public Transform cameraObject;
    private Vector3 originalCameraLocalPosition;

    private void Start()
    {
        inputManager.inputMaster.Movement.Jump.started += _ => Jump();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        originalCameraLocalPosition = cameraObject.transform.localPosition;


    }

    private void Update()
    {
        Moving();
        Crouch();
        Sprint();
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


    private void Sprint()
    {
        float sprintValue = inputManager.inputMaster.Movement.Run.ReadValue<float>();
        _isSprint = sprintValue > 0.0f;


        if (_isSprint)
        {
            m_Animator.SetBool("Running", true); // Running 블렌드 트리로 전환
            m_Animator.SetFloat("Speed_f", 2f); // Speed_f 값을 변경
            Debug.Log("뛰는 중");
        }
        else
        {
            m_Animator.SetBool("Running", false); // 원래 블렌드 트리로 전환
            m_Animator.SetFloat("Speed_f", 1f); // Speed_f 값을 변경
            Debug.Log("뛰기 끝");
        }
    }




    void Jump()
    {

        if (_isGrounded)
        {

            rb.AddForce(Vector3.up * jumpForce);

            Vector3 newPosition = cameraObject.transform.position;
            newPosition.y = inputManager.inputMaster.Movement.Jump.ReadValue<float>() == 0 ? 3f : 2.5f;
            cameraObject.transform.position = newPosition;


            _isJump = true;

        }
        else
        {
            _isJump = false;
            cameraObject.transform.localPosition = originalCameraLocalPosition;
        }

    }




    void Crouch()
    {
        float crouchValue = inputManager.inputMaster.Movement.Crouch.ReadValue<float>();
        _isCrouch = crouchValue > 0.0f;

        if (_isCrouch)
        {
            Vector3 newPosition = cameraObject.transform.position;
            newPosition.y = inputManager.inputMaster.Movement.Jump.ReadValue<float>() == 0 ? 1f : 0.72618f;
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