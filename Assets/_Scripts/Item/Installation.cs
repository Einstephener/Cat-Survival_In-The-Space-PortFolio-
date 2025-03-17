using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 프리뷰
/// </summary>
public class Installation : Item
{
    public bool isInstallation = false;
    public Installation(ItemData data) : base(data)
    {

    }

    public override void Use()
    {
        //Debug.Log("설치 아이템 상호작용");
    }

    public virtual void UISet()
    {
        Main.UI.ShowPopupUI<InventoryUI>("Inventory");
        Main.Inventory.inventoryUI.AdjustParentHeight();

        Main.UI.SwitchToUI();
    }

    public virtual void UIInteract()
    {
        //Debug.Log("설치 아이템 UI 상호작용");
    }

    public virtual void PreView() //프리뷰 생성
    {
        //프리뷰
        InstallationItemData installationData = itemData as InstallationItemData;
        if (installationData != null)
        {
            GameObject preview = installationData.preViewObject;
        }
        else
        {
            Debug.LogWarning("itemData가 InstallationData가 아닙니다.");
        }
    }


    //아이템이 회수 할 때 사용
    public virtual void RemoveObject()
    {

        Debug.Log("회수하다.");

        Main.Inventory.AddItem(this.itemData);
        Destroy(this.gameObject);
    }

    protected void RetrieveSlotItemData(SlotData _slotaData)
    {
        if(!_slotaData.IsEmpty())
        {
            Main.Inventory.AddItem(_slotaData.itemData, _slotaData.amount);
        }
    }
}
