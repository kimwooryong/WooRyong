using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public enum eItemType
{
    None,
    Food,
    Knife,
    Axe,
    Pickaxe,
    Touch
}
public class Item : MonoBehaviour
{
    //ItemID
    [SerializeField]
    private int itemID;
    [SerializeField]
    private string itemName;
    private int itemAmount = 1;
    [SerializeField]
    private int healingAmount;
    [SerializeField]
    private eItemType itemType = eItemType.None;

    private void Start()
    {
        Debug.Log("아이템 설정");
        var itemDataAll = ItemManager.Instance.ReadItemData(itemID);
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        if(itemType == eItemType.Food)
        {
            healingAmount = (int)itemDataAll[eItemKeyColumns.HealingAmount.ToString()];
        }
    }
    public int GetItemID()
    {
        return itemID;
    }
    public string GetItemDescription()
    {
        return itemName;
    }
    public int GetItemAmount()
    {
        return itemAmount;
    }

    public void EquipItem()
    {

        //장비 장착.
    }
    public void EatFood()
    {

        //장비 사용-음식류만
    }
}
