using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestScene : MonoBehaviour
{
    private bool _initialized = false;
    // TODO: Scene에 따라 Scene Num 부여 예정.
    int TestSceneNum = 1;

    private void Start()
    {
        Initialize();
    }


    private bool Initialize()
    {
        if (_initialized) return false;

        Main.Scene.SceneNum = TestSceneNum;

        Object obj = FindObjectOfType<EventSystem>();
        // TODO ResourceManager에서 EventSystem 프리펩 추가예정.
        if (obj == null) Main.Resource.Instantiate("EventSystem.prefab").name = "@EventSystem";

        _initialized = true;
        return true;
    }
}
