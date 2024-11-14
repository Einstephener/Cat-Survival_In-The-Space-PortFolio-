using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Weapon : Item
{
    public int Durability;

    public Weapon(ItemData data) : base(data)
    {
        if (data is WeaponItemData weaponItemData)
        {
            Durability = weaponItemData.WeaponDatas.DurabilityValue;
        }
    }

    public override void Use()
    {
        Debug.Log($"{itemData.DisplayName}으로 의 피해를 줍니다.");
        // 내구도 감소 로직 추가
        //Durability--;
        //Debug.Log($"{itemData.DisplayName}의 내구도가 {Durability}로 감소했습니다.");
    }
}
