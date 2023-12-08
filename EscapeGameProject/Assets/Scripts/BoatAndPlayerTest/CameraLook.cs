using UnityEngine;
using UnityEngine.UIElements;

public class CameraLook : MonoBehaviour
{
    public InputManager inputManager;
    public float mouseSensitivity = 25f;
    public Transform body;

    private float xRotation = 0f;

    private PlayerStatus playerMove;

    //private bool IsMenuOpen = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetAllUIInactive();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();

    }

    public GameObject Build_Base;
    public GameObject Inventory_BAse;
    public GameObject Fire_Base;
    public GameObject Craft_Base;

    private bool isUIActive = false;

    void ActivateUI(GameObject activeUI)
    {
        SetAllUIInactive();

        activeUI.SetActive(true);
    }

    void SetAllUIInactive()
    {
        Build_Base.SetActive(false);
        Inventory_BAse.SetActive(false);
        Fire_Base.SetActive(false);
        Craft_Base.SetActive(false);
    }
    private void CameraMove()
    {
        float mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity *
                Time.deltaTime;
        float mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity *
                       Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }



}
