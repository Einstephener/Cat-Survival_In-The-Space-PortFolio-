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

    private List<TMP_Text> _resourceTexts = new(); // 각 재료의 수량 텍스트를 저장.
    private List<int> _resourceOwnedAmounts = new();    // 각 재료의 현재 소유 개수 저장.

    public void ClickCraftBTN()
    {
        // 아이템 제작을 처리하는 메서드.
        if (_itemData.DropPrefab != null)
        {

            // 재료 차감.
            for (int i = 0; i < _itemData.CraftingResourceList.Length; i++)
            {
                Main.Inventory.RemoveItem(_itemData.CraftingResourceList[i].ResourceData, _itemData.CraftingResourceList[i].Amount);

                // 재료의 남은 수량 업데이트.
                _resourceOwnedAmounts[i] = Main.Inventory.GetTotalItemCount(_itemData.CraftingResourceList[i].ResourceData);
            }

            // 제작한 아이템 인벤에 넣어주기.
            Main.Inventory.AddItem(_itemData);

            // UI 업데이트.
            UpdateResourceTexts();
        }
    }

    public void CanCraft()
    {
        // 아이템을 제작할 수 있는지 확인하고, 버튼 활성화 여부 결정.
        bool canCraft = true;

        for (int i = 0; i < _itemData.CraftingResourceList.Length; i++)
        {
            if (_resourceOwnedAmounts[i] < _itemData.CraftingResourceList[i].Amount)
            {
                canCraft = false;
                break;
            }
        }

        CraftBTN.interactable = canCraft;
    }

    public void InitSetting(GameObject _craftItem)
    {
        // 아이템 데이터를 초기화하고 UI에 반영하는 메서드.
        if (_craftItem.TryGetComponent(out ItemObject _itemObject))
        {
            _itemData = _itemObject.itemData;
            CraftItemImage.sprite = _itemData.Icon;

            for (int i = 0; i < _itemData.CraftingResourceList.Length; i++)
            {
                // 재료 아이콘 및 수량 표시.
                GameObject resourceIcon = Instantiate(CraftItemResoursePrefab, CraftDescriptionParent.transform);
                resourceIcon.GetComponent<Image>().sprite = _itemData.CraftingResourceList[i].ResourceData.Icon;

                int ownedAmount = Main.Inventory.GetTotalItemCount(_itemData.CraftingResourceList[i].ResourceData);
                int needAmount = _itemData.CraftingResourceList[i].Amount;

                // 재료의 현재 소유 수량과 필요 수량 저장.
                _resourceOwnedAmounts.Add(ownedAmount);

                TMP_Text resourceText = resourceIcon.GetComponentInChildren<TMP_Text>();
                resourceText.text = ownedAmount + " / " + needAmount;
                _resourceTexts.Add(resourceText);
            }

            CanCraft();
        }
        else
        {
            Debug.LogError("ItemObject is missing.");
        }
    }

    private void UpdateResourceTexts()
    {
        // 남은 재료 수량을 다시 UI에 업데이트하는 메서드.
        for (int i = 0; i < _itemData.CraftingResourceList.Length; i++)
        {
            int needAmount = _itemData.CraftingResourceList[i].Amount;
            _resourceTexts[i].text = _resourceOwnedAmounts[i] + " / " + needAmount;
        }

        // 버튼 상태도 재확인.
        CanCraft();
    }
}
