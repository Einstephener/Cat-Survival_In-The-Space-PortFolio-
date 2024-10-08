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
                item = GetComponent<Potion>();
                //item = gameObject.AddComponent<Potion>();

                break;
            case ContableItemData contableItemData:
                item = new Resource(itemData);
                item = GetComponent<Resource>();

                break;
            case WeaponItemData weaponData:
                item = new Weapon(itemData);
                item = GetComponent<Weapon>();

                break;
            case InstallationItemData InstallationData:
                item = new Installation(itemData);
                item = GetComponent<Installation>();

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
        //추후에 Layer 따라 작동하도록 해보자.
        //상호작용할 아이템이 설치가능한 아이템인지 확인 후 상호작용하기
        if (itemData.Type != ItemType.Installation)
        {
            Main.Inventory.AddItem(itemData);
        }
        else
        {
            Installation initem = item as Installation;
            BoneFire Test = item as BoneFire;
            //initem.UIInterac();
            Debug.Log("ItemObject.cs - OnIntercat() Errer");
            Test.UIInterac();
            //Use();
        }
    }

    public void Use()
    {
        item.Use();
    }
}
