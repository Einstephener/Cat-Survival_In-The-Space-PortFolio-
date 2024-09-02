using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("#UI")]
    public Button button;
    public Image image;
    public TextMeshProUGUI quantityText;//갯수
    private Outline outline;

    public ItemData itemdata;

    private void Awake()
    {
        
    }

    /// <summary>
    /// 슬롯에 아이템 등록
    /// </summary>
    public void SetItem()
    {

    }

    /// <summary>
    /// 슬롯에서 아이템 제거
    /// </summary>
    public void RemoveItem()
    {

    }

    public void SetImteAmount()
    {

    }
}
