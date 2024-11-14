using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScene : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;



        return true;
    }

    public void MoveNextScene()
    {
        Main.Scene.LoadScene("MainScene");
    }

}
