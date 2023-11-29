using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Item : MonoBehaviour
{
    //ItemID
    [SerializeField]
    private int itemID;
    [SerializeField]
    private string itemName;
    private int itemAmount = 1;

    private void Start()
    {
        Debug.Log("아이템 설정");
        var itemDataAll = ItemManager.Instance.ReadItemData(itemID);
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
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
}
