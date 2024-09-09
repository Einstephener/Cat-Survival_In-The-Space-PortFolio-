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
    public TextMeshProUGUI amuontText;
    public Image icon;
    public Slider WeaponDurability;


    public void SetDragSlot(Slot _dragSlot)
    {
        ///<summary>
        ///드레그한 슬롯 UI 정보 복사
        /// </summary>
        thisSlot = _dragSlot;
        icon.gameObject.SetActive(true);
        //isWeapon();
        icon.sprite = thisSlot.curSlot.itemData.Icon;
        amuontText.text = thisSlot.curSlot.amount > 1 ? thisSlot.curSlot.amount.ToString() : string.Empty;
    }

    public void RemoveDragSlot()
    {
        ///<summary>
        ///드레그한 슬롯 UI 정보 복사
        /// </summary>
        thisSlot = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        amuontText.text = string.Empty;
    }

}
