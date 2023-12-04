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
        Debug.Log($"����â �ҷ�����. {itemID}, {itemAmount}");
        SetItemSlot(itemID, itemAmount);
    }
    private void Start()
    {

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

    #region ��ӹ��� ���� �Լ��� ����ȭ
    //������ �̺�Ʈ ���� �ʱ�
    public override void OnPointerEnter(PointerEventData eventData)
    {
    }
    public override void OnPointerExit(PointerEventData eventData)
    { 
    }
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
