using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary> 상속자들!
/// 1. IBeginDragHandler
///     ㄴ드래그가 시작될 때 호출됩니다.
/// 2. IDragHandler
///     ㄴ 드래그 중에 호출됩니다.
/// 3. IEndDragHandler
///     ㄴ드래그가 끝났을 때 호출됩니다.
/// 4. IPointerEnterHandler
///     ㄴ마우스 포인터가 UI 요소 위로 들어올 때 호출됩니다.
/// 5. IPointerExitHandler
///     ㄴ마우스 포인터가 UI 요소에서 나갈 때 호출됩니다.
/// </summary>

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler/*, IPointerDownHandler, IPointerUpHandler*/
{
    /// <summary>
    /// Inventory에서 마우스 포인터를 이용한 기능
    /// 기능
    /// 1. 아이템 스왑
    /// 2. 아이템 정보 툴
    /// </summary>


    //public void InitializeItem()
    //{

    //}

    //public void RefreshCount()
    //{

    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Bedgin Drap");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End Drap");
    }



    //public void OnPointerDown(PointerEventData eventData)
    //{
        
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //}


    //public void OnPointerExit(PointerEventData eventData)
    //{

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{

    //}
}

