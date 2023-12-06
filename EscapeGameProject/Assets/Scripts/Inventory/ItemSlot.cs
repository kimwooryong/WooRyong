using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, 
    IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public int itemAmount;
    public Image itemIcon;
    public bool canCount;

    public TextMeshProUGUI itemAmountText;
    public virtual void Awake()
    {
        InitializeSlot();
    }

    public virtual void InitializeSlot()
    {
        Debug.Log("�ʱ�ȭ");
        itemID = 0;
        itemName = "�� ĭ";
        itemDescription = "�� ĭ";
        itemAmount = 0;
        itemAmountText.gameObject.SetActive(false);
        canCount = false;

        Transform iconTransform = transform.Find("Icon");
        if (iconTransform != null)
        {
            itemIcon = iconTransform.GetComponent<Image>();
            itemIcon.sprite = null;
            SetColorEmpty();
        }
    }

    protected Dictionary<string, object> itemDataAll = new Dictionary<string, object>();
    public virtual void SetItemSlot(int getItemID, int amount)
    {
        if(getItemID == 0)
        {
            Debug.Log("ID �� 0 �� ĭ �ʱ�ȭ");
            InitializeSlot();
            return;
        }

        itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        if( itemDataAll == null )
        {
            Debug.Log("�о� �� �����Ͱ� �����ϴ�.");
            return;
        }
        itemID = getItemID;
        itemName = itemDataAll[eItemKeyColumns.Name.ToString()] as string;
        itemDescription = itemDataAll[eItemKeyColumns.Description.ToString()] as string;

        object objCount = itemDataAll[eItemKeyColumns.CanCount.ToString()];
        canCount = Convert.ToBoolean(objCount);

        if(itemIcon != null)
        {
            itemIcon.sprite = ItemManager.Instance.LoadItemIcon(itemID);
        }
        itemAmount = amount;
        itemAmountText.text = itemAmount.ToString();
        if (canCount)
        {
            itemAmountText.gameObject.SetActive(true);
        }
        else
        {
            itemAmountText.gameObject.SetActive(false);
        }
        if(amount == 0)
        {
            SetColorEmpty();
        }
        else
        {
            SetColorWhite();
        }
    }
    public void PlusItemAmount(int quantity)
    {
        itemAmount += quantity;
        //-1�� ������ ������.
        if(itemAmount <= 0)
        {
            Debug.Log("������ ���� �� ����");
            SetItemSlot(0, 0);
        }
        itemAmountText.text = itemAmount.ToString();
    }
    public void TestCount()
    {
        if (itemAmount == 0)
        {
            Debug.Log("������ ����");
            return;
        }
    }
    public virtual void SetColorEmpty()
    {
        Color newColor = itemIcon.color;
        newColor.a = 0f;
        itemIcon.color = newColor;
        itemAmountText.gameObject.SetActive(false);
    }
    public virtual void SetColorWhite()
    {
        itemIcon.color = Color.white;
    }
    //Tooltip
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(itemID == 0)
        {
            return;
        }
        ItemManager.Instance.ShowTooltip(this, eventData.position);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ItemManager.Instance.HideTooltip();
    }
    //������ ������
    private bool isRightClickProcessing = false;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (isRightClickProcessing)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            StartCoroutine(CoDropItem());
        }
    }
    IEnumerator CoDropItem()
    {
        isRightClickProcessing = true;
        //Ŭ�� ���� 0.2�� ���ϸ� ����
        if (itemID != 0)
        {
            // ���콺 ��Ŭ��, ������
            Debug.Log("���콺 ��Ŭ��!");
            PlusItemAmount(-1);
            ItemManager.Instance.DropItemToField(itemID);
            yield return new WaitForSeconds(0.2f);
            isRightClickProcessing = false;
        }
        else
        {
            Debug.Log("�� ������ ���� �� ����");
        }
    }

    [HideInInspector]
    public Vector2 startPosition = new Vector2();
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = gameObject.transform.position;
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        itemIcon.gameObject.transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //��ġ �ʱ�ȭ
        itemIcon.gameObject.transform.position = startPosition;
        //�ٸ� ���Կ� �������ٸ�
        GameObject endObject = eventData.pointerCurrentRaycast.gameObject;
        if(endObject != null)
        {
            Debug.Log(endObject);
            ItemSlot endDragSlot = endObject.GetComponent<ItemSlot>();
            if(endDragSlot != null)
            {
                Debug.Log("������ ����");
                ItemManager.Instance.SwapItemSlot(this, endDragSlot);
            }
            else
            {
                Debug.Log("������ �ƴ�");
            }
        }
        else
        {
            Debug.Log("Raycast�� ��������.");
        }

    }

}
