using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public SlotData curSlot;

    public int index;
    public int _amuont;


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
        _amuont = curSlot.amount;

        SlotUIObject.SetActive(true);
        curSlot = _slotData;
        icon = curSlot.itemData.Icon;
        amuontText.text = curSlot.amount > 1 ? curSlot.amount.ToString() : string.Empty;
    }

    /// <summary>
    /// 슬롯에서 아이템 제거
    /// </summary>
    public void ClearSlot()
    {
        _amuont = curSlot.amount;

        icon.gameObject.SetActive(false);
        curSlot = null;
        amuontText.text = string.Empty;
    }

    //public bool IsEmpty()
    //{
    //    //SlotUIObject.SetActive(false);
    //    Debug.Log($"IsEmpty() : {IsEmpty()}");
    //    return amount <= 0 && itemData == null;
    //}

    
}
