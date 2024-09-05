    //using Mono.Cecil;
    //using System;
    //using System.Collections;
    //using System.Collections.Generic;
    //using UnityEngine;

    //public class Inventory : MonoBehaviour
    //{
    //    #region 메모장!
    //    ///<summary>
    //    ///1. 슬롯 아이템 생성
    //    ///2. 슬롯 아이템 제거
    //    ///3. A슬롯 B슬롯 위치 교체
    //    ///3-1.아이템이 서로 같은 constansItem인 경우
    //    ///3-2 같은 아이템인지 확인 후 합치기
    //    ///
    //    ///4. 아이템 정렬
    //    ///5. 아이템 팝업창
    //    ///6. 슬롯 상태 및 UI 갱신
    //    ///
    //    /// 
    //    /// Q.아이템 슬롯은 어떻게 관리 할 것인가?
    //    /// Q.
    //    /// </summary>
    //    /// 
    //    #endregion
    //    public Dictionary<int, Slot> slots = new();

    //    [Header("#UI")]
    //    public Slot[] inventorySlots;
    //    //public Slot[] qickslots;

    //    [Header("#Test")]
    //    public ItemData _Testitemdata;

    //    #region - bool
    //    //아이템 있는지 여부
    //    public bool HadItem(ItemData _itemdata)
    //    {
    //        foreach (var slot in slots.Values)
    //        {
    //            if (slot.itemData == _itemdata)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    //셀 수 있는 아이템인지 여부
    //    public bool IsCountTableItem(ItemData _itemdata)
    //    {
    //        return _itemdata is ContableItemData;
    //    }
    //    #endregion

    //    public void AddItem(ItemData _itemdata, int _amount = 1)
    //    {
    //        if (HadItem(_itemdata) && IsCountTableItem(_itemdata))
    //        {
    //            foreach (var slot in slots.Values)
    //            {
    //                // maxStack은 아이템 최대 스택 수
    //                if (slot.itemData == _itemdata && slot.amount < ((ContableItemData)_itemdata).MaxAmount)
    //                {
    //                    slot.amount += _amount;
    //                    UpdateUI();
    //                    return;
    //                }
    //            }
    //        }

    //        // 빈 슬롯 찾기
    //        foreach (var slot in slots.Values)
    //        {
    //            if (slot.IsEmpty())
    //            {
    //                slot.itemData = _itemdata;
    //                slot.amount = _amount;
    //                UpdateUI();
    //                return;
    //            }
    //        }
    //    }

    //    public void TestAddItem()
    //    {
    //        int _amount = 1;
    //        if (HadItem(_Testitemdata) && IsCountTableItem(_Testitemdata))
    //        {
    //            foreach (var slot in slots.Values)
    //            {
    //                // maxStack은 아이템 최대 스택 수
    //                if (slot.itemData == _Testitemdata && slot.amount < ((ContableItemData)_Testitemdata).MaxAmount)
    //                {
    //                    slot.amount += _amount;
    //                    UpdateUI();
    //                    return;
    //                }
    //            }
    //        }

    //        // 빈 슬롯 찾기
    //        foreach (var slot in slots.Values)
    //        {
    //            if (slot.IsEmpty())
    //            {
    //                slot.itemData = _Testitemdata;
    //                slot.amount = _amount;
    //                UpdateUI();
    //                return;
    //            }
    //        }
    //    }





    //    public void RemoveItem(ItemData itemdata, int amount = 1)
    //    {
    //        foreach (var slot in slots.Values)
    //        {
    //            if (slot.itemData == itemdata)
    //            {
    //                slot.amount -= amount;
    //                if (slot.amount <= 0)
    //                {
    //                    slot.ClearSlot(); // 슬롯 비우기
    //                }
    //                //UpdateUI();
    //                return;
    //            }
    //        }
    //        Debug.Log("아이템을 찾을 수 없습니다.");
    //    }

    //    private void UpdateUI()
    //    {
    //        foreach (var slot in inventorySlots)
    //        {
    //            // 각 슬롯의 UI를 갱신하는 코드
    //        }
    //    }

    //    #region - 정렬
    //    public void TrimAll()
    //    {

    //    }

    //    public void SortAll()
    //    {

    //    }
    //    #endregion

    //    #region - 아이템 스왑
    //    public void SwapItem()
    //    {

    //    }

    //    public void SeparateAmount()
    //    {

    //    }
    //    #endregion

    //}

