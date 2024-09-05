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
    public GameObject SlotUIObject;
    public Image icon;
    public TextMeshProUGUI amuontText;//갯수
    private SlotData curSlot;

    public int index;

    //[Header("#SlotData")]
    //public ItemData itemData;
    //public int amount;

    private void Awake()
    {
        
    }

    /// <summary>
    /// 슬롯에 아이템 등록
    /// </summary>
    public void SetSlot(SlotData _slotData)
    {
        curSlot = _slotData;
        //SlotUIObject.SetActive(true);
        icon.sprite = _slotData.itemData.Icon;
        amuontText.text = _slotData.amount > 1 ? _slotData.amount.ToString() : string.Empty;
    }

    /// <summary>
    /// 슬롯에서 아이템 제거
    /// </summary>
    public void ClearSlot()
    {
        //SlotUIObject.SetActive(false);

        //icon = itemData.Icon;
        //itemData = null;
        amuontText.text = string.Empty;
    }

    //public bool IsEmpty()
    //{
    //    //SlotUIObject.SetActive(false);
    //    Debug.Log($"IsEmpty() : {IsEmpty()}");
    //    return amount <= 0 && itemData == null;
    //}

    
}
