using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public int itemAmount;
    public Image itemIcon;
    public bool canCount;

    public TextMeshProUGUI itemAmountText;


    private void Awake()
    {
        Image[] IconSlots = GetComponentsInChildren<Image>();
        if(IconSlots != null )
        {
            itemIcon = IconSlots[1];
        }
        else
        {
            Debug.Log("�̹��� ��ã��!");
        }
        TextMeshProUGUI TextSlot = GetComponentInChildren<TextMeshProUGUI>();
        if(TextSlot != null)
        {
            itemAmountText = TextSlot;
        }
        else
        {
            Debug.Log("tmp ��ã��!");
        }
    }
    public void SetItemSlot(int getItemID, int amount)
    {
        var itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        if( itemDataAll == null )
        {
            Debug.Log("�о� �� �����Ͱ� �����ϴ�.");
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
        if(amount == 0)
        {
            SetColorBlack();
        }
        else
        {
            SetColorWhite();
        }
    }
    public void PlusItemAmount(int quantity)
    {
        itemAmount += quantity;
        itemAmountText.text = itemAmount.ToString();
    }
    public void TestCount()
    {
        if (itemAmount == 0)
        {
            Debug.Log("������ ����");
            return;
        }
    }
    private void SetColorBlack()
    {
        itemIcon.color = Color.black;
    }
    private void SetColorWhite()
    {
        itemIcon.color = Color.white;
    }
    //Tooltip
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemAmount == 0)
        {
            Debug.Log("������ ����");
            return;
        }
        Debug.Log("���콺 ��");
        ItemManager.Instance.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("���콺 �ƿ�");
        ItemManager.Instance.HideTooltip();
    }
}
