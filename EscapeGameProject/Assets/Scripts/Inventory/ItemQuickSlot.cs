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
            Debug.Log("퀵슬롯에 넣을 수 없는 아이템");
            if(itemAmount > 0)
            {
                Debug.Log($"양 = {itemAmount}");
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
        //선택 슬롯 아니면
        if (!isSelected)
        {
            return;
        }
        //빈 슬롯 선택시 맨손
        if(itemID == 0)
        {
            ItemManager.Instance.UsingItemIcon.gameObject.SetActive(false);
            return;
        }
        //선택된 슬롯일 경우 중앙 슬롯 아이콘을 위 아이콘으로 대체.
        ItemManager.Instance.UsingItemIcon.gameObject.SetActive(true);
        ItemManager.Instance.UsingItemIcon.sprite = itemIcon.sprite;
    }

    //오버라이드로 안쓰기
    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }
}
