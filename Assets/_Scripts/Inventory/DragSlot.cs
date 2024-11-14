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
    public InventorySlot thisSlot;
    public Image icon;
    public TextMeshProUGUI amuontText;
    //public Slider weaponDurability;

    public void SetDragSlot(InventorySlot _dragSlot)
    {
        this.gameObject.SetActive(true);
        thisSlot = _dragSlot;
        Main.Inventory.inventoryUI.curSlot = thisSlot;
        icon.sprite = thisSlot.curSlot.itemData.Icon;
        amuontText.text = thisSlot.curSlot.amount > 1 ? thisSlot.curSlot.amount.ToString() : string.Empty;
    }

    public void RemoveDragSlot()
    {
        thisSlot = null;
        Main.Inventory.inventoryUI.curSlot = thisSlot;
        this.gameObject.SetActive(false);
    }

}
