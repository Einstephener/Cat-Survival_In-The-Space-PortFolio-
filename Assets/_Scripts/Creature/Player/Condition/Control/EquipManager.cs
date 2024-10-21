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
        if (_itemData.Type == ItemType.Consumable)
        {
            curEquip.OnEating();
            _itemData.item.Use();
        }
    }

    public void EquipNew(ItemData itemData) // 인벤토리에서 슬롯을 누를 때 동작하도록 해야 함
    {
        UnEquip();
        _itemData = itemData;
        //SO 수정사항 - 장착하는 아이템 관련(애니메이션도 필요함 :( )
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
}
