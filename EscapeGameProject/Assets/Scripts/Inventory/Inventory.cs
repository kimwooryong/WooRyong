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

    //Ư�� ID ������ ȹ��
    private void GetItem(int itemID)
    {
        //�̹� �ִ� �������� ��.
        if (FindItem(itemID))
        {

        }
        //���� �������� ��.
        else
        {

        }
    }
    private void RemoveItem(int index, int amount)
    {

    }
    //Aĭ ���԰� Bĭ ���� ����.
    private void SwapItemSlot(int indexA, int indexB)
    {

    }
    private bool FindItem(int itemID)
    {
        foreach (ItemSlot slot in InventorySlots)
        {
            //�� ĭ �߽߰�
            if (slot.itemID == 0)
            {
                Debug.Log("������ ���� ��.");
                return false;
            }
            else
            {
                //item�� �̹� ���� ��
                if(slot.itemID == itemID)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
