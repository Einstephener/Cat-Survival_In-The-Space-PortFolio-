using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Inventory System/Item Data/WeaponItem", order = 3)]
public class WeaponItemData : ItemData
{
    [Header("Equip")]
    public GameObject EquipPrefab;

    [Header("Equipable")]
    public ItemDataEquipable[] Equipables;
}
