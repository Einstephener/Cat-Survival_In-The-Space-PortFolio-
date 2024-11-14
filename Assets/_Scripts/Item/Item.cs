using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    public ItemData itemData;

    public Item(ItemData data)
    {
        itemData = data;
    }

    public abstract void Use();
}
