using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContableItemData", menuName = "Inventory System/Item Data/ContableItem", order = 3)]
public class ContableItemData : ItemData
{
    [Header("#MaxAmount")]
    public int MaxAmount = 99;

    //[Header("Equip")]
    //public GameObject EquipPrefab;

}
