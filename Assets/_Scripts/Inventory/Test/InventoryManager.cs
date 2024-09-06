using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;
public class SlotData
{
    public ItemData itemData;
    public int amount;

    public bool IsEmpty()
    {
        return amount <= 0 && itemData == null;
    }
}

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
    //public Dictionary<int, Slot> dicSlots = new();
    public SlotData[] slotsData = new SlotData[18];
    public InventoryUI inventoryUI;

    public void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        Debug.Log("InventoryManager Initialize");
        //if (inventoryUI == null)
        //{
        //    Debug.LogError("InventoryUI가 할당되지 않았습니다.");
        //}
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        Slot[] slotUI = inventoryUI.slotObjects;
        //slotsData = new SlotData[slotUI.Length];

        for (int i = 0; i < slotsData.Length; i++)
        {
            slotsData[i] = new SlotData();
            slotUI[i].index = i;
            slotUI[i].ClearSlot();
            //inventoryUI.UpdateUI();
        }
    }

    #region - bool
    //아이템 있는지 여부
    public bool HadItem(ItemData _itemdata, int amount = 1)
    {

        foreach (var slot in slotsData)
        {
            if (slot != null && slot.itemData == _itemdata)
            {
                // 해당 아이템의 갯수가 충분한지 확인
                if (slot.amount >= amount)
                {
                    return true; // 아이템과 갯수가 충분하면 true 반환
                }
                else
                {
                    Debug.Log($"HadItem() : Check {_itemdata.DisplayName} is not amount");
                    return false;
                }
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

        //for (int i = 0; i < slotsData.Length; i++)
        //{
        //    if (slotsData[i].itemData == null)
        //    {
        //        Debug.Log($"not Data");
        //    }
        //    else
        //    {
        //        Debug.Log($"{slotsData[i].itemData.name}, {slotsData[i].amount}");
        //    }
        //}

        if (HadItem(_itemdata) && IsCountTableItem(_itemdata))
        {
            foreach (var slot in slotsData)
            {
                if (slot != null && slot.itemData == _itemdata && slot.amount < ((ContableItemData)_itemdata).MaxAmount)
                {
                    slot.amount += _amount;
                    //inventoryUI.UpdateUI();
                    Debug.Log($"{slot.itemData} += {_itemdata} // Index : Null ItemName : {slot.itemData.DisplayName}, Amunt : {slot.amount}");

                    //Debug.Log($"Add Item - index : {GetSlotIndex(slotsData)} | 아이템: {slotsData.itemData.DisplayName} | 갯수: {slotsData.amount}");
                    return;
                }
            }
        }

        // 빈 슬롯 찾기
        for (int i = 0; i < slotsData.Length; i++)
        {
            if (slotsData[i] == null || slotsData[i].IsEmpty())
            {
                slotsData[i] = new SlotData();
                slotsData[i].itemData = _itemdata;
                slotsData[i].amount = _amount;
                Debug.Log($"{slotsData[i].itemData} += {_itemdata} //Index : {i} ItemName : {slotsData[i].itemData.DisplayName}, Amunt : {slotsData[i].amount}");
                //inventoryUI.UpdateUI();
                return;
            }
        }

        Debug.Log("슬롯이 가득 찼습니다.");
    }

    public void RemoveItem(ItemData itemdata, int amount = 1)
    {
        foreach (var slot in slotsData)
        {
            if (slot != null && slot.itemData == itemdata)
            {
                slot.amount -= amount;
                if (slot.IsEmpty())
                {
                    //slotsData 데어터 제거
                    //UIUpdate
                    inventoryUI.UpdateUI();
                }
                inventoryUI.UpdateUI();
                return;
            }
        }
        Debug.Log("아이템을 찾을 수 없습니다.");
    }

    #region DeBug
    
    #endregion
}
