using System;
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
    private bool canCook;

    private void Start()
    {
        var itemDataAll = ItemManager.Instance.ReadItemData(itemID);
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        object objCount = itemDataAll[eItemKeyColumns.CanCount.ToString()];
        canCook = Convert.ToBoolean(objCount);

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
