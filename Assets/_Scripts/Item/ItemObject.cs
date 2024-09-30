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
        //switch (itemData)
        //{
        //    case PotionItemData potionData:
        //        item = new Potion(itemData);
        //        break;
        //    case ContableItemData contableItemData:
        //        item = new Resource(itemData);
        //        break;
        //    case WeaponItemData weaponData:
        //        item = new Weapon(itemData);
        //        break;
        //    case InstallationItemData InstallationData:
        //        item = new Weapon(itemData);
        //        break;
        //    default:
        //        Debug.LogWarning("알 수 없는 아이템 타입입니다.");
        //        break;
        //}
    }
    public string GetInteractPrompt()
    {
        return string.Format("<interaction> {0}", itemData.DisplayName);
    }

    public void OnInteract()
    {
        //상호작용할 아이템이 설치가능한 아이템인지 확인 후 상호작용하기
        if (itemData.Type != ItemType.Installation)
        {
            Main.Inventory.AddItem(itemData);
        }
        else
        {
            Use();
        }
    }

    public void Use()
    {
        item.Use();
    }
}
