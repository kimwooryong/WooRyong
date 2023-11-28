using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : ItemSlot
{
    public bool canGrab;
    public override void SetItemSlot(int getItemID, int amount)
    {
        var itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        if (itemDataAll == null)
        {
            Debug.Log("�о� �� �����Ͱ� �����ϴ�.");
            return;
        }
        object objGrab = itemDataAll[eItemKeyColumns.CanGrab.ToString()];
        canGrab = Convert.ToBoolean(objGrab);
        if(!canGrab) 
        {
            Debug.Log("�����Կ� ���� �� ���� ������");
            return;
        }
        itemID = getItemID;
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        itemDescription = itemDataAll[eItemKeyColumns.Description.ToString()] as string;

        object objCount = itemDataAll[eItemKeyColumns.CanCount.ToString()];
        canCount = Convert.ToBoolean(objCount);

        itemIcon.sprite = ItemManager.Instance.LoadItemIcon(itemID);
        itemAmount = amount;
        itemAmountText.text = amount.ToString();
        if (canCount)
        {
            itemAmountText.gameObject.SetActive(true);
        }
        else
        {
            itemAmountText.gameObject.SetActive(false);
        }
        if (amount == 0)
        {
            SetColorBlack();
        }
        else
        {
            SetColorWhite();
        }
    }
}
