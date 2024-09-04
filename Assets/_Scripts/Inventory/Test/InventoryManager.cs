using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager
{
    #region - 메모장!
    /// <summary> - 메모장 !
    /// 1. 아이템 추가
    /// 아이템의 타입 확인 후 dicSlots에서 빈 슬롯 or 갯수++
    /// 2. 제거
    /// dicSlots에서 검색 후 갯수 확인 맞으면 그 만큼 갯수-- or 제거
    /// 3. 아이템 찾기
    /// dicSlots에서 검색해 아이템 있는지 여부
    /// 4. 특정 아이템 타입확인
    /// 셀 수 있는 아이템인지 여부
    /// </summary>
    #endregion
    //int : 슬롯 인덱스, Slot : itemdata, 갯수
    public Dictionary<int, Slot> dicSlots = new();
    public Slot[] slotObjects;
    public InventoryUI inventoryUI;

    #region - bool
    //아이템 있는지 여부
    public bool HadItem(ItemData _itemdata)
    {
        foreach (var slot in dicSlots.Values)
        {
            if (slot.itemData == _itemdata)
            {
                return true;
            }
        }
        return false;
    }

    //셀 수 있는 아이템인지 여부
    public bool IsCountTableItem(ItemData _itemdata)
    {
        return _itemdata is ContableItemData;
    }
    #endregion

    public void AddItem(ItemData _itemdata, int _amount = 1)
    {
        Debug.Log("AddItem 호출됨: " + _itemdata.DisplayName);
        if (HadItem(_itemdata) && IsCountTableItem(_itemdata))
        {
            foreach (var slot in dicSlots.Values)
            {
                // maxStack은 아이템 최대 스택 수
                if (slot.itemData == _itemdata && slot.amount < ((ContableItemData)_itemdata).MaxAmount)
                {
                    slot.amount += _amount;
                    inventoryUI.UpdateUI();
                    Debug.Log($"Add Item - index : {GetSlotIndex(slot)} | 아이템: {slot.itemData.DisplayName} | 갯수: {slot.amount}");
                    //UIUpdate
                    return;
                }
            }
        }

        // 빈 슬롯 찾기
        foreach (var slot in dicSlots.Values)
        {
            if (slot.IsEmpty())
            {
                slot.itemData = _itemdata;
                slot.amount = _amount;
                inventoryUI.UpdateUI();
                //UIUpdate
                return;
            }
        }
    }

    public void RemoveItem(ItemData itemdata, int amount = 1)
    {
        foreach (var slot in dicSlots.Values)
        {
            if (slot.itemData == itemdata)
            {
                slot.amount -= amount;
                if (slot.amount <= 0)
                {
                    slot.ClearSlot(); // 슬롯 비우기
                }
                inventoryUI.UpdateUI();
                //UpdateUI();
                return;
            }
        }
        Debug.Log("아이템을 찾을 수 없습니다.");
    }

    private int GetSlotIndex(Slot slot)
    {
        // 슬롯의 인덱스를 찾는 메서드
        foreach (var kvp in dicSlots)
        {
            if (kvp.Value == slot)
            {
                return kvp.Key;
            }
        }
        return -1; // 슬롯을 찾지 못한 경우
    }
}
