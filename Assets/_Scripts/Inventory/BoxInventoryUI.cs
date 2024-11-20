using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInventoryUI : MonoBehaviour
{
    [Header("#Box")]
    public GameObject boxSlotsObject;
    public InventorySlot[] boxSlots;

    public void BoxInitialize() //[10/28] 나중에 수정하자 :) - 수정 완
    {
        boxSlotsObject = this.gameObject;

        Transform[] children = boxSlotsObject.GetComponentsInChildren<Transform>();
        int index = 0;
        boxSlots = new InventorySlot[12];
        for (int i = 0; i < children.Length; i++)
        {
            if (index >= boxSlots.Length) break; // 배열의 크기를 초과하지 않도록 함

            InventorySlot slot = children[i].GetComponent<InventorySlot>();
            if (slot != null)
            {
                boxSlots[index] = slot;
                index++;
            }
        }
    }

    public void BoxSlotUpdateUI()
    {

        //for (int i = 0; i < boxSlots.Length; i++)
        //{
        //    if (boxSlots[i].curSlot.itemData != null || boxSlots[i].curSlot.amount > 0) // 슬롯이 데이터가 있는지 확인
        //    {
        //        slotObjects[i].SetSlot(boxSlots[i].curSlot); // 슬롯 정보 업데이트
        //    }
        //    else /*if (_slots[i].IsEmpty())*/ if (boxSlots[i].curSlot.itemData == null || boxSlots[i].curSlot.amount <= 0)
        //    {
        //        slotObjects[i].ClearSlot(); // 빈 슬롯 처리
        //    }
        //}

        //if(!boxSlotsObject.activeInHierarchy)
        //{
        //    return;
        //}

        for (int i = 0; i < boxSlots.Length; i++)
        {
            if (!boxSlots[i].curSlot.IsEmpty())
            {
                boxSlots[i].SetSlot(boxSlots[i].curSlot);
            }
            else
            {
                boxSlots[i].ClearSlot();
            }
        }
    }

    public void BoxSlotsGet(SlotData[] Get_boxSlots) // 박스 데이터 Get 하기
    {
        for (int i = 0; i < Get_boxSlots.Length; i++)
        {
            boxSlots[i].curSlot = Get_boxSlots[i];
        }

        BoxSlotUpdateUI();
    }
}
