using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private PoolManager _pool = new();
    private ResourceManager _resource = new();
    private DataManager _data = new();
    private UIManager _ui = new();
    private SceneManagerEOM _scene;
    private ObjectManager _object;
    private InventoryManager _inventory = new();

    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static DataManager Data => Instance?._data;
    public static UIManager UI => Instance?._ui;
    public static SceneManagerEOM Scene => Instance?._scene;
    public static ObjectManager Object => Instance?._object;
    public static InventoryManager Inventory => Instance?._inventory;


    private void Awake()
    {
        // SceneManagerEOM과 ObjectManager는 MonoBehaviour이므로 AddComponent로 추가
        _scene = gameObject.AddComponent<SceneManagerEOM>();
        _object = gameObject.AddComponent<ObjectManager>();
    }


    private static void Initialize()
    {
        Resource.Initialize();
        Data.Initialize();
    }
}