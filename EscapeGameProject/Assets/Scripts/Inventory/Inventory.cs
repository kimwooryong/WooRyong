using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public List<ItemSlot> InventorySlots;

    [SerializeField]
    private GameObject inventorySlotPrefab;
    [SerializeField]
    private int inventorySize;

    protected void Awake()
    {
        InitializeInventory();
    }
    protected void InitializeInventory()
    {
        InventorySlots = new List<ItemSlot>();
        for(int i = 0; i < inventorySize;  i++)
        {
            GameObject tempGo = Instantiate(inventorySlotPrefab, gameObject.transform);
            ItemSlot tempSlot = tempGo.GetComponent<ItemSlot>();
            InventorySlots.Add(tempSlot);
        }
    }

    //특정 ID 아이템 획득
    public virtual void AddItem(int itemID, int amount)
    {
        //아이템 검색
        int index = FindItem(itemID);
        //없다면 빈 칸 찾아서 추가
        if(index == -1)
        {
            int emptyIndex = FindItem(0);
            if(emptyIndex == -1)
            {
                Debug.Log("인벤토리에 빈 칸이 없음.");
                for(int i = 0; i < amount; i++)
                {
                    ItemManager.Instance.DropItemToField(itemID);
                }
            }
            else
            {
                InventorySlots[emptyIndex].SetItemSlot(itemID, amount);
            }
        }
        //있다면
        else
        {
            //있고, 합칠 수 있다면
            if (InventorySlots[index].canCount == true)
            {
                InventorySlots[index].PlusItemAmount(amount);
            }
            //있지만, 합칠 수 없다면 빈칸 탐색.
            else
            {
                int emptyIndex = FindItem(0);
                if (emptyIndex == -1)
                {
                    Debug.Log("인벤토리에 빈 칸이 없음.");
                }
                else
                {
                    InventorySlots[emptyIndex].SetItemSlot(itemID, amount);
                }
            }
        }
    }
    public virtual void RemoveItem(int index, int amount)
    {
        InventorySlots[index].PlusItemAmount(-amount);
    }

    //ID로 검색해서 아이템 위치(index) 반환, 0으로 검색하면 빈칸찾기. -1 반환은 같은게 없다.
    public virtual int FindItem(int itemID)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            //아이템 ID가 같으면, 인덱스 반환
            if (InventorySlots[i].itemID == itemID)
            {
                return i;
            }
        }
        return -1;
    }
    public int FindItemAmountWithIndex(int index)
    {
        int itemAmount = 0;
        if(0 <= index && index < InventorySlots.Count)
        {
            itemAmount = InventorySlots[index].itemAmount;
        }
        if(itemAmount <= 0)
        {
            Debug.Log("그 칸에는 뭐가 없다.");
            return -1;
        }
        return itemAmount;
    }
}
