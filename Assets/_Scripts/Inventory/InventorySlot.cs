using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

#region - 상속자들!
/// <summary> 
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
#endregion
public class InventorySlot : SlotBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool boneFireSlot = false;
    #region UI 기능
    protected override void Awake()
    {
        base.Awake();
        //ClearOutLine();
    }

    public override void SetSlot(SlotData _slotData)
    {
        base.SetSlot(_slotData);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
    }

    public override void isWeapon()
    {
        base.isWeapon();
    }

    public override void SetOutLine()
    {
        base.SetOutLine();
    }

    public override void ClearOutLine()
    {
        base.ClearOutLine();
    }

    #endregion


    #region Pointer
    // 마우스 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"OnPointerClick");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (curSlot.itemData != null && curSlot.itemData.item != null)
            {
                if (Main.Inventory.IsPotionItem(curSlot.itemData))
                {
                    Debug.Log($"{curSlot.itemData} 사용하기");
                    //curSlot.itemData.item.Use();
                }
                //Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");
            }
        }
    }

    // 마우스 포인터가 슬롯에 들어갈 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        //1. 툴팁 보이도록
        //2. 투팀 생성된 위치값 조정 :)
        if (curSlot.itemData != null)
        {
            //Debug.Log($"OnPointerEnter");
            Main.Inventory.inventoryUI.toolTipContainer.SetToolTip(curSlot, this.transform.position);
        }
    }

    // 마우스 포인터가 슬롯에서 빠져나갈 때
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"OnPointerEixt");
        Main.Inventory.inventoryUI.toolTipContainer.HideToolTip();
    }
   
    #endregion

    #region Drag & Drop

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (curSlot.itemData != null)
        {
            //Debug.Log("OnDeginDrag");
            DragSlot _dragSlot = Main.Inventory.inventoryUI.dragSlot;
            _dragSlot.SetDragSlot(this);
            _dragSlot.transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    //이놈 잡아라!!!
    public void OnDrag(PointerEventData eventData)
    {
        if (curSlot.itemData != null)
        {
            Main.Inventory.inventoryUI.dragSlot.transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log($"End Drap");
        if (Main.Inventory.inventoryUI.dragSlot.thisSlot != null)
        {
            Main.Inventory.inventoryUI.dragSlot.RemoveDragSlot();
        }
        else
        {
            return;
        }
    }


    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log($"OnDrop");

        if (Main.Inventory.inventoryUI.dragSlot.thisSlot != null)
        {
            if (!boneFireSlot)
            {
                //Debug.Log("inventorySlot");
                ChangeSlot();
            }
            else
            {
                //Debug.Log("boneFireSlot");
                ChangeSlot();
            }
        }

    }
    #endregion

    //public void BoneFireTest()
    //{
    //    if (boneFireSlot)
    //    {
    //        AddFireSlot();

    //    }
    //}

    //private void AddFireSlot()
    //{

    //}

    private void ChangeSlot()
    {
        ///<summary>
        ///SlotData의 정보를 교환 하자
        /// </summary>
        if (curSlot.IsEmpty() && !Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot.IsEmpty())//이동할 슬롯의 SlotaData가 업으면
        {
            //기존의 있던 슬롯의 데이터 제거 후 index슬롯으로 이동;
            Debug.Log($"아이템 이동");
            Main.Inventory.inventoryUI.MoveSlot(Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot, curSlot);
            //Main.Inventory.inventoryUI.SwapItem(Main.Inventory.inventoryUI.SlotIndex(index), index);
        }
        else if (!curSlot.IsEmpty() && !Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot.IsEmpty())
        {
            Debug.Log($"아이템 스왑");
            Main.Inventory.inventoryUI.SwapItem(Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot, curSlot);
        }
        else
        {
            Debug.Log($"아이템 정보 없음");
        }
    }

}
