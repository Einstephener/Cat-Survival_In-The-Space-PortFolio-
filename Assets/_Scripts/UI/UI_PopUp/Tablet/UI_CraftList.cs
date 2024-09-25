using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour
{
    public Image CraftItemImage;
    public GameObject CraftItemDescription;
    public Button CraftBTN;
    private ItemData _itemData;

    private void Awake()
    {
        
    }


    public void ClickCraftBTN()
    {

    }

    public void InitSetting(GameObject _craftItem)
    {
        if (_craftItem.TryGetComponent(out ItemObject _itemObject))
        {
            _itemData = _itemObject.itemData;
            CraftItemImage.sprite = _itemData.Icon;
        }
        else
        {
            Debug.LogError("ItemObject");
        }
    }


}
