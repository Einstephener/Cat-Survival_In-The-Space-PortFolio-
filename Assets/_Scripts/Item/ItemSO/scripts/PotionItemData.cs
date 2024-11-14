using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PotionItemData", menuName = "Inventory System/Item Data/Portion", order = 3)]
public class PotionItemData : ContableItemData
{

    [Header("#Consumable")]
    public ItemDataConsumable[] Consumables;

    [Header("#Cooking")]
    public ItemData CookingItemData;
}
