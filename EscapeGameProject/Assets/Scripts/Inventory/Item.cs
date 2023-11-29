using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //ItemID
    [SerializeField]
    private int itemID;
    private string itemDescription;

    private void OnEnable()
    {
        itemDescription = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Description) as string;
    }
    public int GetItemID()
    {
        return itemID;
    }
    public string GetItemDescription()
    {
        return itemDescription;
    }
}
