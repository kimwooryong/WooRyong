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
                Debug.Log("아이템 줍기");
            }
        }
    }
    private RaycastHit hit;

    public float maxRaycast; // 오브젝트 확인 레이 거리 
    public GameObject crosshairPrefab; // 크로스헤어 프리팹
    private GameObject crosshairInstance; // 크로스헤어
    [SerializeField]
    private LayerMask itemLayer;
    public TextMeshProUGUI ItemText;

    private bool isDetecting;

    private void RayObject() // 은하씨 드릴 레이 아이템 확인용
    {
        Ray testRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // 카메라 기준 가운데에 레이 발사
        TestWhatHit(testRay);
    }
    private void TestWhatHit(Ray ray)
    {
        //RaycastHit hit;
        //아이템 스크립트 감지
        if (Physics.Raycast(ray, out hit, maxRaycast, itemLayer))
        {
            isDetecting = true;

            Debug.Log("맞은 오브젝트 " + hit.collider.gameObject.name); // 레이에 충돌한 오브젝트의 이름을 표시
            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
            if (hitItem != null)
            {
                ItemText.gameObject.SetActive(true);
                ItemText.text = hitItem.GetItemDescription();
            }
            else
            {
                Debug.Log("item 스크립트가 없다.");
            }
            crosshairPrefab.gameObject.SetActive(true);
            UpdateCrosshair(hit.point); // 크로스헤어를 표시하거나 업데이트
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
            Debug.Log("item 스크립트가 없다.");
        }
    }

    void UpdateCrosshair(Vector3 position)
    {
        // 크로스헤어를 표시할 위치에 인스턴스를 생성하거나 위치 업데이트
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
