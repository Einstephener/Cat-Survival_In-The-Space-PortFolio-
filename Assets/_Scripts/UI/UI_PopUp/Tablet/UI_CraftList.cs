using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour
{
    public Image CraftItemImage;
    public GameObject CraftItemDescriptionParent;
    public TMP_Text CraftItemListTxt;
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
            for(int i = 0; i < _itemData.CraftingResourceList.Length; i++)
            {
                CraftItemListTxt.text += _itemData.CraftingResourceList[i].ResourceType.ToString() + " " + _itemData.CraftingResourceList[i].Amount + " \n";
            }
        }
        else
        {
            Debug.LogError("ItemObject");
        }
    }


}
