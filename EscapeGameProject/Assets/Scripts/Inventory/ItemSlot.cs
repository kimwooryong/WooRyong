using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;


public class ItemSlot : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public int itemAmount;
    public SpriteRenderer itemIcon;
    public bool canCount;

    public void SetItemSlot(int getItemID)
    {
        //������ �ִ°� �� ���´ٸ�
        if(itemID == getItemID)
        {
            //������ �� �ִ��� �˻�
            //if(ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.CanCount) == true)
            //{

            //}

        }
        //�ƴ϶��

        itemID = getItemID;
        itemName = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Name).ToString();
        itemDescription = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Description).ToString();
        //��������Ʈ ��� �޾Ƽ� ����. �̹��� �о����.
        //itemIcon =
    }
    public void PlusItemAmount(int quantity)
    {
        
    }
}
