using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTable : MonoBehaviour
{
    public int targetItemID;
    public int targetItemAmount;

    public struct materialItem
    {
        public int materialItemID;
        public int materialItemAmount;
    }

    public CreateItemSlot targetItem;
    [SerializeField]
    public materialItem[] materialItems;

    private void Awake()
    {
        SetSlots();
    }
    public void SetSlots()
    {
        targetItem.SetItemSlot(targetItemID, targetItemAmount);
    }
}
