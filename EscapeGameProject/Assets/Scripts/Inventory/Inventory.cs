using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        InitializeInventory();
    }
    private void InitializeInventory()
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
    public void AddItem(int itemID, int amount)
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
    private void RemoveItem(int index, int amount)
    {

    }
    //Aĭ ������ Bĭ ���Կ� ���
    private void SwapItemSlot(ItemSlot slotA, ItemSlot slotB)
    {
        ItemSlot tempSlot = slotA;
        slotA = slotB;
        slotB = tempSlot;
    }
    //ID�� �˻��ؼ� ������ ��ġ(index) ��ȯ, 0���� �˻��ϸ� ��ĭã��. -1 ��ȯ�� ������ ����.
    private int FindItem(int itemID)
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


}
