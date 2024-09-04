using System.Collections;
using System.Collections.Generic;
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
    public Slot[] slotObjects = Main.Inventory.slotObjects;

    //public void OnUI_Inventory()
    //{

    //}

    //bool isToggle()
    //{
    //    return inventoryUI.activeInHierarchy;
    //}

    

    public void UpdateUI()
    {
        //foreach (var slot in inventorySlots)
        //{
        //    // 각 슬롯의 UI를 갱신하는 코드
        //}

        for (int i = 0; i < Main.Inventory.dicSlots.Count; i++)
        {
            if (slotObjects[i] != null)
            {
                Slot slot = Main.Inventory.dicSlots[i];

                slotObjects[i].SetSlot();
                if (slot.IsEmpty())
                {
                    slotObjects[i].ClearSlot();
                }
                else
                {
                    slotObjects[i].SetSlot();
                }
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
