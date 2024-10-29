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
            Destroy(this.gameObject);
        }
        else // 임시입니다.
        {
            //Installation initem = item as Installation;

            //BoneFire boneFire = item as BoneFire;

            //Tent tent = item as Tent;

            if (item is Installation installation)
            {
                // Installation 관련 작업 수행
                installation.UIInterac();
                //Debug.Log("ItemObject.cs - OnInteract() : Installation.cs Insert");

            }
            // item이 BoneFire 타입인지 확인
            //else if (item is BoneFire boneFire)
            //{
            //    // BoneFire 관련 작업 수행
            //    boneFire.UIInterac();
            //    Debug.Log("ItemObject.cs - OnInteract() : BoneFire.cs Insert");
            //}
            //// item이 Tent 타입인지 확인
            //else if (item is Tent tent)
            //{
            //    // Tent 관련 작업 수행
            //    tent.UIInterac();
            //    Debug.Log("ItemObject.cs - OnInteract() : Tent.cs Insert");
            //}
            //else
            //{
            //    Debug.Log("ItemObject.cs - OnInteract() Error: 알 수 없는 아이템 타입입니다.");
            //}
            //initem.UIInterac();
            //boneFire.UIInterac();
            //Debug.Log("ItemObject.cs - OnIntercat() Errer");
            Use();
        }
    }

    public void Use()
    {
        item.Use();
    }
}
