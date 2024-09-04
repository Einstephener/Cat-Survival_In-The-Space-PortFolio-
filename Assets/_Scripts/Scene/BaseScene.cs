using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    private bool _initialized = false;

    private void Start()
    {
        Initialize();
    }


    protected virtual bool Initialize()
    {
        if (_initialized) return false;

        Main.Scene.CurrentScene = this;

        Object obj = FindObjectOfType<EventSystem>();
        // TODO ResourceManager에서 EventSystem 프리펩 추가예정.
        //if (obj == null) Main.Resource.Instantiate("EventSystem.prefab").name = "@EventSystem";


        _initialized = true;
        return true;
    }
}
