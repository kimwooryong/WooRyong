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
        //�����̶��
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
        //DB���� ������ Ÿ�� �о����
        string typeOnDB = itemDataAll[eItemKeyColumns.Type.ToString()] as string;
        itemType = (eItemType)Enum.Parse(typeof(eItemType), typeOnDB);

        object objGrab = itemDataAll[eItemKeyColumns.CanGrab.ToString()];
        canGrab = Convert.ToBoolean(objGrab);
        if(!canGrab) 
        {
            Debug.Log("�����Կ� ���� �� ���� ������");
            if(itemAmount > 0)
            {
                Debug.Log($"�� = {itemAmount}");
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
        //���� ���� �ƴϸ�
        if (!isSelected)
        {
            return;
        }
        //�� ���� ���ý� �Ǽ�
        if(itemID == 0)
        {
            ItemManager.Instance.UsingItemIcon.gameObject.SetActive(false);
            return;
        }
        SoundManager.Instance.PlayEuipItem();
        //���õ� ������ ��� �߾� ���� �������� �� ���������� ��ü.
        ItemManager.Instance.UsingItemIcon.gameObject.SetActive(true);
        ItemManager.Instance.UsingItemIcon.sprite = itemIcon.sprite;


        //�տ� ��� �ڵ�
        switch (itemType)
        {
            case eItemType.Food:
                ItemManager.Instance.SetFoodOnHand(itemID);
                playerController.EquipFood(itemID);
                Debug.Log("�տ� ���� ��");
                break;
            case eItemType.Knife:
                playerController.SetHasKnife(true);
                playerController.EquipKnife();
                Debug.Log("�տ� Į ��");
                break;
            case eItemType.Axe:
                playerController.SetHasAxe(true);
                playerController.EquipAxe();
                Debug.Log("�տ� ���� ��");
                break;
            case eItemType.Pickaxe:
                playerController.SetHasPickaxe(true);
                playerController.EquipPickaxe();
                Debug.Log("�տ� ��� ��");
                break;
            case eItemType.Torch:
                playerController.SetHasTorch(true);
                playerController.EquipTorch();
                Debug.Log("�տ� ȶ�� ��");
                break;
            default:
                Debug.Log("�̻��� ��. ���� �ʿ�");
                break;
        }
    }
    public override void EatFood()
    {
        //�� ���Կ� �ִ� ���� ������ ���
    }

    //�������̵�� �Ⱦ���
    public override void OnPointerEnter(PointerEventData eventData)
    {

    }
    public override void OnPointerExit(PointerEventData eventData)
    {

    }
}
