using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void Update()
    {
        RayObject();
        if(isDetecting == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LootFieldItem();
                Debug.Log("������ �ݱ�");
            }
        }
    }
    private RaycastHit hit;

    public float maxRaycast; // ������Ʈ Ȯ�� ���� �Ÿ� 
    public GameObject crosshairPrefab; // ũ�ν���� ������
    private GameObject crosshairInstance; // ũ�ν����
    [SerializeField]
    private LayerMask itemLayer;
    public TextMeshProUGUI ItemText;

    private bool isDetecting;

    private void RayObject() // ���Ͼ� �帱 ���� ������ Ȯ�ο�
    {
        Ray testRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ī�޶� ���� ����� ���� �߻�
        TestWhatHit(testRay);
    }
    private void TestWhatHit(Ray ray)
    {
        //RaycastHit hit;
        //������ ��ũ��Ʈ ����
        if (Physics.Raycast(ray, out hit, maxRaycast, itemLayer))
        {
            isDetecting = true;

            Debug.Log("���� ������Ʈ " + hit.collider.gameObject.name); // ���̿� �浹�� ������Ʈ�� �̸��� ǥ��
            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
            if (hitItem != null)
            {
                ItemText.gameObject.SetActive(true);
                ItemText.text = hitItem.GetItemDescription();
            }
            else
            {
                Debug.Log("item ��ũ��Ʈ�� ����.");
            }
            crosshairPrefab.gameObject.SetActive(true);
            UpdateCrosshair(hit.point); // ũ�ν��� ǥ���ϰų� ������Ʈ
        }
        else
        {
            isDetecting = false;

            ItemText.gameObject.SetActive(false);
            crosshairPrefab.gameObject.SetActive(false);
        }
    }
    
    private void LootFieldItem()
    {
        Item hitItem = hit.collider.gameObject.GetComponent<Item>();
        if (hitItem != null)
        {
            ItemManager.Instance.LootItemToInventory(hitItem.GetItemID(), hitItem.GetItemAmount());
            Destroy(hitItem.gameObject);
        }
        else
        {
            Debug.Log("item ��ũ��Ʈ�� ����.");
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
}
