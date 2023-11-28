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
    CanCount
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

    private void Start()
    {
        data = CSVReader.Read("ItemDataBase");
    }

    //특정값 받아오기
    public object ReadItemData(int itemID, eItemKeyColumns dataBaseKey)
    {
        return data[itemID][dataBaseKey.ToString()];
    }
    //값 전체 받아오기
    public Dictionary<string, object> ReadItemData(int itemID)
    {
        return data[itemID];
    }

    //테스트용
    [SerializeField]
    public TextMeshProUGUI text;
    private int count = 0;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            Debug.Log(("ID : " + data[count]["ID"] + "\t" +
           "Name : " + data[count]["Name"] + "\t" +
           "Description : " + data[count][eItemKeyColumns.Description.ToString()]));

            text.text = ("ID : " + data[count]["ID"] + "\t" +
           "Name : " + data[count]["Name"] + "\t" +
           "Description : " + data[count]["Description"]);
            count++;
            if (count > 6) count = 0;
        }

        //테스트용 아이템 드랍
        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItemToField(int.Parse(idText.text), transform.position);
        }
        //테스트용 아이템 획득
        if (Input.GetKeyDown(KeyCode.L))
        {
            LootItemToInventory(int.Parse(idText.text), 1);
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

    public TMP_InputField idText;
    //ID 받아와서 필드 내에 prefab 드롭함
    public void DropItemToField(int id, Vector3 createPos)
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
                Instantiate(itemPrefab, createPos, Quaternion.identity);
            }
            else
            {
                Debug.Log("데이터베이스에 아이템이 없다");
            }
        }
    }

    public Inventory playerInventory;
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

    public void ShowTooltip(ItemSlot item)
    {
        TooltipUI.SetActive(true);
        TooltipItemName.text = item.itemName;
        TooltipItemDescription.text = item.itemDescription;
        TooltipItemAmount.text = item.itemAmount.ToString();
    }
    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }
}
