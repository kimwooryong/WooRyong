using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : ItemSlot
{
    public bool canGrab;
    public bool isSelected = false;
    private Vector3 SelectScale = Vector3.one * 1.1f;
    private void Update()
    {
        if (isSelected)
        {
            gameObject.transform.localScale = SelectScale;
        }
        else 
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }

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
    public void SetSelect()
    {
        isSelected = true;
    }
    public void SetNonSelect()
    {
        isSelected = false;
    }
    public void EquipItem()
    {
        Debug.Log("������ ���");
    }
}
