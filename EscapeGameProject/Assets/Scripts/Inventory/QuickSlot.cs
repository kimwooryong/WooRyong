using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : ItemSlot
{
    public bool canGrab;

    public override void SetItemSlot(int getItemID, int amount)
    {
        base.SetItemSlot(getItemID, amount);
        
        object objGrab = itemDataAll[eItemKeyColumns.CanGrab.ToString()];
        canGrab = Convert.ToBoolean(objGrab);
        if(!canGrab) 
        {
            Debug.Log("�����Կ� ���� �� ���� ������");
            if(itemAmount > 0)
            {
                Debug.Log($"�� = {itemAmount}");
                for (int i = 0; i < itemAmount; i++)
                {
                    ItemManager.Instance.DropItemToField(getItemID);
                }
            }
            InitializeSlot();
            return;
        }
    }
}
