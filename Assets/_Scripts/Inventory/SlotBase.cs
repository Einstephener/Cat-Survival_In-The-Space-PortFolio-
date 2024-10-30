using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotBase : MonoBehaviour
{
    [Header("#SlotUI")]
    public Button button;
    public Outline outline;

    [Header("#SlotData")]
    public GameObject SlotUIObject;
    public Image icon;
    public TextMeshProUGUI amuontText;//갯수
    public SlotData curSlot;
    public Slider WeaponDurability;

    public int index;
    public int _amuont;

    #region UI 기능
    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        //ClearOutLine();
    }

    public virtual void SetSlot(SlotData _slotData)
    {
        curSlot = _slotData;
        icon.gameObject.SetActive(true);
        isWeapon();
        icon.sprite = curSlot.itemData.Icon;
        _amuont = _slotData.amount;
        amuontText.text = curSlot.amount > 1 ? curSlot.amount.ToString() : string.Empty;
    }

    public virtual void ClearSlot()
    {
        icon.sprite = null;
        curSlot.itemData = null;
        isWeapon();
        //ClearOutLine();
        _amuont = 0;
        icon.gameObject.SetActive(false);
        amuontText.text = string.Empty;
    }

    public virtual void isWeapon()
    {
        if (curSlot.itemData != null)
        {
            if (Main.Inventory.IsWeaponItem(curSlot.itemData))
            {
                WeaponDurability.gameObject.SetActive(true);
            }
            else
            {
                WeaponDurability.gameObject.SetActive(false);
            }
        }
        else
        {
            WeaponDurability.gameObject.SetActive(false);
        }
    }

    public virtual void SetOutLine()
    {
        outline.enabled = true;
    }

    public virtual void ClearOutLine()
    {
        outline.enabled = false;
    }

    #endregion
}
