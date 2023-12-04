using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateItemSlot : ItemSlot
{
    public void Awake()
    {
        Debug.Log($"제작창 불러오기. {itemID}, {itemAmount}");
        SetItemSlot(itemID, itemAmount);
    }
    private void Start()
    {

    }
    public override void SetItemSlot(int getItemID, int amount)
    {
        if (getItemID == 0 || amount == 0)
        {
            Debug.Log("ID 가 0 인 칸은 안보이도록");
            SetColorEmpty();
            return;
        }
        itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        Debug.Log($"아이템데이터 = {itemDataAll}");
        if (itemDataAll == null)
        {
            Debug.Log("해당 아이템 없음. -> 수정 필요");
            return;
        }
        itemID = getItemID;
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        itemDescription = itemDataAll[eItemKeyColumns.Description.ToString()] as string;

        object objCount = itemDataAll[eItemKeyColumns.CanCount.ToString()];
        canCount = Convert.ToBoolean(objCount);

        if (itemIcon != null)
        {
            itemIcon.sprite = ItemManager.Instance.LoadItemIcon(itemID);
        }
        itemAmount = amount;
        itemAmountText.text = itemAmount.ToString();
        if (canCount)
        {
            itemAmountText.gameObject.SetActive(true);
        }
        else
        {
            itemAmountText.gameObject.SetActive(false);
        }
        SetColorWhite();
    }

    #region 상속받지 않을 함수들 깡통화
    //포인터 이벤트 받지 않기
    public override void OnPointerEnter(PointerEventData eventData)
    {
    }
    public override void OnPointerExit(PointerEventData eventData)
    { 
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
    }
    //드래그 받지 않기
    public override void OnBeginDrag(PointerEventData eventData)
    {
    }
    public override void OnDrag(PointerEventData eventData)
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
    }
    #endregion
}
