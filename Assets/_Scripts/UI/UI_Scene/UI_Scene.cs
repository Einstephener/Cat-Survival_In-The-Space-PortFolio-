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

    //[HideInInspector] public QuickSlot[] UI_QuickSlots;
    //public TextMeshProUGUI interactionTXT;

    //private void Awake()
    //{
    //    QuickSlot[] quickSlotComponents = GetComponentsInChildren<QuickSlot>();

    //    for (int i = 0; i < UI_QuickSlots.Length; i++)
    //    {
    //        UI_QuickSlots[i] = quickSlotComponents[i];
    //    }
    //}

}
