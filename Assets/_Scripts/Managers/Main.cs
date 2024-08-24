using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
