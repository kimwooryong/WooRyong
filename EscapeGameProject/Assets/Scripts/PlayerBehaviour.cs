using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    void Update()
    {
        RayObject();
        UpdateCrosshair(); // ũ�ν��� ǥ���ϰų� ������Ʈ
        if (isDetecting == true)
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
    public Image crosshair; // ũ�ν���� ������
    public Sprite selectedCrosshairSprite;
    public Sprite nonSelectedCrosshairSprite;
    [SerializeField]
    private LayerMask itemLayer;
    public TextMeshProUGUI ItemText;

    private bool isDetecting;
    private void RayObject()
    {
        if(Camera.main != null)
        {
            Ray testRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // ī�޶� ���� ����� ���� �߻�
            TestWhatHit(testRay);
        }
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
            
        }
        else
        {
            isDetecting = false;

            ItemText.gameObject.SetActive(false);
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

    void UpdateCrosshair()
    {
        if (isDetecting)
        {
            crosshair.sprite = selectedCrosshairSprite;
        }
        else
        {
            crosshair.sprite = nonSelectedCrosshairSprite;
        }

    }
}
