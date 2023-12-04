using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateItemSlot : ItemSlot
{
    public override void Awake()
    {
        InitializeSlot();
    }
    private void Start()
    {
        SetItemSlot(itemID, itemAmount);
    }
    public override void InitializeSlot()
    {
        itemName = "빈 칸";
        itemDescription = "빈 칸";
        itemAmountText.gameObject.SetActive(false);
        canCount = false;

        Transform iconTransform = transform.Find("Icon");
        if (iconTransform != null)
        {
            itemIcon = iconTransform.GetComponent<Image>();
            itemIcon.sprite = null;
            SetColorEmpty();
        }
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
    public override void SetColorEmpty()
    {
        Color newColor = itemIcon.color;
        newColor.a = 0f;
        itemIcon.color = newColor;
        itemAmountText.gameObject.SetActive(false);
        Image boxImage = transform.gameObject.GetComponent<Image>();
        boxImage.color = newColor;
    }
    public override void SetColorWhite()
    {
        itemIcon.color = Color.white;
        Image boxImage = transform.gameObject.GetComponent<Image>();
        boxImage.color = Color.white;
    }

    #region 상속받지 않을 함수들 깡통화
    //포인터 이벤트 받지 않기
    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //}
    //public override void OnPointerExit(PointerEventData eventData)
    //{ 
    //}
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
