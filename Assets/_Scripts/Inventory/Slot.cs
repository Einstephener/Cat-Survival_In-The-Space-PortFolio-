using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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


    /// <summary>
    /// 슬롯에 아이템 등록
    /// </summary>
    public void SetSlot(SlotData _slotData)
    {
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

    //마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");

            if (curSlot.itemData != null)
            {
                Debug.Log($"{curSlot.itemData} aount : {curSlot.amount}");
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (curSlot.itemData != null)
        {
            DragSlot _dragSlot = Main.Inventory.inventoryUI.dragSlot;
            _dragSlot.SetDragSlot(this);
            _dragSlot.transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    //이놈 잡아라!!!
    public void OnDrag(PointerEventData eventData)
    {
        if (curSlot.itemData != null)
        {
            Main.Inventory.inventoryUI.dragSlot.transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End Drap");

        Main.Inventory.inventoryUI.dragSlot.RemoveDragSlot();
    }


    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop");

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

        if (curSlot.IsEmpty())//이동할 슬롯의 SlotaData가 업으면
        {
            //기존의 있던 슬롯의 데이터 제거 후 index슬롯으로 이동;
            Debug.Log($"아이템 이동");
            Main.Inventory.inventoryUI.MoveSlot(Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot, curSlot);
            //Main.Inventory.inventoryUI.SwapItem(Main.Inventory.inventoryUI.SlotIndex(index), index);
        }
        else
        {
            Debug.Log($"아이템 스왑");
            Main.Inventory.inventoryUI.SwapItem(Main.Inventory.inventoryUI.dragSlot.thisSlot.curSlot, curSlot);
        }
    }
}
