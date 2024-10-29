using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public float value;
    public PlayerCondition playerCondition;

    //private void Awake() // 플레이어 컨디션 납치하자
    //{
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player != null)
    //    {
    //        playerCondition = player.GetComponent<PlayerCondition>();
    //        if (playerCondition != null)
    //        {
    //            // PlayerCondition을 사용할 수 있음
    //            Debug.Log("PlayerCondition found!");
    //        }
    //        else
    //        {
    //            Debug.Log("PlayerCondition not found on player.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Player not found with tag.");
    //    }
    //}

    //private void Start()
    //{
       
    //}

    private void Initialize()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
        {
            // A 스크립트를 가져오기
            playerCondition = Player.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                // A 스크립트의 메서드 호출
                Debug.Log("연결함");
            }
            else
            {
            }
        }
        else
        {
        }
    }

    public Potion(ItemData data) : base(data)
    {
        if (data is PotionItemData potionData)
        {
            // 첫 번째 consumable의 값을 무조건 받음
            if (potionData.Consumables.Length > 0)
            {
                value = potionData.Consumables[0].ConsumableValue;
                //Debug.Log(value);
                //Debug.Log($"{potionData.Consumables[0].type}, {potionData.Consumables[0].ConsumableValue}");
            }
        }
    }
    public override void Use()
    {

        Initialize();

        if (itemData is PotionItemData potionData)
        {
            // Consumables 배열의 모든 항목에 대해 반복
            foreach (var consumable in potionData.Consumables)
            {
                // 타입에 따라 동작
                switch (consumable.type)
                {
                    case ConsumableType.Health:
                        playerCondition.UpdateHealth(consumable.ConsumableValue);
                        Debug.Log($"{itemData.DisplayName}을 사용하여 {consumable.ConsumableValue}만큼 체력을 회복합니다.");
                        break;
                    case ConsumableType.Hunger:
                        playerCondition.UpdateHunger(consumable.ConsumableValue);
                        Debug.Log($"{itemData.DisplayName}을 사용하여 {consumable.ConsumableValue}만큼 허기를 회복합니다.");   
                        break;
                    case ConsumableType.Thirsty:
                        playerCondition.UpdateThirst(consumable.ConsumableValue);
                        Debug.Log($"{itemData.DisplayName}을 사용하여 {consumable.ConsumableValue}만큼 갈증을 회복합니다.");
                        break;
                    default:
                        Debug.LogWarning("알 수 없는 consumable type입니다.");
                        break;
                }
            }

            // 인벤토리 업데이트
            //Main.Inventory.RemoveItem(Main.Inventory.inventoryUI.selectSlot.curSlot.itemData);
            //Main.Inventory.inventoryUI.selectSlot.curSlot.amount--;
            Main.Inventory.Select_RemoveItem(Main.Inventory.inventoryUI.selectSlot.index);

            //Main.Inventory.inventoryUI.UpdateUI();
        }
        else
        {
            if (itemData == null)
            {
                Debug.Log("itemData가 null입니다.");
            }
            Debug.Log($"Current itemData type: {itemData}");
        }
    }
}
