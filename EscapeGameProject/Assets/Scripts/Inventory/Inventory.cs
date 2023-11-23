using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> InventorySlots;
    public event Action OnItemChanged;

    [SerializeField]
    private int inventorySize;

    

    private void InitializeInventory()
    {
        InventorySlots = new List<ItemSlot>();
    }

    //특정 ID 아이템 획득
    private void GetItem(int itemID)
    {
        //이미 있는 아이템일 때.
        if (FindItem(itemID))
        {

        }
        //없는 아이템일 때.
        else
        {

        }
    }
    private void RemoveItem(int index, int amount)
    {

    }
    //A칸 슬롯과 B칸 슬롯 변경.
    private void SwapItemSlot(int indexA, int indexB)
    {

    }
    private bool FindItem(int itemID)
    {
        foreach (ItemSlot slot in InventorySlots)
        {
            //빈 칸 발견시
            if (slot.itemID == 0)
            {
                Debug.Log("아이템 슬롯 끝.");
                return false;
            }
            else
            {
                //item이 이미 있을 때
                if(slot.itemID == itemID)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
