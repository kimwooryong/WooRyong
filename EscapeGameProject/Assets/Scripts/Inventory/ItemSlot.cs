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
        Debug.Log("초기화");
        itemID = 0;
        itemName = "빈 칸";
        itemDescription = "빈 칸";
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
            Debug.Log("ID 가 0 인 칸 초기화");
            InitializeSlot();
            return;
        }

        itemDataAll = ItemManager.Instance.ReadItemData(getItemID);
        if( itemDataAll == null )
        {
            Debug.Log("읽어 올 데이터가 없습니다.");
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
        //-1로 아이템 버리기.
        if(itemAmount <= 0)
        {
            Debug.Log("버리는 양이 더 많아");
            SetItemSlot(0, 0);
        }
        itemAmountText.text = itemAmount.ToString();
    }
    public void TestCount()
    {
        if (itemAmount == 0)
        {
            Debug.Log("아이템 없음");
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
    //아이템 버리기
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
        //클릭 간격 0.2초 이하면 무시
        if (itemID != 0)
        {
            // 마우스 우클릭, 버리기
            Debug.Log("마우스 우클릭!");
            PlusItemAmount(-1);
            ItemManager.Instance.DropItemToField(itemID);
            yield return new WaitForSeconds(0.2f);
            isRightClickProcessing = false;
        }
        else
        {
            Debug.Log("빈 슬롯은 버릴 수 없어");
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
        //위치 초기화
        itemIcon.gameObject.transform.position = startPosition;
        //다른 슬롯에 놓여진다면
        GameObject endObject = eventData.pointerCurrentRaycast.gameObject;
        if(endObject != null)
        {
            Debug.Log(endObject);
            ItemSlot endDragSlot = endObject.GetComponent<ItemSlot>();
            if(endDragSlot != null)
            {
                Debug.Log("아이템 스왑");
                ItemManager.Instance.SwapItemSlot(this, endDragSlot);
            }
            else
            {
                Debug.Log("슬롯이 아님");
            }
        }
        else
        {
            Debug.Log("Raycast에 맞지않음.");
        }

    }

}
