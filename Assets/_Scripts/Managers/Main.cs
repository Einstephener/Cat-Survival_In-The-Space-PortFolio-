using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    #region Singleton
    // �̱���

    private static Main _instance;
    private static bool _initialized;
    //�ʱ�ȭ ����.

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
                    obj = new() { name = "@Main" };     //@Main�̶�� ������Ʈ ����.
                    obj.AddComponent<Main>();
                    DontDestroyOnLoad(obj);     // �� �Ѿ�� �ı� �ȵǵ���.
                    _instance = obj.GetComponent<Main>();   // �ν��Ͻ�ȭ.
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
    private SceneManager _scene = new();
    private ObjectManager _object = new();

    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static DataManager Data => Instance?._data;
    public static UIManager UI => Instance?._ui;
    public static SceneManager Scene => Instance?._scene;
    public static ObjectManager Object => Instance?._object;




}
