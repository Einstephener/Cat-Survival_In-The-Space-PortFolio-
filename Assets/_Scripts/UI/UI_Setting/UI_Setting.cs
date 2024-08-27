using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting : UI_Base
{
    public override void Init()
    {
        Main.UI.SetCanvas(gameObject, OrderValue._settingOrder);
    }
}
