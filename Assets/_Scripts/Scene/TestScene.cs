using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestScene : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;


        //Scene에 필요한 오브젝트들 소환.
        Main.Resource.Initialize();
        //Main.Resource.Instantiate("UI_MainScene");

        return true;
    }
}
