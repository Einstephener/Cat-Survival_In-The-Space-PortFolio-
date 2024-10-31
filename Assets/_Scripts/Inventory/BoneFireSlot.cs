using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFireSlot : InventorySlot // 추 후에 변경될 수 있음
{
    public InventorySlot nextSlot;
    public float cookingTime = 20f;
    public float curCookingTime = 0;
    #region UI 기능
    protected override void Awake()
    {
        base.Awake();
        //ClearOutLine();
    }

    public override void SetSlot(SlotData _slotData)
    {
        base.SetSlot(_slotData);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
    }

    //public override void isWeapon()
    //{
    //    base.isWeapon();
    //}

    public override void SetOutLine()
    {
        base.SetOutLine();
    }

    public override void ClearOutLine()
    {
        base.ClearOutLine();
    }

    #endregion


    //Slot에 아이템이 있는지 확인 후 실행 -> 
    public bool Cooking()
    {
        if (curSlot == null && curSlot.itemData == nextSlot.curSlot.itemData) // 요리할 아이템이 있으면서 완료된 요리와 요리할 아이템이 같을 경우 true
        {
            return true;
        }

        if (curSlot != null && curSlot.itemData == nextSlot.curSlot.itemData) // 요리할 아이템이 없으면서 완료된 요리와 요리할 아이템이 같을 경우 true
        {
            return true;
        }

        if (curSlot != null && nextSlot.curSlot.IsEmpty()) // 둘 다 없을 경우 true
        {
            return true;
        }

        return false;
    }
}
