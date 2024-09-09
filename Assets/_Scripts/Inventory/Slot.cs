using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

#region - 상속자들!
/// <summary> 
/// 1. IBeginDragHandler
///     ㄴ드래그가 시작될 때 호출됩니다.
/// 2. IDragHandler
///     ㄴ 드래그 중에 호출됩니다.
/// 3. IEndDragHandler
///     ㄴ드래그가 끝났을 때 호출됩니다.
/// 4. IPointerEnterHandler
///     ㄴ마우스 포인터가 UI 요소 위로 들어올 때 호출됩니다.
/// 5. IPointerExitHandler
///     ㄴ마우스 포인터가 UI 요소에서 나갈 때 호출됩니다.
/// </summary>
#endregion
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("#SlotButton")]
    public Button button;
    private Outline outline;    

    [Header("#SlotDataUI")]
    public GameObject SlotUIObject;
    public Image icon;
    public TextMeshProUGUI amuontText;//갯수
    public SlotData curSlot;
    public Slider WeaponDurability;

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
        //_amuont = curSlot.amount == 0 ? _amuont = 0 : _amuont = 1;

        //if (curSlot == null)
        //{
        //    Debug.Log("curSlot null");
        //}
        
        curSlot = _slotData;
        icon.gameObject.SetActive(true);
        isWeapon();
        icon.sprite = curSlot.itemData.Icon;
        amuontText.text = curSlot.amount > 1 ? curSlot.amount.ToString() : string.Empty;
    }

    /// <summary>
    /// 슬롯에서 아이템 제거
    /// </summary>
    public void ClearSlot()
    {
        //_amuont = curSlot.amount == 0 ? _amuont = 0 : _amuont = 1;
        //if (curSlot == null )
        //{
        //    Debug.Log("curSlot null");
        //}
        icon.sprite = null;
        curSlot.itemData = null;
        isWeapon();
        icon.gameObject.SetActive(false);
        amuontText.text = string.Empty;
    }

    public void isWeapon()
    {
        if (curSlot.itemData != null)
        {
            if (!Main.Inventory.IsCountTableItem(curSlot.itemData))
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

    //public bool IsEmpty()
    //{
    //    //SlotUIObject.SetActive(false);
    //    Debug.Log($"IsEmpty() : {IsEmpty()}");
    //    return amount <= 0 && itemData == null;
    //}

    //마우스 드래그가 시작 됐을 때 발생하는 이벤트

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (curSlot.itemData != null)
            {
                Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ///<summary>
        ///아이템 있는지 확인 
        /// DaragSlot에 연결하여 복사
        /// DaragSlot을 PointerEventData연결
        /// </summary>
        if(curSlot.itemData != null)
        {
            Main.Inventory.inventoryUI.dragSlot.thisSlot = this;
            Main.Inventory.inventoryUI.dragSlot.SetDragSlot(this);
            Main.Inventory.inventoryUI.dragSlot.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        ///<summary>
        ///DaragSlot을 PointerEventData 계속 연결
        /// </summary>
        if(curSlot.itemData != null)
        {
            Main.Inventory.inventoryUI.dragSlot.transform.position = eventData.position;
        }
    }


    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        ///<summary>
        ///DaragSlot 연결 해제
        /// </summary>

        Main.Inventory.inventoryUI.dragSlot.RemoveDragSlot();
        Main.Inventory.inventoryUI.dragSlot = null;
    }




    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        ///<summary>
        ///슬롯의 데이터를 스왑한다.
        /// </summary>
        Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");

        if (Main.Inventory.inventoryUI.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        ///<summary>
        ///SlotData의 정보를 교환 하자
        /// </summary>

        SlotData tempSlotData = curSlot;

        if (curSlot.IsEmpty())
        {
            //이동's
        }
        else
        {
            //체체체체체인지
        }

    }

}
