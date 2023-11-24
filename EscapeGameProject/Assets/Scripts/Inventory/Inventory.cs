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
    private void AddItem(int itemID)
    {
        //아이템 검색
        int index = FindItem(itemID);
        //없다면
        if(index == -1)
        {

        }
        //있다면
        else
        {
            //있고, 합칠 수 있다면

            //있지만, 합칠 수 없다면

        }
    }
    private void RemoveItem(int index, int amount)
    {

    }
    //A칸 슬롯과 B칸 슬롯 변경.
    private void SwapItemSlot(int indexA, int indexB)
    {

    }
    //ID로 검색해서 아이템 위치(index) 반환, 0으로 검색하면 빈칸찾기. -1 반환은 같은게 없다.
    private int FindItem(int itemID)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            //아이템 ID 같은거
            if (InventorySlots[i].itemID == itemID)
            {
                return i;
            }
        }
            return -1;
    }
}
