using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("#SlotButton")]
    public Button button;
    private Outline outline;

    [Header("#SlotDataUI")]
    public Sprite image;
    public TextMeshProUGUI quantityText;//갯수

    [Header("#SlotData")]
    public ItemData itemData;
    public int amount;

    private void Awake()
    {
        
    }

    /// <summary>
    /// 슬롯에 아이템 등록
    /// </summary>
    public void SetSlot()
    {
        image = itemData.Icon;
        quantityText.text = amount > 1 ? amount.ToString() : string.Empty;
    }

    /// <summary>
    /// 슬롯에서 아이템 제거
    /// </summary>
    public void ClearSlot()
    {
        image = itemData.Icon;
        quantityText.text = string.Empty;
    }

    public bool IsEmpty()
    {
        return amount <= 0 && itemData == null;
    }

    
}
