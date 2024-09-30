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
            // 첫 번째 consumable의 값을 무조건 받음
            if (weaponItemData.Equipables.Length > 0)
            {
                Durability = weaponItemData.Equipables[0].DurabilityValue;
                //Debug.Log($"{weaponItemData.Equipables[0].type}, {weaponItemData.Equipables[0].DurabilityValue}");
            }
        }
    }

    public override void Use()
    {
        Debug.Log($"{itemData.DisplayName}으로 의 피해를 줍니다.");
        // 내구도 감소 로직 추가
        Durability--;
        //Debug.Log($"{itemData.DisplayName}의 내구도가 {Durability}로 감소했습니다.");
    }
}
