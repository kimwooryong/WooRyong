using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private Image inventoryBG;
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
    private int count = 0;
    public Vector3 dropPos;
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
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isActiveQuickSlot = false;
        }
        if (isActiveInventory)
        {
            inventoryBG.gameObject.SetActive(true);
            leftCanvas.SetActive(true);
            middleCanvas.SetActive(true);
            rightCanvas.SetActive(true);
        }
        else if(isActiveQuickSlot)
        {
            inventoryCanvas.gameObject.SetActive(true);
            inventoryBG.gameObject.SetActive(false);
            leftCanvas.SetActive(false);
            middleCanvas.SetActive(true);
            rightCanvas.SetActive(false);
        }
        else
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
    }
    public Sprite LoadItemIcon(int id)
    {
        if(id == 0)
        {
            Debug.Log("그건 손입니다.");
            return null;
        }
        else
        {
            string itemIconFile = $"{itemIconPath}{id.ToString()}";
            Sprite icon = Resources.Load<Sprite>(itemIconFile);
            return icon;
        }
    }

    private Vector3 FindDropPos()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            Vector3 dropPos = playerGO.transform.position + playerGO.transform.forward * 4;
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

    //아이템툴팁
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

    //A칸 아이템을 B칸 아이템과 위치 교체
    public void SwapItemSlot(ItemSlot slotA, ItemSlot slotB)
    {
        //퀵슬롯에서면 canGrab 테스트할 것.
        int tempID = slotA.itemID;
        int tempAmount = slotA.itemAmount;
        slotA.SetItemSlot(slotB.itemID, slotB.itemAmount);
        slotB.SetItemSlot(tempID, tempAmount);
    }

    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }
}
