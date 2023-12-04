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
        itemName = "�� ĭ";
        itemDescription = "�� ĭ";
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
            Debug.Log("ID �� 0 �� ĭ�� �Ⱥ��̵���");
            SetColorEmpty();
            return;
        }
        itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        Debug.Log($"�����۵����� = {itemDataAll}");
        if (itemDataAll == null)
        {
            Debug.Log("�ش� ������ ����. -> ���� �ʿ�");
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

    #region ��ӹ��� ���� �Լ��� ����ȭ
    //������ �̺�Ʈ ���� �ʱ�
    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //}
    //public override void OnPointerExit(PointerEventData eventData)
    //{ 
    //}
    public override void OnPointerClick(PointerEventData eventData)
    {
    }
    //�巡�� ���� �ʱ�
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
