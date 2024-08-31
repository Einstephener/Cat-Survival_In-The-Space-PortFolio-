using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;

    public string GetInteractPrompt()
    {
        return string.Format("<interaction> {0}", item.DisplayName);
    }

    public void OnInteract()
    {
        //인벤에 아이템 추가 
        Debug.Log($"Eat Item {item.DisplayName}");
    }
}
