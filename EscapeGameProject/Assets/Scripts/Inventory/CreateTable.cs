using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTable : MonoBehaviour
{
    [SerializeField]
    private int targetItemID;
    [SerializeField]
    private int targetItemAmount;
    [SerializeField]
    private Button btnCreate;

    [Serializable]
    public class materialItem
    {
        public CreateItemSlot slot;
        public int materialItemID;
        public int materialItemAmount;
    }

    public CreateItemSlot targetItem;
    [SerializeField]
    public materialItem[] materialItems;

    private void Awake()
    {
        InitializeTable();
        
    }
    private void InitializeTable()
    {
        btnCreate.onClick.AddListener(CreateItemToInventory);
        ItemManager.Instance.OpenInventory += TestCreateCondition;
        Debug.Log(ItemManager.Instance.OpenInventory);

    }
    private void Start()
    {
        SetSlots();
    }

    public void SetSlots()
    {
        targetItem.SetItemSlot(targetItemID, targetItemAmount);
        for(int i = 0; i < materialItems.Length; i++)
        {
            materialItems[i].slot.SetItemSlot
                (materialItems[i].materialItemID, materialItems[i].materialItemAmount);
        }
    }
    public void TestCreateCondition()
    { 
        bool createCondition = true;
        for(int i = 0; i < materialItems.Length; i++)
        {
            if(materialItems[i].materialItemID == 0)
            {
                break;
            }
            //맞는 재료가 있는지 확인
            int itemIndex = ItemManager.Instance.playerInventory.FindItem(materialItems[i].materialItemID);
            int itemAmount = ItemManager.Instance.playerInventory.FindItemAmountWithIndex(itemIndex);
            if(itemAmount < materialItems[i].materialItemAmount)
            {
                createCondition = false;
                break;
            }
        }
        if (createCondition == true)
        {
            btnCreate.gameObject.SetActive(true);
        }
        else
        {
            
            btnCreate.gameObject.SetActive(false);
        }
    }
    public void CreateItemToInventory()
    {
        //아이템 재료가 다 있으면 활성화되는 버튼
        for (int i = 0; i < materialItems.Length; i++)
        {
            if (materialItems[i].materialItemID == 0)
            {
                break;
            }
            //맞는 재료가 있는지 확인
            int itemIndex = ItemManager.Instance.playerInventory.FindItem(materialItems[i].materialItemID);
            int itemAmount = ItemManager.Instance.playerInventory.FindItemAmountWithIndex(itemIndex);
            ItemManager.Instance.playerInventory.RemoveItem(itemIndex, materialItems[i].materialItemAmount);
        }
        ItemManager.Instance.OpenInventory?.Invoke();
        ItemManager.Instance.LootItemToInventory(targetItemID, targetItemAmount);
        Debug.Log("아이템 생성");
    }
}
