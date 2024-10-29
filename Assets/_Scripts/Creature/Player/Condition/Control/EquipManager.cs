using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;
    [SerializeField] private ItemData _itemData;

    private PlayerInputController controller;
    private PlayerCondition condition;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        condition = GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        //ItemDataSO 에 장착 아이템 추가 ㄲ
        //curEquip = Main.Inventory.inventoryUI.selectSlot.curSlot.itemData.EquipPrefab.GetComponent<Equip>();
    }
    public void OnFire()
    {
        //공격 관련
        if (curEquip != null)// [10/12] 조건문 추가해야 함
        {
            curEquip.OnAttacking();
        }
    }

    public void OnInteract(InputValue value)
    {
        if(!curEquip)
        {
            //Debug.Log($"EquipManager - OnInteract : null Equiped Item");
            return;
        }

        if (_itemData.Type == ItemType.Consumable) //에러/errer
        {
            curEquip.OnEating();

            if (_itemData.item is Potion) // [10/21] 임시
            {
                Potion potion = _itemData.item as Potion;
                if (potion.playerCondition == null)
                {
                    potion.playerCondition = GetComponent<PlayerCondition>();
                }

            }

            _itemData.item.Use();
        }
    }

    public void EquipNew(ItemData itemData) // 인벤토리에서 슬롯을 누를 때 동작하도록 해야 함 , [10/21] - InventoryUI - SelectSLot() 주석 처리함; 
    {
        UnEquip();
        _itemData = itemData;
        //SO 수정사항 - 장착하는 아이템 관련(애니메이션도 필요함 :( )
        if (_itemData == null)
        {
            return;
        }

        curEquip = Instantiate(_itemData.EquipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
            _itemData = null;
        }
    }

    public void SwapEquip(SlotData dragSlotData, SlotData dropSlotData, SlotBase selectSlot)
    {

        if (dragSlotData == selectSlot.curSlot)
        {
            Debug.Log("UnEquip Item");
            EquipNew(dragSlotData.itemData);
        }
        else if (dropSlotData.itemData == selectSlot.curSlot.itemData)
        {
            Debug.Log("Equip New Item");
            EquipNew(dropSlotData.itemData);
        }
        else
        {
            Debug.Log("UnEquip Item");
            UnEquip();
        }

    }
}
