using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum eItemType
{
    None,
    Food,
    Knife,
    Axe,
    Pickaxe,
    Torch
}
public class QuickSlot : ItemSlot
{
    public bool canGrab;
    private bool isSelected = false;
    private Vector3 SelectScale = Vector3.one * 1.1f;
    [SerializeField]
    private int healingAmount;
    [SerializeField]
    private eItemType itemType = eItemType.None;

    //
    public PlayerCotroller playerController;

    private void Start()
    {
        //음식이라면
        if (itemType == eItemType.Food)
        {
            healingAmount = (int)itemDataAll[eItemKeyColumns.HealingAmount.ToString()];
        }
        GameObject goPlayer = GameObject.Find("Player");
        if (goPlayer != null) 
        {
            playerController = goPlayer.GetComponent<PlayerCotroller>();
        }
    }
    private void Update()
    {
        if (isSelected)
        {
            gameObject.transform.localScale = SelectScale;
        }
        else 
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }

    public override void SetItemSlot(int getItemID, int amount)
    {
        base.SetItemSlot(getItemID, amount);
        //DB에서 아이템 타입 읽어오기
        string typeOnDB = itemDataAll[eItemKeyColumns.Type.ToString()] as string;
        itemType = (eItemType)Enum.Parse(typeof(eItemType), typeOnDB);

        object objGrab = itemDataAll[eItemKeyColumns.CanGrab.ToString()];
        canGrab = Convert.ToBoolean(objGrab);
        if(!canGrab) 
        {
            Debug.Log("퀵슬롯에 넣을 수 없는 아이템");
            if(itemAmount > 0)
            {
                Debug.Log($"양 = {itemAmount}");
                for (int i = 0; i < itemAmount; i++)
                {
                    ItemManager.Instance.DropItemToField(getItemID);
                }
            }
            InitializeSlot();
            return;
        }
    }
    public void SetSelect()
    {
        isSelected = true;
    }
    public void SetNonSelect()
    {
        isSelected = false;
    }
    public void UseQuickSlot()
    {
        //선택 슬롯 아니면
        if (!isSelected)
        {
            return;
        }
        //빈 슬롯 선택시 맨손
        if(itemID == 0)
        {
            ItemManager.Instance.UsingItemIcon.gameObject.SetActive(false);
            return;
        }
        SoundManager.Instance.PlayEuipItem();
        //선택된 슬롯일 경우 중앙 슬롯 아이콘을 위 아이콘으로 대체.
        ItemManager.Instance.UsingItemIcon.gameObject.SetActive(true);
        ItemManager.Instance.UsingItemIcon.sprite = itemIcon.sprite;


        //손에 드는 코드
        switch (itemType)
        {
            case eItemType.Food:
                ItemManager.Instance.SetFoodOnHand(itemID);
                playerController.EquipFood(itemID);
                Debug.Log("손에 음식 듬");
                break;
            case eItemType.Knife:
                playerController.SetHasKnife(true);
                playerController.EquipKnife();
                Debug.Log("손에 칼 듬");
                break;
            case eItemType.Axe:
                playerController.SetHasAxe(true);
                playerController.EquipAxe();
                Debug.Log("손에 도끼 듬");
                break;
            case eItemType.Pickaxe:
                playerController.SetHasPickaxe(true);
                playerController.EquipPickaxe();
                Debug.Log("손에 곡괭이 듬");
                break;
            case eItemType.Torch:
                playerController.SetHasTorch(true);
                playerController.EquipTorch();
                Debug.Log("손에 횃불 듬");
                break;
            default:
                Debug.Log("이상한 것. 수정 필요");
                break;
        }
    }
    public override void EatFood()
    {
        //이 슬롯에 있는 음식 아이템 사용
    }

    //오버라이드로 안쓰기
    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

    }
}
