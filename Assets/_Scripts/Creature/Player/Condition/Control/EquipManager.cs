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

    private PlayerInputController controller;
    private PlayerCondition condition;


    private void Start()
    {
        //ItemDataSO 에 장착 아이템 추가 ㄲ
        //curEquip = Main.Inventory.inventoryUI.selectSlot.curSlot.itemData.EquipPrefab.GetComponent<Equip>();
    }
    public void OnFire(InputValue value)
    {
        //공격 관련
    }

    public void EquipNew(ItemData item) // 인벤토리에서 슬롯을 누를 때 동작하도록 해야 함
    {
        UnEquip();
        //SO 수정사항 - 장착하는 아이템 관련(애니메이션도 필요함 :( )
        curEquip = Instantiate(item.EquipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
