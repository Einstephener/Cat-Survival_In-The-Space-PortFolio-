using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetCanvas(gameObject, OrderValue._sceneOrder);

        return true;
    }

    [HideInInspector] public QuickSlot[] UI_QuickSlots;
    public TextMeshProUGUI interactionTXT;

    private void Awake()
    {
        QuickSlot[] QuickSlots = FindObjectsOfType<QuickSlot>();

        // slotNum 중 가장 큰 값을 찾아 배열 크기를 설정
        int maxSlotNum = 0;
        foreach (var slot in QuickSlots)
        {
            if (slot.slotNum > maxSlotNum)
                maxSlotNum = slot.slotNum;
        }

        // maxSlotNum + 1 크기로 UI_QuickSlots 배열 초기화
        UI_QuickSlots = new QuickSlot[maxSlotNum + 1];

        for (int i = 0; i < QuickSlots.Length; i++)
        {
            int a = QuickSlots[i].slotNum;

            // 범위 체크: a가 UI_QuickSlots 배열의 유효한 인덱스인지 확인
            if (a >= 0 && a < UI_QuickSlots.Length)
            {
                UI_QuickSlots[a] = QuickSlots[i];
            }
            else
            {
                Debug.LogWarning($"Invalid slot number {a} for QuickSlot at index {i}. Slot number out of bounds.");
            }
        }
    }
}
