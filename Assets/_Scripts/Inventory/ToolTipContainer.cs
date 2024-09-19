using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipContainer : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    public void SetToolTip(SlotData _slotData)
    {
        this.gameObject.SetActive(true);
        icon.sprite = _slotData.itemData.Icon;
        ItemName.text = _slotData.itemData.DisplayName;
        ItemDescription.text = _slotData.itemData.Description;
    }

    public void HideToolTip()
    {
        this.gameObject.SetActive(false);
    }
}
