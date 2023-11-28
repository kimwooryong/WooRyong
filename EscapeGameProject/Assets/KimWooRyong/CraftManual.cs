using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[System.Serializable]
public class craft
{
    public string craftName; // �̸�
    public GameObject go_Prefab; // ���� ��ġ�Ǵ� ������
    public GameObject go_PreviewPrefab; // �̸����� ������
}

public class prefabs
{

}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false; // ���º��� ���ΰ� ����
    private bool isPreviewActived = false; // ������ Ȱ��ȭ

    public Dictionary<int, GameObject> buildBlocks = new Dictionary<int, GameObject>();


    [SerializeField]
    private GameObject go_BaseUI; // �⺻ ���̽�


    [SerializeField]
    public craft[] craft_fire; // ��ںҿ� ��

    
    private GameObject go_Preview; // �̸����� �������� ���� ����

    private PreviewObject previewObject; // Ŭ���� ����

    private Quaternion savedRotation;


    private GameObject go_Prefab; // ��ü ������ �������� ���� ����


    [SerializeField]
    private Transform tf_Player; // �÷��̾� ��ġ


    // RaycastHit �ʿ� ���� ����
    private RaycastHit hitInfo; // ����
    [SerializeField]
    private LayerMask layerMask; // ���̾� ����ũ ����
    [SerializeField]
    private float range; // �Ÿ�


    public void SlotClick(int _slotNumber) // ���� Ŭ��
    {
        Debug.Log("0");
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        previewObject = go_Preview.GetComponent<PreviewObject>();

        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActived = true; // ������ ���ֱ�
        go_BaseUI.SetActive(false); // UI ���ֱ�
        
    }

    void Update()
    {
        RayObject(); // ���� ������Ʈ ���� Ȯ�ο�
        if (go_Preview != null)
        {
            savedRotation = go_Preview.transform.rotation;
            go_Prefab.transform.rotation = go_Preview.transform.rotation;
        }
        InputHandler();
    }

    private void LateUpdate()
    {
        if (isPreviewActived) // ������ Ȱ��ȭ
        {
            PreviewPositionUpdate(); // ������ ������
        }
    }

    public float maxRaycast; // ������Ʈ Ȯ�� ���� �Ÿ� 
    public GameObject crosshairPrefab; // ũ�ν���� ������
    private GameObject crosshairInstance; // ũ�ν����

    private void RayObject() // ���Ͼ� �帱 ���� ������ Ȯ�ο�
    {
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.blue); // �� â�� ���̼��� ���̰� ���ִ� �ڵ�
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ī�޶� ���� ����� ���� �߻�

        RaycastHit hit; // ������

        if (Physics.Raycast(ray, out hit, maxRaycast))
        {
            Debug.Log("���� ������Ʈ " + hit.collider.gameObject.name); // ���̿� �浹�� ������Ʈ�� �̸��� ǥ��

            crosshairPrefab.gameObject.SetActive(true);

            Vector3 crosshairPosition = hit.point; // ũ�ν��� ǥ���� ��ġ ����

            UpdateCrosshair(crosshairPosition); // ũ�ν��� ǥ���ϰų� ������Ʈ
        }
        else
        {
            Debug.Log("HideCrosshair");
            crosshairPrefab.gameObject.SetActive(false);
        }
    }
    void UpdateCrosshair(Vector3 position)
    {
        // ũ�ν��� ǥ���� ��ġ�� �ν��Ͻ��� �����ϰų� ��ġ ������Ʈ
        if (crosshairInstance == null)
        {
            crosshairInstance = Instantiate(crosshairPrefab, position, Quaternion.identity);
        }
        else
        {
            crosshairInstance.transform.position = position;
        }
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isPreviewActived) // UI���� �ߺ����� ����
        {
            Window(); // UI
        }

        if (Input.GetButtonDown("Fire1")) // ���콺 ��Ŭ��
        {
            Build(); // ��ġ
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cancel(); // ���
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel(); // ���
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, 90f * Time.deltaTime, 0f)); // ȸ��
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, -90f * Time.deltaTime, 0f)); // ȸ��
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            if (go_Preview != null)
            {
                go_Preview.transform.Rotate(new Vector3(0f, 0f, -90f * Time.deltaTime)); // ȸ��
            }
        }
    }

    private void Build() // �Ǽ�
    {
        if (isPreviewActived == true && previewObject.isBuildable() == true)
        {
            Vector3 buildPosition = go_Preview.transform.position;
            Quaternion buildRotation = go_Preview.transform.rotation;

            GameObject newObject = Instantiate(go_Prefab, buildPosition, buildRotation); // ����� ��ġ��, �����̼ǰ��� �ޱ�
            Destroy(go_Preview); // ������ ������Ʈ ����
            isActivated = false; // ���º��� ���ΰ� ����
            isPreviewActived = false; // ������ ���ֱ�
            go_Prefab = null;
            go_Preview = null;
            previewObject = null;
        }
    }

    private void PreviewPositionUpdate() // ������ ��ġ�� ������
    {
        Debug.Log("1");
        Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, Color.red); // �� â�� ���̼��� ���̰� ���ִ� �ڵ�
        if(Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, out hitInfo, range, layerMask)) // ī�޶� ������ ���̸� ��� ��ġ���� �����ְ� ���̾� ����ũ�� Ȯ���Ѵ�
        {
            if(hitInfo.transform != null) // ��ġ���� �ִٸ�
            {
                Quaternion rotation = go_Preview.transform.rotation;
                Vector3 _location = previewObject.GetSnapPosition(hitInfo.point, ref rotation); 
                Debug.Log("4");
                go_Preview.transform.position = _location; // ����� ��ġ���� �����並 �����ش�
                go_Preview.transform.rotation = rotation;
            }
        }
    }

  
        // ���� �Ǽ� Ȯ���ϸ� ������ ������Ʈ�� ��ġ���ʰ� ���� �� �ִ�. ( ��ġ ���� �����϶� �־��ָ� ������ )
        // BoxCollider box;
        // box.bounds.extents.x;
    

    private void Cancel() // ���
    {
        if (isPreviewActived)
        {
            Destroy(go_Preview);
        }
        isActivated = false;
        isPreviewActived= false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if(!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        Cursor.lockState = CursorLockMode.None; // ���콺 ���� ����
        Cursor.visible = true; // ���콺 Ŭ������
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ����
        Cursor.visible = false; // ���콺 Ŭ���Ұ�
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

}
