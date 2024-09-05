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
    
    private void OnEnable()
    {
        //UpdateUI();
    }


    public void AddItemUIUpdate()
    {

    }


    public void UpdateUI()
    {
        var _slots = Main.Inventory.slotsData; // InventoryManager에서 아이템 딕셔너리 가져오기

        // 슬롯 UI 업데이트
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmpty()) // 슬롯이 딕셔너리에 존재하는지 확인
            {
                slotObjects[i].SetSlot(_slots[i]); // 슬롯 정보 업데이트
            }
            else
            {
                slotObjects[i].ClearSlot(); // 빈 슬롯 처리
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
    public void SwapItem()
    {

    }

    public void SeparateAmount()
    {

    }
    #endregion
}
