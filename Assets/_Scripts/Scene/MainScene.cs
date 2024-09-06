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
        Main.Resource.Instantiate("UI_MainScene");



        return true;
    }
}
