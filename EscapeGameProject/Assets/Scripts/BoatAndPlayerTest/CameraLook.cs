using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraLook : MonoBehaviour
{
    public InputManager inputManager;
    public float mouseSensitivity = 25f;
    public Transform body;

    private float xRotation = 0f;

    private PlayerStatus playerMove;

    public bool IsMenuOpen;

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.InvisibleCursor();
    }

    void Update()
    {
        if (!IsMenuOpen) // 평소
        {
            Debug.Log("움직임");

            float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
            float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);
        }
        else // 그 외에는 못움직임
        {
            Debug.Log("못움직임");


            float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * 0;
            float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * 0;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);
        }
    }

    public void OnMouseMoveStop()
    {
        IsMenuOpen = false;
    }

    public void OnMouseMove()
    {
        IsMenuOpen = true;
    }


}
