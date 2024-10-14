using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public float value;
    public PlayerCondition playerCondition;

    private void Awake() // 플레이어 컨디션 납치하자
    {
    }
    public Potion(ItemData data) : base(data)
    {
        if (data is PotionItemData potionData)
        {
            // 첫 번째 consumable의 값을 무조건 받음
            if (potionData.Consumables.Length > 0)
            {
                value = potionData.Consumables[0].ConsumableValue;
                //Debug.Log($"{potionData.Consumables[0].type}, {potionData.Consumables[0].ConsumableValue}");
            }
        }
    }
    public override void Use()
    {
        if (itemData is PotionItemData potionData)
        {
            // 타입에 따라 동작
            switch (potionData.Consumables[0].type)
            {
                case ConsumableType.Health:
                    playerCondition.UpdateHealth(value);
                    //Player.Instance.Heal(value); // 체력 회복
                    Debug.Log($"{itemData.DisplayName}을 사용하여 {value}만큼 체력을 회복합니다.");
                    break;
                case ConsumableType.Hunger:
                    playerCondition.UpdateHunger(value);
                    //Player.Instance.RecoverHunger(value); // 허기 회복 로직
                    Debug.Log($"{itemData.DisplayName}을 사용하여 {value}만큼 허기를 회복합니다.");
                    break;
                case ConsumableType.Thirsty:
                    playerCondition.UpdateThirst(value);
                    //Player.Instance.RecoverThirst(value); // 갈증 회복 로직
                    Debug.Log($"{itemData.DisplayName}을 사용하여 {value}만큼 갈증을 회복합니다.");
                    break;
                default:
                    Debug.LogWarning("알 수 없는 consumable type입니다.");
                    break;
            }
        }
    }
}
