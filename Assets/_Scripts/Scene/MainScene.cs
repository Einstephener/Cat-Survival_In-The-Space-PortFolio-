using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    #region Fields
    #endregion

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Resource.Initialize();

        // Simplify showing and closing popup UIs with a helper method
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
