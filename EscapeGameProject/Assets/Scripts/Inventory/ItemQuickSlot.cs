using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlot : ItemSlot
{
    public bool canGrab;
    private bool isSelected = false;
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
    public void UseQuickSlot()
    {
        //���� ���� �ƴϸ�
        if (!isSelected)
        {
            return;
        }
        //�� ���� ���ý� �Ǽ�
        if(itemID == 0)
        {
            ItemManager.Instance.UsingItemIcon.gameObject.SetActive(false);
            return;
        }
        //���õ� ������ ��� �߾� ���� �������� �� ���������� ��ü.
        ItemManager.Instance.UsingItemIcon.gameObject.SetActive(true);
        ItemManager.Instance.UsingItemIcon.sprite = itemIcon.sprite;
    }

    //�������̵�� �Ⱦ���
    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }
}
