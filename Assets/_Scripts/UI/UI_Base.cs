using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    private bool _initialized;
    protected virtual void OnEnable()
    {
        Initialize();
    }

    public virtual bool Initialize()
    {
        if (_initialized) return false;

        _initialized = true;
        return true;
    }

    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }


    #region UI Binding 코드

    // 유니티 씬 상에 존재하는 오브젝트들을 로드하여 이 곳에 바인딩하여 보관
    //protected Dictionary<Type, UnityEngine.Object[]> _objects = new();

    // 바인딩 코드.
    //private void Bind<T>(Type type) where T : UnityEngine.Object
    //{
    //    string[] names = Enum.GetNames(type);
    //    UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

    //    //for (int i = 0; i < names.Length; i++)
    //    //    objects[i] = typeof(T) == typeof(GameObject) ? this.gameObject.FindChild(names[i]) : this.gameObject.FindChild<T>(names[i]);

    //    _objects.Add(typeof(T), objects);
    //}
    //protected void BindObject(Type type) => Bind<GameObject>(type);
    //protected void BindText(Type type) => Bind<TextMeshProUGUI>(type);
    //protected void BindButton(Type type) => Bind<Button>(type);
    //protected void BindImage(Type type) => Bind<Image>(type);
    //protected void BindSlider(Type type) => Bind<Slider>(type);

    //private T Get<T>(int index) where T : UnityEngine.Object
    //{
    //    if (!_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objs)) return null;
    //    return objs[index] as T;
    //}
    //protected GameObject GetObject(int index) => Get<GameObject>(index);
    //protected TextMeshProUGUI GetText(int index) => Get<TextMeshProUGUI>(index);
    //protected Button GetButton(int index) => Get<Button>(index);
    //protected Image GetImage(int index) => Get<Image>(index);
    //protected Slider GetSlider(int index) => Get<Slider>(index);

    #endregion
}