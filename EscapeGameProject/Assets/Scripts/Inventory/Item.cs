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
    //Cook
    [HideInInspector]
    public bool canCook;
    private Collider itemCollider;
    private float cookingGaugeNow;
    [SerializeField]
    private float cookingGaugeMax;

    private void Start()
    {
        var itemDataAll = ItemManager.Instance.ReadItemData(itemID);
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        object objCount = itemDataAll[eItemKeyColumns.CanCook.ToString()];
        canCook = Convert.ToBoolean(objCount);
        itemCollider = gameObject.GetComponent<Collider>();
        cookingGaugeNow = 0;
    }
    private void Update()
    {
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
    private void OnTriggerStay(Collider other)
    {
        if (canCook)
        {
            Cook cookComponent = other.GetComponent<Cook>();
            if (cookComponent != null)
            {
                cookingGaugeNow += Time.deltaTime;
                if(cookingGaugeNow >= cookingGaugeMax)
                {
                    ItemManager.Instance.DropItemToField(itemID + 1, gameObject.transform.position);
                    Destroy(gameObject);
                }
            }

        }
    }
}
