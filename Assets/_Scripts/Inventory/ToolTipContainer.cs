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

    public void SetToolTip(SlotData _slotData, Vector3 _pos)
    {
        this.gameObject.SetActive(true);
        //_pos += new Vector3(this.GetComponent<RectTransform>().rect.width * 0.5f, this.GetComponent<RectTransform>().rect.height * -0.5f, 0f);

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector3 offset = new Vector3(rectTransform.rect.width * 0.5f, -rectTransform.rect.height * 0.5f, 0f);
        _pos += offset;

        this.gameObject.transform.position = _pos;
        icon.sprite = _slotData.itemData.Icon;
        ItemName.text = _slotData.itemData.DisplayName;
        ItemDescription.text = _slotData.itemData.Description;
    }

    public void HideToolTip()
    {
        this.gameObject.SetActive(false);
    }
}
