using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Pathfinding.Drawing.Palette.Colorbrewer;

public class InventoryUI : /*MonoBehaviour*/ UI_Popup
{
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
    [Header("#Slots")]
    public InventorySlot[] slotObjects;
    public QuickSlot[] quickSlotObjects;

    [Header("#BoneFire")]
    public BoneFireInventoryUI boneFireInventoryUI;

    [Header("#Box")]
    public BoxInventoryUI boxInventoryUI;


    [HideInInspector] public InventorySlot curSlot;
    [HideInInspector] public InventorySlot nextSlot;

    [Header("#Inventory_Info")]
    public SlotBase selectSlot;
    public DragSlot dragSlot;
    public ToolTipContainer toolTipContainer;
    public RectTransform parentTransform; // inventory 창 크기

    public bool shortcutKey { get; private set; } = false;

    [Header("#Test")]
    public ItemData testItemData1;
    public ItemData testItemData2;
    public ItemData testItemData3;
    public ItemData testItemData4;
    public ItemData testItemData5;

    public EquipManager equipManager;

    private void Awake()
    {
        //임시 초기화
        Main.Inventory.Initialize();

        #region Test
        //Main.Inventory.RemoveItem(testItemData1, 7);
        //Main.Inventory.RemoveItem(testItemData2, 5);
        //this.gameObject.SetActive(false);
        Main.Inventory.AddItem(testItemData1, 20);
        Main.Inventory.AddItem(testItemData2, 20);
        //Main.Inventory.AddItem(testItemData1, 10);

        //Main.Inventory.Select_AddItem(testItemData2, 29, 7);
        //Main.Inventory.Select_AddItem(testItemData2, 29, 9);
        //Main.Inventory.Select_AddItem(testItemData2, 29, 11);
        Main.Inventory.AddItem(testItemData3);
        //Main.Inventory.AddItem(testItemData4);
        Main.Inventory.AddItem(testItemData4);
        Main.Inventory.AddItem(testItemData5);
        #endregion

        //모닥불 
        //BoneFireInitialize();
        boneFireInventoryUI.BoneFireInitialize();
        //창고 [10/24] - 수정해야 함
        //BoxInitialize();
        boxInventoryUI.BoxInitialize();
        //InventoryTotalUpdateUI();
        equipManager = FindObjectOfType<EquipManager>();
        AdjustParentHeight();
        SelectSlot(0);
    }

    #region QuickSlot
    public void QuickSlot()
    {
        for (int i = 0; i < quickSlotObjects.Length; i++)
        {
            quickSlotObjects[i].curSlot = slotObjects[i].curSlot;
        }
        QuickSlotUpdateUI();
    }
    public void QuickSlotUpdateUI()
    {
        SlotData[] _slots = Main.Inventory.slotsData;

        for (int i = 0; i < quickSlotObjects.Length; i++)
        {
            if (quickSlotObjects[i].curSlot.itemData != null) // 슬롯이 데이터가 있는지 확인
            {
                quickSlotObjects[i].SetSlot(_slots[i]); // 슬롯 정보 업데이트
                //Debug.Log($"InventorySlot {i}: Item - {quickSlotObjects[i].curSlot.itemData.DisplayName}, Amount - {quickSlotObjects[i].curSlot.amount}");

            }
            else
            {
                quickSlotObjects[i].ClearSlot(); // 빈 슬롯 처리
            }
        }
    }

    #endregion

    public bool ShortcutKey(bool isPress)
    {
        shortcutKey = isPress;
        return shortcutKey ? true : false;
    }

    public void AdjustParentHeight() // UI 크기 셋팅
    {
        float totalHeight = 140f;//위 아래 빈 공간
        //구조상 이렇게 해야함 :(
        foreach (RectTransform child in parentTransform)
        {
            foreach (RectTransform grandChild in child)
            {
                // 자식 오브젝트가 활성화되어 있는지 확인
                if (grandChild.gameObject.activeSelf)
                {
                    // 자식의 높이를 추가
                    totalHeight += grandChild.sizeDelta.y;
                }
            }
        }

        // 부모의 높이를 자식 오브젝트의 총 높이에 설정
        Vector2 parentSize = parentTransform.sizeDelta;
        parentSize.y = totalHeight;
        parentTransform.sizeDelta = parentSize;
    }

    public void InventoryUISet()
    {
        boneFireInventoryUI.boneFireObject.SetActive(false);

        boxInventoryUI.boxSlotsObject.SetActive(false);

        Main.Inventory.inventoryUI.AdjustParentHeight();
    }

    public void UpdateUI()
    {

        SlotData[] _slots = Main.Inventory.slotsData; // InventoryManager SlotData 가지고 오기

        // 슬롯 UI 업데이트
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmpty()) // 슬롯이 데이터가 있는지 확인
            {
                slotObjects[i].SetSlot(_slots[i]); // 슬롯 정보 업데이트
            }
            else
            {
                slotObjects[i].ClearSlot(); // 빈 슬롯 처리
            }
        }

        QuickSlot();
    }

    //디버그 호출 함수 Test용
    public void DebugSlotObjects()
    {
        for (int i = 0; i < slotObjects.Length; i++)
        {
            if (slotObjects[i] != null && slotObjects[i].curSlot.itemData != null)
            {
                Debug.Log($"InventorySlot {i}: Item - {slotObjects[i].curSlot.itemData.DisplayName}, Amount - {slotObjects[i].curSlot.amount}");
            }
            else
            {
                Debug.Log($"InventorySlot {i}: Empty InventorySlot");
            }
        }
    }

    public void SelectSlot()
    {
        SelectSlot(selectSlot.index);
    }

    public void SelectSlot(int index)
    {
        if (selectSlot != quickSlotObjects[index])
        {
            selectSlot = slotObjects[index];

            quickSlotObjects[index].SetOutLine();

            //equipManager 관련 버그(에러) : 선택된 슬롯의 아이템을 이동 시켜도 그 아이템을 들고 있는 버그가 있음 이걸 Update문이나 이벤트에서 관리를 해야할 거 같음 - 수정 완료[10/24]
            equipManager.EquipNew(selectSlot.curSlot.itemData);// 임시
            if (selectSlot.curSlot.itemData == null)
            {
                equipManager.UnEquip();
            }

            for (int i = 0; i < quickSlotObjects.Length; i++)
            {
                if (i != index)
                {
                    //selectSlot = quickSlotObjects[i];
                    quickSlotObjects[i].ClearOutLine();
                }
            }

            // 들고 있는 도구
            Main.Inventory.InHand = selectSlot.curSlot.itemData;
        }
    }
    public void SelectSlotRemove()
    {
        Main.Inventory.RemoveItem(selectSlot.curSlot.itemData);
    }

    public void WhetherSelectSlot() // 현재 장착된 슬릇
    {
        //1. 장착한 슬롯을 가지고 온다
        //2. 슬롯의 아이템 데이터가 없으면 retrun
        //3. 있으면 Equipmanager.unEquip();
        //이걸 모든 이벤트에 적용을 해야함;;
        //예를들어 스왑, 아이템 사용 등 전부 추가해야함;

        if (selectSlot == null)
        {
            return;
        }
        equipManager.UnEquip();

    }


    public ItemData GetSelectItemData()
    {
        if (selectSlot.curSlot.itemData != null)
        {
            //Debug.Log($"{selectSlot.curSlot.itemData}");
            return selectSlot.curSlot.itemData;
        }
        else
        {
            //Debug.Log($"{selectSlot.curSlot.itemData}");
            return null;
        }
    }

    public void InventoryTotalUpdateUI()
    {
        //인벤토리,화로,창고 UI Update
        if (this.gameObject.activeInHierarchy)
        {
            UpdateUI();
        }

        if (boneFireInventoryUI.boneFireObject.activeInHierarchy)
        {
            boneFireInventoryUI.BoneFireUpdateUI();
        }

        if (boxInventoryUI.boxSlotsObject.activeInHierarchy)
        {
            boxInventoryUI.BoxSlotUpdateUI();
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
                //Debug.Log("Test");

                //Debug.Log($"[InventoryUI] SwapItem After - dragSlotData : {dragSlotData.itemData.DisplayName}, {dragSlotData.amount}/ dropSlotData : {dropSlotData.itemData.DisplayName}, {dropSlotData.amount}");
            }
            else if (dragSlotData.itemData == dropSlotData.itemData && Main.Inventory.IsCountTableItem(dragSlotData.itemData))
            {
                //Debug.Log("같은 아이템이며 셀 수 있는 아이템이넹");
                //두 아이템이 같은 ItemData이면서 셀 수 있는 아이템이면 SeparateAmount() 함수를 통해 갯수를 합치고 만약 함친 값이 MaxAmount보다 높으면 MaxAmount, @ 헤서 함치자 예압
                CombineSlots(dragSlotData, dropSlotData);

            }
        }
        else
        {
            Debug.Log($"아이템의 정보가 없습니다");
        }
    }

    public void MoveSlot(SlotData dragSlotData, SlotData dropSlotData) // 두 슬롯의 데이터를 교환하는 함수
    {
        if (dragSlotData.itemData != null && dropSlotData != null)
        {

            ItemData tempSlotaData_ItemData = dragSlotData.itemData;
            int tempAmount = dragSlotData.amount;

            dragSlotData.itemData = dropSlotData.itemData;
            dragSlotData.amount = dropSlotData.amount;

            dropSlotData.itemData = tempSlotaData_ItemData;
            dropSlotData.amount = tempAmount;

            equipManager.SwapEquip(dragSlotData, dropSlotData, selectSlot);

        }
        else
        {
            Debug.Log($"아이템의 정보가 없습니다");
        }

        InventoryTotalUpdateUI();
    }

    public void CombineSlots(SlotData dragSlotData, SlotData dropSlotData) // 같은 아이템이면 병합하는 함수
    {
        if (dragSlotData.itemData != dropSlotData.itemData)
        {
            //Debug.Log($"InventoruUI - CombineSlots() : dragSlotData != dropSlotData // return");
            return;
        }

        int totalItems = dragSlotData.amount + dropSlotData.amount;

        if (totalItems > 99)
        {
            //Debug.Log($"1 dragSlotData: {dragSlotData.amount}, dropSlotData: {dropSlotData.amount}");

            dropSlotData.amount = 99;
            dragSlotData.amount = totalItems - 99;

            //Debug.Log($"2 dragSlotData: {dragSlotData.amount}, dropSlotData: {dropSlotData.amount}");
        }
        else
        {
            if (curSlot.index != nextSlot.index)
            {
                dropSlotData.amount = totalItems;
                dragSlotData.amount = 0;
                dragSlotData.itemData = null;
            }

            if (dragSlotData == selectSlot.curSlot)
            {
                WhetherSelectSlot();
            }
        }

        InventoryTotalUpdateUI();
    }

    public void SwapSlot(InventorySlot _curSlot, InventorySlot _nextSlot)
    {
        this.curSlot = _curSlot;
        this.nextSlot = _nextSlot;
    }

    public void HalfSlotAmount(SlotData dragSlotData, SlotData dropSlotData)
    {
        ///<summary> 반띵 조건
        ///1. 반띵할 대상이 있어야 할 것
        ///2. 대상이 셀 수 있는지 확인 할 것
        ///3. 대상의 갯수가 2개 이상이여하 할것
        ///4. 갯수를 넘겨주는 대상이 같은 아이템이거나, 없어야 할 것
        /// </summary>
        if (dragSlotData.IsEmpty() || !Main.Inventory.IsCountTableItem(dragSlotData.itemData) || dragSlotData.amount <= 1 || !dropSlotData.IsEmpty() && dragSlotData.itemData != dropSlotData.itemData)
        {
            return;
        }

        if (dropSlotData.IsEmpty())
        {
            dropSlotData.itemData = dragSlotData.itemData;
            dropSlotData.amount = 0;
        }

        if (dragSlotData.amount % 2 == 0)
        {
            //Debug.Log("even");
            int temp = dragSlotData.amount / 2;
            dragSlotData.amount -= temp;
            dropSlotData.amount += temp;
        }
        else
        {
            //Debug.Log("odd");
            int temp = dragSlotData.amount / 2;
            dragSlotData.amount = temp + 1;
            dropSlotData.amount += temp;
        }

        InventoryTotalUpdateUI();
    }
    #endregion

}
