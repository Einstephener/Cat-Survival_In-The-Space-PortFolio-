using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

[System.Serializable]
public enum ConsumableType
{
    Hunger,
    Health,
    Thirsty
}
[System.Serializable]
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

[System.Serializable]
public class CraftingResource
{
    public ItemData ResourceData;
    public int Amount;
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
    public Item item;

    [Header("Crafting")]
    public CraftingResource[] CraftingResourceList;

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
