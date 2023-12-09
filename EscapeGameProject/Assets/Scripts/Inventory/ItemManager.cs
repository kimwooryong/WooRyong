using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public enum eItemKeyColumns
{
    ID,
    Name,
    Type,
    Description,
    CanCount,
    CanGrab,
    HealingAmount,
    CanCook
}
public class ItemManager : MonoBehaviour
{
    
    #region �̱���
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
        InitializeItemManager();
    }
    #endregion
    private List<Dictionary<string, object>> data;
    private string itemPrefabPath = "ItemPrefabs/ItemPrefab";
    private string itemIconPath = "ItemIcons/ItemIcon";
    private string dataBasePath = "ItemDataBase";

    //Ű �Է�
    private bool isActiveInventory;
    private bool isActiveQuickSlot;
    //���, ����, �߾�, ������
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
    [SerializeField]
    public Image UsingItemIcon;

    public Action OpenInventory;

    private void Start()
    {
    }
    private void InitializeItemManager()
    {
        SetQuickSlot();
        data = CSVReader.Read(dataBasePath);
        preSlotObject = null;
        playerInventory.InitializeInventory();
        playerQuickSlot.InitializeInventory();
        OpenInventory += HideTooltip;
    }
    //ID�� �� ��ü �޾ƿ���
    public Dictionary<string, object> ReadItemData(int itemID)
    {
        // ID�� �ش��ϴ� ������ ������ ã��
        foreach (var item in data)
        {
            if (item.ContainsKey(eItemKeyColumns.ID.ToString()) 
                && Convert.ToInt32(item[eItemKeyColumns.ID.ToString()]) == itemID)
            {
                return item;
            }
        }
        return data[itemID];
    }

    //�׽�Ʈ��
    public void Update()
    {
        // �κ��丮 UI ����
        if (Input.GetKeyDown(KeyCode.T))
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        //�׽�Ʈ�� ������ ���
        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItemToField(int.Parse(idText.text));
        }
        //�׽�Ʈ�� ������ ȹ��
        if (Input.GetKeyDown(KeyCode.L))
        {
            LootItemToInventory(int.Parse(idText.text), 1);
        }
        //�κ��丮 �¿���
        if(Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory?.Invoke();
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
        //������ �¿���
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActiveQuickSlot = true;
            GameManager.Instance.InvisibleAndNoneCursor();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            //���
            foreach(QuickSlot slots in playerQuickSlot.InventorySlots)
            {
                slots.UseQuickSlot();
            }
            isActiveQuickSlot = false;
            GameManager.Instance.InvisibleCursor();
        }
        //�κ��丮 Ȱ��ȭ(IŰ)
        if (isActiveInventory)
        {
            SetCanvasInventory();
        }
        //������ Ȱ��ȭ(TabŰ)
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

    #region ������

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
    GameObject preSlotObject;
    private void DetectQuickSlot()
    {
        raycastResults.Clear();

        ped.position = SetPointerPosition();
        quickSlotGR.Raycast(ped, raycastResults);
        if (raycastResults.Count == 0)
        {
            Debug.Log("�浹 UI����");
            return;
        }
        GameObject tempSlotObject;
        QuickSlot tempQuickSlot;
        foreach (var hit in raycastResults)
        {
            //�浹 UI�� ������ �ִ��� �˻�.
            tempSlotObject = hit.gameObject;
            //UI �浹�� �ִٸ�
            if (tempSlotObject != null)
            {
                //���� ������ ����
                tempQuickSlot = tempSlotObject.GetComponent<QuickSlot>();
                if (tempQuickSlot != null)
                {
                    tempQuickSlot.SetSelect();
                }
                //ù �˻簡 �ƴϰ�, ������ �ٸ��ٸ� 
                if (preSlotObject != null && preSlotObject != tempSlotObject)
                {
                    tempQuickSlot = preSlotObject.GetComponent<QuickSlot>();
                    if (tempQuickSlot != null)
                    {
                        SoundManager.Instance.PlayArrangeItem();
                        tempQuickSlot.SetNonSelect();
                    }
                }
                else
                {
                    Debug.Log("�浹�� UI�� ����");
                }
                preSlotObject = tempSlotObject;
            }
        }
    }
    #endregion

    #region �κ��丮 ĵ���� Ȱ��ȭ ����
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
            Debug.Log("�װ� �����ϴ�.");
            return null;
        }
        else
        {
            string itemIconFile = $"{itemIconPath}{id.ToString()}";
            Sprite icon = Resources.Load<Sprite>(itemIconFile);
            return icon;
        }
    }

    //�÷��̾� ��ġ ã��
    private Vector3 FindDropPos()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            Vector3 dropPos = playerGO.transform.position + Vector3.up + playerGO.transform.forward * 0.3f;
            return dropPos;
        }
        else
        {
            Debug.Log("�÷��̾� ����");
            return Vector3.zero;
        }
    }
    public TMP_InputField idText;
    
    //ID �޾ƿͼ� �ʵ� ���� prefab �����
    public void DropItemToField(int id)
    {
        if(id == 0)
        {
            Debug.Log("�׷� �������� ����.");
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
                Debug.Log("�����ͺ��̽��� �������� ����");
            }
        }
    }
    //ID �޾ƿͼ� �ʵ� ���� prefab �����
    public void DropItemToField(int id, Vector3 dropPos)
    {
        if (id == 0)
        {
            Debug.Log("�׷� �������� ����.");
            return;
        }
        else
        {
            string itemPrefabFile = $"{itemPrefabPath}{id.ToString()}";
            GameObject itemPrefab = Resources.Load(itemPrefabFile) as GameObject;
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, dropPos, Quaternion.identity);
            }
            else
            {
                Debug.Log("�����ͺ��̽��� �������� ����");
            }
        }
    }

    public Inventory playerInventory;
    public Inventory playerQuickSlot;
    public void LootItemToInventory(int id, int amount)
    {
        if(id == 0)
        {
            Debug.Log("�׷� �������� ����.");
            return;
        }
        else
        {
            playerInventory.AddItem(id, amount);
        }
    }

    #region ������ ����
    [SerializeField]
    private GameObject TooltipUI;
    [SerializeField]
    private TextMeshProUGUI TooltipItemName;
    [SerializeField]
    private TextMeshProUGUI TooltipItemDescription;
    [SerializeField]
    private TextMeshProUGUI TooltipItemAmount;
    private bool tooltipTimerCondition;
    public void ShowTooltip(ItemSlot item, Vector3 cursorPos)
    {

        RectTransform tooltipRect = TooltipUI.GetComponent<RectTransform>();
        float pivotX;
        float pivotY;
        float ScreenX = Screen.width;
        float ScreenY = Screen.height;
        //�����ʿ� �ִٸ� �������� ���� â ����
        pivotX = cursorPos.x > (ScreenX / 2f) ? 1f : 0f;
        //���� �ִٸ� �Ʒ������� ���� â ����
        pivotY = cursorPos.y > (ScreenY * (2f / 3f)) ? 1f : 0f;

        tooltipRect.pivot = new Vector2(pivotX, pivotY);
        TooltipUI.transform.position = cursorPos;
        TooltipUI.SetActive(true);
        TooltipItemName.text = item.itemName;
        TooltipItemDescription.text = item.itemDescription;
        TooltipItemAmount.text = $"���� : {item.itemAmount}";
    }
    
    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }
    #endregion

    //Aĭ �������� Bĭ �����۰� ��ġ ��ü
    public void SwapItemSlot(ItemSlot slotA, ItemSlot slotB)
    {
        //�����Կ����� canGrab �׽�Ʈ�� ��.
        int tempID = slotA.itemID;
        int tempAmount = slotA.itemAmount;
        slotA.SetItemSlot(slotB.itemID, slotB.itemAmount);
        slotB.SetItemSlot(tempID, tempAmount);
        SoundManager.Instance.PlayArrangeItem();
    }

    public GameObject FoodParentHand;
    public void UseFood(int itemID)
    {
        //���� ������� �������� ������ ���� -1
        Debug.Log("���� ����1");

    }
    public void UseFood(CreateItemSlot slot, int itemID)
    {
        Debug.Log("���� ����1");
        int slotIndex = playerQuickSlot.FindItem(itemID);
        if(slotIndex == -1)
        {
            Debug.Log("�����Կ� ������ ����");
            return;
        }
        playerQuickSlot.InventorySlots[slotIndex].EatFood();
        
    }
    GameObject FoodOnHandObject;
    public void SetFoodOnHand(int itemID)
    {
        DeleteFoodOnHand();
        if(itemID == 0)
        {
            return;
        }

        //quickslot�� isSelected�� false��
        string itemPrefabFile = $"{itemPrefabPath}{itemID.ToString()}";
        GameObject itemPrefab = Resources.Load(itemPrefabFile) as GameObject;
        if (itemPrefab != null)
        {
            FoodOnHandObject = Instantiate(itemPrefab, FoodParentHand.transform.position, Quaternion.identity);
            Collider collider = FoodOnHandObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            Rigidbody rigidbody = FoodOnHandObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true; // Ȥ�� rigidbody.enabled = false;
            }
            FoodOnHandObject.transform.parent = FoodParentHand.transform;
        }
    }

    public void DeleteFoodOnHand()
    {
        if(FoodOnHandObject != null)
        {
            Destroy(FoodOnHandObject.gameObject);
        }
        FoodOnHandObject = null;
    }

}
