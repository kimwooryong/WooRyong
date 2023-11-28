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
    }
    #endregion
    private List<Dictionary<string, object>> data;
    private string itemPrefabPath = "ItemPrefabs/ItemPrefab";
    private string itemIconPath = "ItemIcons/ItemIcon";


    //Ű �Է�
    private bool isActiveInventory;
    private bool isActiveQuickSlot;
    //���, ����, �߾�, ������
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
        data = CSVReader.Read("ItemDataBase");
    }

    //Ư���� �޾ƿ���
    public object ReadItemData(int itemID, eItemKeyColumns dataBaseKey)
    {
        return data[itemID][dataBaseKey.ToString()];
    }
    //�� ��ü �޾ƿ���
    public Dictionary<string, object> ReadItemData(int itemID)
    {
        return data[itemID];
    }

    //�׽�Ʈ��
    private int count = 0;
    public void Update()
    {
        //�׽�Ʈ�� �����
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            Debug.Log(("ID : " + data[count]["ID"] + "\t" +
           "Name : " + data[count]["Name"] + "\t" +
           "Description : " + data[count][eItemKeyColumns.Description.ToString()]));
            if (count > 6) count = 0;
        }

        //�׽�Ʈ�� ������ ���
        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItemToField(int.Parse(idText.text), transform.position);
        }
        //�׽�Ʈ�� ������ ȹ��
        if (Input.GetKeyDown(KeyCode.L))
        {
            LootItemToInventory(int.Parse(idText.text), 1);
        }
        //�׽�Ʈ�� �κ��丮 �¿���
        if(Input.GetKeyDown(KeyCode.I))
        {
            isActiveInventory = !isActiveInventory;
            Debug.Log(isActiveInventory);
            inventoryCanvas.gameObject.SetActive(isActiveInventory);
        }
        //�׽�Ʈ�� ������ �¿���
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
            Debug.Log("�װ� ���Դϴ�.");
            return null;
        }
        else
        {
            string itemIconFile = $"{itemIconPath}{id.ToString()}";
            Sprite icon = Resources.Load<Sprite>(itemIconFile);
            return icon;
        }
    }

    public TMP_InputField idText;
    //ID �޾ƿͼ� �ʵ� ���� prefab �����
    public void DropItemToField(int id, Vector3 createPos)
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
                Instantiate(itemPrefab, createPos, Quaternion.identity);
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

    //����������
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
        //�����ʿ� �ִٸ� �������� �ǹ�����
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
        TooltipItemAmount.text = $"���� : {item.itemAmount}";
    }
    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }
}
