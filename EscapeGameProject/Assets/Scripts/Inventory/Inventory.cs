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
    private void AddItem(int itemID)
    {
        //������ �˻�
        int index = FindItem(itemID);
        //���ٸ�
        if(index == -1)
        {

        }
        //�ִٸ�
        else
        {
            //�ְ�, ��ĥ �� �ִٸ�

            //������, ��ĥ �� ���ٸ�

        }
    }
    private void RemoveItem(int index, int amount)
    {

    }
    //Aĭ ���԰� Bĭ ���� ����.
    private void SwapItemSlot(int indexA, int indexB)
    {

    }
    //ID�� �˻��ؼ� ������ ��ġ(index) ��ȯ, 0���� �˻��ϸ� ��ĭã��. -1 ��ȯ�� ������ ����.
    private int FindItem(int itemID)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            //������ ID ������
            if (InventorySlots[i].itemID == itemID)
            {
                return i;
            }
        }
            return -1;
    }
}
