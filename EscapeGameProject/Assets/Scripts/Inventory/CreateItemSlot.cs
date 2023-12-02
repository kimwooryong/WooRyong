using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateItemSlot : ItemSlot
{


    #region 상속받지 않을 함수들 깡통화
    //포인터 이벤트 받지 않기
    public override void OnPointerEnter(PointerEventData eventData)
    {
    }
    public override void OnPointerExit(PointerEventData eventData)
    { 
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
    }
    //드래그 받지 않기
    public override void OnBeginDrag(PointerEventData eventData)
    {
    }
    public override void OnDrag(PointerEventData eventData)
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
    }
    #endregion
}
