using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour
{
    public Image CraftItemImage;
    public GameObject CraftDescriptionParent;
    public GameObject CraftItemResoursePrefab;
    public Button CraftBTN;
    private ItemData _itemData;

    private Image ResourceItemImage;
    private TMP_Text ResourceItemAmount;
    private int _itemOwnedAmount = 0; //todo
    private int _itemNeedAmount;


    public void ClickCraftBTN()
    {

    }

    public void CanCraft()
    {
        if(_itemOwnedAmount < _itemNeedAmount)
        {
            // 제작 불가
            CraftBTN.interactable = false;
        }
        else
        {
            //제작 가능
            CraftBTN.interactable = true;
        }
    }

    public void InitSetting(GameObject _craftItem)
    {
        if (_craftItem.TryGetComponent(out ItemObject _itemObject))
        {
            _itemData = _itemObject.itemData;
            CraftItemImage.sprite = _itemData.Icon;
            for(int i = 0; i < _itemData.CraftingResourceList.Length; i++)
            {
                GameObject ResourceIcon = Instantiate(CraftItemResoursePrefab, CraftDescriptionParent.transform);

                ResourceIcon.GetComponent<Image>().sprite = _itemData.CraftingResourceList[i].ResourceData.Icon;

                _itemOwnedAmount = Main.Inventory.GetTotalItemCount(_itemData.CraftingResourceList[i].ResourceData);
                _itemNeedAmount = _itemData.CraftingResourceList[i].Amount;
                CanCraft();

                ResourceIcon.GetComponentInChildren<TMP_Text>().text = _itemOwnedAmount + " / " + _itemNeedAmount;


            }
        }
        else
        {
            Debug.LogError("ItemObject");
        }
    }


}
