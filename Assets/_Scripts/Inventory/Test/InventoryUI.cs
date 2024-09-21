using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : /*MonoBehaviour*/ UI_Popup
{
    //public GameObject inventoryUI;
    #region 메모장!
    ///<summary>
    ///1. 슬롯 UI 업데이트
    ///갯수 변경?
    ///2. 정렬
    ///이름순으로 정렬 할 계획
    ///3. 스왑
    ///선택한 슬롯 두 개의 정보를 변경하거나, 같은 아이템이면 합치기
    ///4. 아이템 정보창
    ///5. ContableItem의 갯수 정렬
    ///(130개의 아이템이면 99, 31 개로 분할? 할 수 있도록 도와주는 코드)
    ///
    /// </summary>
    #endregion
    public Slot[] slotObjects;
    public Slot selectSlot = null;
    public DragSlot dragSlot;
    public ToolTipContainer toolTipContainer;

    public ItemData testItemData1;
    public ItemData testItemData2;
    public ItemData testItemData3;

    private void Awake()
    {
        //임시 초기화
        Main.Inventory.Initialize();
        //Test
        Main.Inventory.AddItem(testItemData1, 10);
        Main.Inventory.AddItem(testItemData2, 99);
        Main.Inventory.AddItem(testItemData2, 29);
        Main.Inventory.AddItem(testItemData3);
        //Main.Inventory.RemoveItem(testItemData1, 7);
        //Main.Inventory.RemoveItem(testItemData2, 5);
    }

    public void UpdateUI()
    {
        SlotData[] _slots = Main.Inventory.slotsData; // InventoryManager SlotData 가지고 오기

        // 슬롯 UI 업데이트
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].itemData != null) // 슬롯이 데이터가 있는지 확인
            {
                slotObjects[i].SetSlot(_slots[i]); // 슬롯 정보 업데이트
            }
            else
            {
                slotObjects[i].ClearSlot(); // 빈 슬롯 처리
            }
        }
    }

    //디버그 호출 함수 Test용
    public void DebugSlotObjects()
    {
        for (int i = 0; i < slotObjects.Length; i++)
        {
            if (slotObjects[i] != null && slotObjects[i].curSlot.itemData != null)
            {
                Debug.Log($"Slot {i}: Item - {slotObjects[i].curSlot.itemData.DisplayName}, Amount - {slotObjects[i].curSlot.amount}");
            }
            else
            {
                Debug.Log($"Slot {i}: Empty Slot");
            }
        }
    }


    #region - 정렬
    public void TrimAll()
    {

    }

    public void SortAll()
    {

    }
    #endregion

    #region - 아이템 스왑
    public void SwapItem(SlotData dragSlotData, SlotData dropSlotData)
    {
        if (dragSlotData.itemData != null && dropSlotData != null)
        {
            if (dragSlotData.itemData != dropSlotData.itemData)
            {
                //Debug.Log($"[InventoryUI] SwapItem Before - dragSlotData : {dragSlotData.itemData.DisplayName}, {dragSlotData.amount}/ dropSlotData : {dropSlotData.itemData.DisplayName}, {dropSlotData.amount}");
                MoveSlot(dragSlotData, dropSlotData);
                //Debug.Log($"[InventoryUI] SwapItem After - dragSlotData : {dragSlotData.itemData.DisplayName}, {dragSlotData.amount}/ dropSlotData : {dropSlotData.itemData.DisplayName}, {dropSlotData.amount}");
            }
            else if (dragSlotData.itemData == dropSlotData.itemData && Main.Inventory.IsCountTableItem(dragSlotData.itemData))
            {
                //Debug.Log("같은 아이템이며 셀 수 있는 아이템이넹");
                //두 아이템이 같은 ItemData이면서 셀 수 있는 아이템이면 SeparateAmount() 함수를 통해 갯수를 합치고 만약 함친 값이 MaxAmount보다 높으면 MaxAmount, @ 헤서 함치자 예압
            }
        }
        else
        {
            Debug.Log($"아이템의 정보가 없습니다");
        }
    }

    public void MoveSlot(SlotData dragSlotData, SlotData dropSlotData)
    {
        if (dragSlotData.itemData != null && dropSlotData != null)
        {
            ItemData tempSlotaData_ItemData = dragSlotData.itemData;
            int tempAmount = dragSlotData.amount;

            dragSlotData.itemData = dropSlotData.itemData;
            dragSlotData.amount = dropSlotData.amount;

            dropSlotData.itemData = tempSlotaData_ItemData;
            dropSlotData.amount = tempAmount;

            UpdateUI();
        }
        else
        {
            Debug.Log($"아이템의 정보가 없습니다");
        }
    }

    public void SeparateAmount()
    {

    }
    #endregion
}
