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

    public void SetItemSlot(int getItemID)
    {
        //가지고 있는게 또 들어온다면
        if(itemID == getItemID)
        {
            //합쳐질 수 있는지 검사


        }
        //아니라면

        itemID = getItemID;
        itemName = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Name).ToString();
        itemDescription = ItemManager.Instance.ReadItemData(itemID, eItemKeyColumns.Description).ToString();
        //스프라이트 경로 받아서 설정. 이미지 읽어오기.
        //itemIcon =
        }
}
