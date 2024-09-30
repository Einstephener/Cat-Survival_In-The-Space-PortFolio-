using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestScene : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.ShowSceneUI<UI_Scene>("UI_MainSceneUI");


        return true;
    }
}
