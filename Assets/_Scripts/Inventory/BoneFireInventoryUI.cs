using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFireInventoryUI : MonoBehaviour
{
    [Header("#BoneFire")]
    public GameObject boneFireObject; // 모닥불 UI_Object
    public InventorySlot[] boneFireSlots;


    public void BoneFireInitialize()
    {
        boneFireObject = this.gameObject;

        boneFireSlots[0].curSlot = new SlotData();
        boneFireSlots[1].curSlot = new SlotData();
    }

    private void UpdateSlotUI(InventorySlot slot) // 나중에 사용하면 코드를 간소화 할 수 있을 거 같다.
    {
        if (!slot.curSlot.IsEmpty())
        {
            //Debug.Log("UpdateSlotUI");
            slot.SetSlot(slot.curSlot);
        }
        else
        {
            //Debug.Log("UpdateSlotUI");
            slot.ClearSlot();
        }
    }

    public void BoneFireUpdateUI()
    {
        UpdateSlotUI(boneFireSlots[0]);
        UpdateSlotUI(boneFireSlots[1]);

    }

    public void BoneFireSlotsGet(SlotData boneFireSlotData, SlotData nextBoneFireSlotData) // 연결하는 함수 
    {
        // 모닥불의 데이터 불러오기 ㄱㄱ
        boneFireSlots[0].curSlot = boneFireSlotData;
        boneFireSlots[1].curSlot = nextBoneFireSlotData;

        BoneFireUpdateUI();
    }
}
