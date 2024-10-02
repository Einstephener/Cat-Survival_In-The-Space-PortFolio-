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

    public void OnFire(InputValue value)
    {
        //공격 관련
    }

    public void EquipNew(ItemData item)
    {
        UnEquip();
        //SO 수정사항 - 장착하는 아이템 관련(애니메이션도 필요함 :( )
        curEquip = Instantiate(item.DropPrefab, equipParent).GetComponent<Equip>();
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
