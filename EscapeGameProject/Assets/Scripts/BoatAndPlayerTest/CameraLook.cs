using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public InputManager inputManager;
    public float mouseSensitivity = 25f;
    public Transform body;

    private float xRotation = 0f;

    private PlayerStatus playerMove;

    private bool IsMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMenuOpen)  // UI가 열려있지 않은 경우에만 카메라 움직임 수행
        {
            float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
            float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);
        }
    }

    // UI가 열릴 때 호출되는 함수 (예: 게임 메뉴 열릴 때)
    public void OnMenuOpen()
    {
        IsMenuOpen = true;
    }

    // UI가 닫힐 때 호출되는 함수 (예: 게임 메뉴 닫힐 때)
    public void OnMenuClose()
    {
        IsMenuOpen = false;
    }


}
