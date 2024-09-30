using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase
{
    #region UI 기능
    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void SetSlot(SlotData _slotData)
    {
        base.SetSlot(_slotData);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
    }

    public override void isWeapon()
    {
        base.isWeapon();
    }
    public override void SetOutLine()
    {
        base.SetOutLine();
    }

    public override void ClearOutLine()
    {
        base.ClearOutLine();
    }

    #endregion

    public void GetPrefab()//ItemData의 프리펩 불러오는 작업
    {

    }
}
