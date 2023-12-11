using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCameraLook : MonoBehaviour
{
    public InputManager inputManager;
    public float mouseSensitivity = 10f;
    public Transform body;

    private float xRotation = 0f;
    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity *
                        Time.deltaTime;
        float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity *
                       Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        // body.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.localRotation = Quaternion.Euler(
        Mathf.Clamp(xRotation, -60f, 60f),
        Mathf.Clamp(yRotation, -60f, 60f),
        0f
    );
    }
}
