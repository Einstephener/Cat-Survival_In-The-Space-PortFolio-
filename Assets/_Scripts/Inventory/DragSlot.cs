using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    #region - 메모장
    /// <summary>
    /// 선택된 슬롯의 ItemData의 Icon을 복사한다.
    /// </summary>
    #endregion
    public Slot thisSlot;
    //public TextMeshProUGUI amuontText;
    public Image icon;
    //public Slider WeaponDurability;

    //private void OnEnable()
    //{
    //    SetDragSlot(thisSlot);
    //}

    public void SetDragSlot(Slot _dragSlot)
    {
        this.gameObject.SetActive(true);
        thisSlot = _dragSlot;
        icon.sprite = thisSlot.curSlot.itemData.Icon;
        //amuontText.text = thisSlot.curSlot.amount > 1 ? thisSlot.curSlot.amount.ToString() : string.Empty;
    }

    public void RemoveDragSlot()
    {
        thisSlot = null;
        this.gameObject.SetActive(false);
    }

}
