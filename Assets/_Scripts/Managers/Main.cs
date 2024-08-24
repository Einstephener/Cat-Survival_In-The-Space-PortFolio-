using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    #region Singleton
    // 싱글톤

    private static Main _instance;
    private static bool _initialized;
    //초기화 여부.

    public static Main Instance
    {
        get
        {
            if (!_initialized)
            {
                _initialized = true;

                GameObject obj = GameObject.Find("@Main");
                if (obj == null)
                {
                    obj = new() { name = "@Main" };     //@Main이라는 오브젝트 생성.
                    obj.AddComponent<Main>();
                    DontDestroyOnLoad(obj);     // 씬 넘어가도 파괴 안되도록.
                    _instance = obj.GetComponent<Main>();   // 인스턴스화.
                }
            }
            return _instance;
        }
    }

    #endregion

}
