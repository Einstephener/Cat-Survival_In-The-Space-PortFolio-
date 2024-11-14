using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyScene : BaseScene
{
    //public GameObject Canvas;
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //Scene에 필요한 오브젝트들 소환.
        Main.Resource.Initialize();

        //오류 방지용 UI초기화
        Main.UI.ClearDictionary();

        Main.Resource.Instantiate("UI_LobbyScene" );

        return true;
    }

}
