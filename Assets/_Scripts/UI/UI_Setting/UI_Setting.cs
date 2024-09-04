using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetCanvas(gameObject, OrderValue._settingOrder);
        return true;
    }
}
