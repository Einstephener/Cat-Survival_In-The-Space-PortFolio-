using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainScene : BaseScene
{
    #region Fields

    public InputActionAsset inputActionAsset;

    #endregion

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Resource.Initialize();

        Main.UI.inputActionAsset = inputActionAsset;

        Main.UI.ShowSceneUI<UI_Scene>("UI_MainScene");
        InitPopupUI<InventoryUI>("Inventory");
        InitPopupUI<UI_Map>("UI_Map");
        InitPopupUI<UI_Tablet>("UI_CraftingTabletUI");

        return true;
    }

    private void InitPopupUI<T>(string uiName) where T : UI_Popup
    {
        Main.UI.ShowPopupUI<T>(uiName);
        if (Main.UI._uiPopUpDictionary.TryGetValue(uiName, out GameObject popup))
        {
            Main.UI.ClosePopupUI(popup);
        }
    }
}
