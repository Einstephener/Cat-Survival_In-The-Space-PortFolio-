using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Installation
{
    public SlotData[] boxSlotsData;
    public Box(ItemData data) : base(data)
    {

    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        boxSlotsData = new SlotData[12];
        //box와 인벤토리 연결하는 작업 할 꺼임
        
        for (int i = 0; i < boxSlotsData.Length; i++)
        {
            if (boxSlotsData[i] == null)
            {
                boxSlotsData[i] = new SlotData();
            }
        }

        //for (int i = 0; i < boxSlotsData.Length; i++)
        //{
        //    Main.Inventory.inventoryUI.BoxSlotsGet(boxSlotsData);
        //}

        //Main.Inventory.inventoryUI.BoxSlotUpdateUI();
    }

    public override void Use()
    {
        //Debug.Log("설치 아이템 상호작용");
    }

    public override void UISet()
    {
        //Main.Inventory.inventoryUI.gameObject.SetActive(true);
        Main.Inventory.inventoryUI.boneFireObject.SetActive(false);
        Main.Inventory.inventoryUI.boxSlotsObject.SetActive(true);

        base.UISet();
    }

    public override void UIInterac()
    {
        base.UIInterac();
        Main.Inventory.inventoryUI.BoxSlotsGet(boxSlotsData);

        UISet();
    }
}
