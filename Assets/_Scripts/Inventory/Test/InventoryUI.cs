using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour /*UI_Popup*/
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

    public ItemData TestItemData;

    //public void OnUI_Inventory()
    //{

    //}

    //bool isToggle()
    //{
    //    return inventoryUI.activeInHierarchy;
    //}
    private void OnEnable()
    {
        UpdateUI();
    }


    public void UpdateUI()
    {
        //foreach (var slot in inventorySlots)
        //{
        //    // 각 슬롯의 UI를 갱신하는 코드
        //}

        var dicSlots = Main.Inventory.GetItems(); // 아이템 딕셔너리 가져오기

        foreach (var entry in dicSlots)
        {
            int index = entry.Key; // 슬롯 인덱스
            Slot slot = entry.Value; // 슬롯 정보

            if (index < slotObjects.Length) // 인덱스가 배열 범위 내에 있는지 확인
            {
                slotObjects[index].SetSlot();
                //slotObjects[index].SetItemText($"{slot.itemData.DisplayName} (수량: {slot.amount})"); // 슬롯 텍스트 업데이트
            }
        }

        // 빈 슬롯 처리
        for (int i = 0; i < slotObjects.Length; i++)
        {
            if (!dicSlots.ContainsKey(i)) // 해당 인덱스에 아이템이 없으면
            {
                slotObjects[i].SetSlot();
                //slotObjects[i].SetItemText("빈 슬롯"); // 빈 슬롯 텍스트 설정
            }
        }

        ////
        //for (int i = 0; i < Main.Inventory.dicSlots.Count; i++)
        //{
        //    if (slotObjects[i] != null)
        //    {
        //        Slot slot = Main.Inventory.dicSlots[i];

        //        slotObjects[i].SetSlot();
        //        if (slot.IsEmpty())
        //        {
        //            slotObjects[i].ClearSlot();
        //        }
        //        else
        //        {
        //            slotObjects[i].SetSlot();
        //        }
        //    }
        //}
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
    public void SwapItem()
    {

    }

    public void SeparateAmount()
    {

    }
    #endregion
}
