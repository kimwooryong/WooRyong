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
        if (!IsMenuOpen)  // UI�� �������� ���� ��쿡�� ī�޶� ������ ����
        {
            float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
            float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);
        }
    }

    // UI�� ���� �� ȣ��Ǵ� �Լ� (��: ���� �޴� ���� ��)
    public void OnMenuOpen()
    {
        IsMenuOpen = true;
    }

    // UI�� ���� �� ȣ��Ǵ� �Լ� (��: ���� �޴� ���� ��)
    public void OnMenuClose()
    {
        IsMenuOpen = false;
    }


}
