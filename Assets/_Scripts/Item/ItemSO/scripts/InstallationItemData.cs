using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstallationItemData", menuName = "Inventory System/Item Data/InstallationItem", order = 4)]
public class InstallationItemData : ItemData
{
    [Header("#MaxAmount")]
    public int MaxAmount = 1;

    [Header("Equip")]
    public GameObject EquipPrefab;

    [Header("PreView")]
    public GameObject preViewObject;
}
