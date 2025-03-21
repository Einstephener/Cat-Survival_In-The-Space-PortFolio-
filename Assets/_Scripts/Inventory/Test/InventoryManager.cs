using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlotData : Data
{
    public ItemData itemData;
    public int amount;
    public int weaponDurability;
    public bool IsEmpty()
    {
        return amount <= 0 || itemData == null;
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
    public SlotData[] slotsData = new SlotData[23];
    public InventoryUI inventoryUI;

    public ItemData InHand;

    public void Initialize()
    {
        Debug.Log("InventoryManager Initialize");
        
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        InventorySlot[] slotUI = inventoryUI.slotObjects;
        inventoryUI.quickSlotObjects = Main.UI.QuickSlots;
        for (int i = 0; i < slotsData.Length; i++)
        {
            slotsData[i] = new SlotData();
            slotUI[i].index = i;
            slotUI[i].curSlot = slotsData[i];
        }

        inventoryUI.UpdateUI();
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

    public bool IsInstallationItem(ItemData _itemdata)
    {
        return _itemdata is InstallationItemData;
    }
    
    public bool IsWeaponItem(ItemData _itemdata)
    {
        return _itemdata is WeaponItemData;
    }

    

    public bool IsPotionItem(ItemData _itemdata)
    {
        return _itemdata is PotionItemData;
    }

    // 특정 아이템의 갯수를 전부 반환하는 함수
    public int GetTotalItemCount(ItemData _itemdata)
    {
        int totalCount = 0;

        foreach (var slot in slotsData)
        {
            if (slot != null && slot.itemData == _itemdata)
            {
                // 셀 수 있는 아이템인지 확인
                if (IsCountTableItem(_itemdata))
                {
                    totalCount += slot.amount; // 갯수 누적
                }
            }
        }

        return totalCount; // 총 갯수 반환
    }
    #endregion


    public void AddItem(ItemData _itemdata, int _amount = 1)
    {
        if (HadItem(_itemdata) && IsCountTableItem(_itemdata))
        {
            foreach (var slot in slotsData)
            {
                if (slot != null && slot.itemData == _itemdata && slot.amount < ((ContableItemData)_itemdata).MaxAmount)
                {
                    slot.amount += _amount;
                    inventoryUI.UpdateUI();
                    //Debug.Log($"{slot.itemData} += {_itemdata} // Index : Null ItemName : {slot.itemData.DisplayName}, Amunt : {slot.amount}");

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
                //Debug.Log($"{boxSlotsData[i].itemData} += {_itemdata} //Index : {i} ItemName : {boxSlotsData[i].itemData.DisplayName}, Amunt : {boxSlotsData[i].amount}");
                inventoryUI.UpdateUI();
                return;
            }
        }

        Debug.Log("슬롯이 가득 찼습니다.");
        //[기혁님의 요청사항 10/01] : 인벤토리가 다 찼을 때 플레이어 위치 앞에 아이템이 생성되도록 해줭
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
                    inventoryUI.UpdateUI();
                }
                inventoryUI.UpdateUI();
                return;
            }
        }
        Debug.Log("아이템을 찾을 수 없습니다.");
    }


    public void Select_AddItem(ItemData _itemdata, int _amount = 1, int slotIndex = -1)
    {
        if (HadItem(_itemdata) && IsCountTableItem(_itemdata))
        {
            // 특정 슬롯에 아이템 추가
            if (slotIndex >= 0 && slotIndex < slotsData.Length)
            {
                var slot = slotsData[slotIndex];
                if (slot != null && slot.itemData == _itemdata && slot.amount < ((ContableItemData)_itemdata).MaxAmount)
                {
                    slot.amount += _amount;
                    inventoryUI.UpdateUI();
                    //Debug.Log($"{slot.itemData} += {_itemdata} // Index : {slotIndex} ItemName : {slot.itemData.DisplayName}, Amount : {slot.amount}");
                    return;
                }
                else if (slot == null || slot.IsEmpty())
                {
                    slotsData[slotIndex] = new SlotData();
                    slotsData[slotIndex].itemData = _itemdata;
                    slotsData[slotIndex].amount = _amount;
                    //Debug.Log($"{boxSlotsData[slotIndex].itemData} += {_itemdata} // Index : {slotIndex} ItemName : {boxSlotsData[slotIndex].itemData.DisplayName}, Amount : {boxSlotsData[slotIndex].amount}");
                    inventoryUI.UpdateUI();
                    return;
                }
            }
        }

        // 빈 슬롯 찾기 (기존 로직)
        for (int i = 0; i < slotsData.Length; i++)
        {
            if (slotsData[i] == null || slotsData[i].IsEmpty())
            {
                slotsData[i] = new SlotData();
                slotsData[i].itemData = _itemdata;
                slotsData[i].amount = _amount;
                //Debug.Log($"{boxSlotsData[i].itemData} += {_itemdata} // Index : {i} ItemName : {boxSlotsData[i].itemData.DisplayName}, Amount : {boxSlotsData[i].amount}");
                inventoryUI.UpdateUI();
                return;
            }
        }

        Debug.Log("슬롯이 가득 찼습니다.");
        //[기혁님의 요청사항 10/01] : 인벤토리가 다 찼을 때 플레이어 위치 앞에 아이템이 생성되도록 해줭
    }

    public void Select_RemoveItem(int slotIndex, int _amount = 1)
    {
        // 슬롯 인덱스가 유효한지 확인
        if (slotIndex >= 0 && slotIndex < slotsData.Length)
        {
            var slot = slotsData[slotIndex];

            // 슬롯이 존재하고 아이템이 있는지 확인
            if (slot != null && !slot.IsEmpty())
            {
                // 아이템 수량이 제거할 수량보다 큰 경우
                if (slot.amount > _amount)
                {
                    slot.amount -= _amount;
                    // 수량이 변경된 후 UI 업데이트
                    //inventoryUI.UpdateUI();
                    //Debug.Log($"Removed {_amount} from {slot.itemData.DisplayName} // Index: {slotIndex}, New Amount: {slot.amount}");
                }
                // 아이템 수량이 제거할 수량과 같은 경우
                else if (slot.amount == _amount)
                {
                    // 슬롯을 비움
                    slot.itemData = null;
                    slot.amount -= _amount;
                    inventoryUI.UpdateUI();
                    inventoryUI.WhetherSelectSlot();
                    //Debug.Log($"Removed all items from {slot.itemData.DisplayName} // Index: {slotIndex}, New Amount: {slot.amount}");
                }
                // 아이템 수량이 제거할 수량보다 적은 경우
                else
                {
                    Debug.Log("제거할 수량이 현재 수량보다 많습니다.");
                }
            }
            else
            {
                Debug.Log("해당 슬롯에 아이템이 없습니다.");
            }
        }
        else
        {
            Debug.Log("유효하지 않은 슬롯 인덱스입니다.");
        }
        inventoryUI.UpdateUI();

    }
    #region DeBug

    #endregion
}
