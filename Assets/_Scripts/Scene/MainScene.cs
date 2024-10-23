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

        Main.Resource.Instantiate("@EventSystem");
        Main.UI.ShowSceneUI<UI_MainScene>("UI_MainScene");
        Main.UI.ShowSceneUI<UI_Damaged>("UI_Hit");
        Main.UI.ShowSceneUI<UI_Respawn>("UI_Respawn");

        InitPopupUI<InventoryUI>("Inventory");
        InitPopupUI<UI_Map>("UI_Map");
        InitPopupUI<UI_Tablet>("UI_CraftingTabletUI");

        //Main.UI.ShowSettingPopupUI<UI_Setting>("Setting");
        //Main.UI.CloseSetting(_alreayOpenSetting);

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
