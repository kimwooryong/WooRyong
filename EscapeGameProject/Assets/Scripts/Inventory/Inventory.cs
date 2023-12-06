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

    //Ư�� ID ������ ȹ��
    public virtual void AddItem(int itemID, int amount)
    {
        //������ �˻�
        int index = FindItem(itemID);
        //���ٸ� �� ĭ ã�Ƽ� �߰�
        if(index == -1)
        {
            int emptyIndex = FindItem(0);
            if(emptyIndex == -1)
            {
                Debug.Log("�κ��丮�� �� ĭ�� ����.");
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
        //�ִٸ�
        else
        {
            //�ְ�, ��ĥ �� �ִٸ�
            if (InventorySlots[index].canCount == true)
            {
                InventorySlots[index].PlusItemAmount(amount);
            }
            //������, ��ĥ �� ���ٸ� ��ĭ Ž��.
            else
            {
                int emptyIndex = FindItem(0);
                if (emptyIndex == -1)
                {
                    Debug.Log("�κ��丮�� �� ĭ�� ����.");
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

    //ID�� �˻��ؼ� ������ ��ġ(index) ��ȯ, 0���� �˻��ϸ� ��ĭã��. -1 ��ȯ�� ������ ����.
    public virtual int FindItem(int itemID)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            //������ ID�� ������, �ε��� ��ȯ
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
            Debug.Log("�� ĭ���� ���� ����.");
            return -1;
        }
        return itemAmount;
    }
}
