using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
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
            Debug.Log("이미지 못찾음!");
        }
        TextMeshProUGUI TextSlot = GetComponentInChildren<TextMeshProUGUI>();
        if(TextSlot != null)
        {
            itemAmountText = TextSlot;
        }
        else
        {
            Debug.Log("tmp 못찾음!");
        }
    }
    public void SetItemSlot(int getItemID, int amount)
    {
        itemID = getItemID;
        itemName = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Name).ToString();
        itemDescription = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Description).ToString();
        itemIcon.sprite = ItemManager.Instance.LoadItemIcon(itemID);
        itemAmountText.text = amount.ToString();
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
    private void SetColorBlack()
    {
        itemIcon.color = Color.black;
    }
    private void SetColorWhite()
    {
        itemIcon.color = Color.white;
    }

    //마우스 동작
}
