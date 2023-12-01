using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eItemKeyColumns
{
    ID,
    Name,
    Description,
    CanCount,
    CanGrab
}
public class ItemManager : MonoBehaviour
{

    #region 싱글톤
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        SetQuickSlot();
    }
    #endregion
    private List<Dictionary<string, object>> data;
    private string itemPrefabPath = "ItemPrefabs/ItemPrefab";
    private string itemIconPath = "ItemIcons/ItemIcon";
    private string dataBasePath = "ItemDataBase";



    //키 입력
    private bool isActiveInventory;
    private bool isActiveQuickSlot;
    //배경, 왼쪽, 중앙, 오른쪽
    [SerializeField]
    private Canvas inventoryCanvas;
    [SerializeField]
    private UnityEngine.UI.Image inventoryBG;
    [SerializeField]
    private GameObject leftCanvas;
    [SerializeField]
    private GameObject middleCanvas;
    [SerializeField]
    private GameObject rightCanvas;


    private void Start()
    {
        data = CSVReader.Read(dataBasePath);
    }

    //ID로 값 전체 받아오기
    public Dictionary<string, object> ReadItemData(int itemID)
    {
        // ID에 해당하는 아이템 데이터 찾기
        foreach (var item in data)
        {
            if (item.ContainsKey(eItemKeyColumns.ID.ToString()) && Convert.ToInt32(item[eItemKeyColumns.ID.ToString()]) == itemID)
            {
                return item;
            }
        }
        return data[itemID];
    }

    //테스트용
    public void Update()
    {
        //테스트용 아이템 드랍
        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItemToField(int.Parse(idText.text));
        }
        //테스트용 아이템 획득
        if (Input.GetKeyDown(KeyCode.L))
        {
            LootItemToInventory(int.Parse(idText.text), 1);
        }
        //인벤토리 온오프
        if(Input.GetKeyDown(KeyCode.I))
        {
            isActiveInventory = !isActiveInventory;
            inventoryCanvas.gameObject.SetActive(isActiveInventory);
            if (isActiveInventory)
            {
                GameManager.Instance.VisibleCursor();
            }
            else
            {
                GameManager.Instance.InvisibleCursor();
            }
        }
        //퀵슬롯 온오프
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActiveQuickSlot = true;
            GameManager.Instance.InvisibleAndNoneCursor();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isActiveQuickSlot = false;
            GameManager.Instance.InvisibleCursor();
        }
        //인벤토리 활성화(I키)
        if (isActiveInventory)
        {
            SetCanvasInventory();
        }
        //퀵슬롯 활성화(Tab키)
        else if(isActiveQuickSlot)
        {
            SetCanvasQuickSlot();
            DetectQuickSlot();
        }
        else
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
    }

    #region 퀵슬롯

    public GameObject testUI;
    private GraphicRaycaster quickSlotGR;
    private PointerEventData ped;
    private List<RaycastResult> raycastResults;
    private float quickSlotDistance = 250f;
    private void SetQuickSlot()
    {
        quickSlotGR = inventoryCanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();
    }
    private Vector2 SetPointerPosition()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 quickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return (quickPos - screenCenter).normalized * quickSlotDistance + screenCenter;
    }
    private void DetectQuickSlot()
    {
        raycastResults.Clear();

        //Test
        testUI.transform.position = SetPointerPosition();

        ped.position = SetPointerPosition();
        quickSlotGR.Raycast(ped, raycastResults);
        if (raycastResults.Count == 0)
        {
            Debug.Log("충돌 UI없음");
            return;
        }
        GameObject preSlotObject = null;
        GameObject tempSlotObject;
        QuickSlot tempQuickSlot;
        foreach (var hit in raycastResults)
        {
            //충돌 UI가 퀵슬롯 있는지 검사.
            tempSlotObject = hit.gameObject;
            //UI 충돌이 있다면
            if(tempSlotObject != null)
            {
                //퀵슬롯이 있다면 선택
                tempQuickSlot = tempSlotObject.GetComponent<QuickSlot>();
                if(tempQuickSlot != null)
                {
                    tempQuickSlot.SetSelect();
                    //첫 검사라면 
                    if(preSlotObject == null)
                    {
                        preSlotObject = tempSlotObject;
                    }
                    //첫 검사가 아니고, 기존과 같다면
                    else if(preSlotObject == tempSlotObject)
                    {
                        return;
                    }
                    //첫 검사가 아니고, 기존과 다르다면 
                    else if(preSlotObject != tempSlotObject)
                    {
                        tempQuickSlot = preSlotObject.GetComponent<QuickSlot>();
                        tempQuickSlot.SetNonSelect();
                    }
                    else
                    {
                        Debug.Log("의도하지 않은 사항");
                    }
                }
                else
                {
                    Debug.Log("충돌UI에 퀵슬롯이 없음.");
                }
            }
            else
            {
                Debug.Log("충돌한 UI가 없음");
            }
        }
    }
    #endregion

    #region 인벤토리 캔버스 활성화 종류
    private void SetCanvasInventory()
    {
        inventoryBG.gameObject.SetActive(true);
        leftCanvas.SetActive(true);
        middleCanvas.SetActive(true);
        rightCanvas.SetActive(true);
    }
    private void SetCanvasQuickSlot()
    {
        inventoryCanvas.gameObject.SetActive(true);
        inventoryBG.gameObject.SetActive(false);
        leftCanvas.SetActive(false);
        middleCanvas.SetActive(true);
        rightCanvas.SetActive(false);
    }
    #endregion

    public Sprite LoadItemIcon(int id)
    {
        if(id == 0)
        {
            Debug.Log("그건 없습니다.");
            return null;
        }
        else
        {
            string itemIconFile = $"{itemIconPath}{id.ToString()}";
            Sprite icon = Resources.Load<Sprite>(itemIconFile);
            return icon;
        }
    }

    //플레이어 위치 찾기
    private Vector3 FindDropPos()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            Vector3 dropPos = playerGO.transform.position + playerGO.transform.forward * 2;
            return dropPos;
        }
        else
        {
            Debug.Log("플레이어 없음");
            return Vector3.zero;
        }
    }
    public TMP_InputField idText;
    
    //ID 받아와서 필드 내에 prefab 드롭함
    public void DropItemToField(int id)
    {
        if(id == 0)
        {
            Debug.Log("그런 아이템은 없다.");
            return;
        }
        else
        {
            string itemPrefabFile = $"{itemPrefabPath}{id.ToString()}";
            GameObject itemPrefab = Resources.Load(itemPrefabFile) as GameObject;
            if(itemPrefab != null)
            {
                Instantiate(itemPrefab, FindDropPos(), Quaternion.identity);
            }
            else
            {
                Debug.Log("데이터베이스에 아이템이 없다");
            }
        }
    }

    public Inventory playerInventory;
    public Inventory playerQuickSlot;
    public void LootItemToInventory(int id, int amount)
    {
        if(id == 0)
        {
            Debug.Log("그런 아이템은 없다.");
            return;
        }
        else
        {
            playerInventory.AddItem(id, amount);
        }
    }

    #region 아이템 툴팁
    [SerializeField]
    private GameObject TooltipUI;
    [SerializeField]
    private TextMeshProUGUI TooltipItemName;
    [SerializeField]
    private TextMeshProUGUI TooltipItemDescription;
    [SerializeField]
    private TextMeshProUGUI TooltipItemAmount;

    public void ShowTooltip(ItemSlot item, Vector3 cursorPos)
    {
        RectTransform tooltipRect = TooltipUI.GetComponent<RectTransform>();
        //오른쪽에 있다면 왼쪽으로 피벗생성
        if(cursorPos.x > Screen.width / 2)
        {
            tooltipRect.pivot = new Vector2(1f, 0f);
        }
        else
        {
            tooltipRect.pivot = new Vector2(0f, 0f);
        }
        TooltipUI.transform.position = cursorPos;
        TooltipUI.SetActive(true);
        TooltipItemName.text = item.itemName;
        TooltipItemDescription.text = item.itemDescription;
        TooltipItemAmount.text = $"수량 : {item.itemAmount}";
    }
    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }
    #endregion

    //A칸 아이템을 B칸 아이템과 위치 교체
    public void SwapItemSlot(ItemSlot slotA, ItemSlot slotB)
    {
        //퀵슬롯에서면 canGrab 테스트할 것.
        int tempID = slotA.itemID;
        int tempAmount = slotA.itemAmount;
        slotA.SetItemSlot(slotB.itemID, slotB.itemAmount);
        slotB.SetItemSlot(tempID, tempAmount);
    }

}
