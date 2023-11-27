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
    public void AddItem(int itemID)
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
                InventorySlots[emptyIndex].SetItemSlot(itemID);
            }
        }
        //�ִٸ�
        else
        {
            //�ְ�, ��ĥ �� �ִٸ�
            

            //������, ��ĥ �� ���ٸ� ��ĭ Ž��.

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
            //������ ID�� ������, �ε��� ��ȯ
            if (InventorySlots[i].itemID == itemID)
            {
                return i;
            }
        }
        return -1;
    }
}
