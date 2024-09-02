using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health,
    Thirsty
}

public enum EquipableType
{
    Ax,
    Pick,
    Weapon
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float ConsumableValue;
}

[System.Serializable]
public class ItemDataEquipable
{
    public EquipableType type;
    public int DurabilityValue;
}

//[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string DisplayName;
    public string Description;
    public ItemType Type;
    public Sprite Icon;
    public GameObject DropPrefab;

    //[Header("Stacking")]
    //public bool CanStack;
    //public int MaxStackAmount;

    //[Header("Consumable")]
    //public ItemDataConsumable[] Consumables;

    //[Header("Equip")]
    //public GameObject EquipPrefab;

    //[Header("Equipable")]
    //public ItemDataEquipable[] Equipables;
}
