using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public Item item;

    private void Start()
    {
        switch (itemData)
        {
            case PotionItemData potionData:
                item = new Potion(itemData);
                break;
            case ContableItemData contableItemData:
                item = new Resource(itemData);
                break;
            case WeaponItemData weaponData:
                item = new Weapon(itemData);
                break;
            default:
                Debug.LogWarning("알 수 없는 아이템 타입입니다.");
                break;
        }
    }
    public string GetInteractPrompt()
    {
        return string.Format("<interaction> {0}", itemData.DisplayName);
    }

    public void OnInteract()
    {
        //인벤에 아이템 추가 
        Main.Inventory.AddItem(itemData);
        //Destroy(this.gameObject);

        //Debug.Log($"Add Inventory AddItem() function to OnInteract() ");
        //Debug.Log($"Eat Item - ItemName : {itemData.DisplayName}");
    }

    public void Use()
    {
        item.Use();
    }
}
