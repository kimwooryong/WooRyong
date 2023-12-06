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
        btnCreate.onClick.AddListener(CreateItemToInventory);
    }
    private void Start()
    {
        SetSlots();
    }

    private void OnEnable()
    {
        TestCreateCondition();
    }
    public void SetSlots()
    {
        targetItem.SetItemSlot(targetItemID, targetItemAmount);
        for(int i = 0; i < materialItems.Length; i++)
        {
            Debug.Log($"{i}번째 재료 슬롯 세팅. {materialItems[i].slot}, {materialItems[i].materialItemID}, {materialItems[i].materialItemAmount}");
            materialItems[i].slot.SetItemSlot
                (materialItems[i].materialItemID, materialItems[i].materialItemAmount);
        }
    }
    public void TestCreateCondition()
    {
        bool createCondition = true;
        for(int i = 0; i < materialItems.Length; i++)
        {
            //맞는 재료가 있는지 확인
            int itemIndex = ItemManager.Instance.playerInventory.FindItem(materialItems[i].materialItemID);
            int itemAmount = ItemManager.Instance.playerInventory.FindItemAmountWithIndex(itemIndex);
            if(itemAmount < materialItems[i].materialItemAmount)
            {
                createCondition = false;
                return;
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
        //아이템 재료가 다 있으면


        Debug.Log("아이템 생성");
    }
}
