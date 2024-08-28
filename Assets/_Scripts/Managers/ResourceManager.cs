using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Pathfinding.AdvancedSmooth;
using Object = UnityEngine.Object;
//using UnityEngine.AddressableAssets;

public class ResourceManager
{
    /// <summary>
    /// Type - 리소스 타입 구분하기 위해 사용함(GameObjec,TextAsset, 등)
    /// </summary>
    private Dictionary<Type, Dictionary<string, Object>> _resources = new();

    private bool _isInitialized;

    // 초기화
    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }
        _isInitialized = true;

        // 임시파일 이름 - 아직 추가 안함. 파일 추가 되면 수정 될 예정
        LoadResource<GameObject>("Prefabs");
        LoadResource<TextAsset>("JsonData");
        LoadResource<AudioClip>("Sounds");
    }

    private string GetResourceName<T>(T resource) where T : Object
    {
        return resource.name;
    }

    // 리소스를 비동기적으로 로드하기 - Initialize에 사용하여 특정 파일에 있는 Object를 전부 생성 Hierchy에 생성 되도록 함
    private void LoadResource<T>(string path, Func<T, string> keyFinder = null) where T : Object
    {
        if (keyFinder == null)
        {
            keyFinder = GetResourceName;
        }
        var loadedResources = Resources.LoadAll<T>(path);
        var resourceDictionary = new Dictionary<string, Object>();

        foreach (var resource in loadedResources)
        {
            string key = keyFinder(resource);
            resourceDictionary[key] = (Object)resource;
        }

        _resources[typeof(T)] = resourceDictionary;
    }

    // 리소스 있는지 확인 
    public bool IsExist<T>(string key) where T : Object
    {
        //해당 타입의 리소스 딕셔너리 조회하기
        if (!_resources.TryGetValue(typeof(T), out Dictionary<string, Object> dictionary))
        {
            return false;
        }
        return dictionary.ContainsKey(key);
    }
    // 특정 리소스 가져오기
    public T Get<T>(string key) where T : Object
    {
        // 해당 타입의 리소스 딕셔너리 조회하기
        if (!_resources.TryGetValue(typeof(T), out Dictionary<string, Object> dictionary))
        {
            Debug.LogError($"[ResourceManager] Get<{typeof(T)}>({key})");
            return null;
        }
        // 키가 존재하는지 확인하기
        if (!dictionary.TryGetValue(key, out Object resource))
        {
            Debug.LogError($"[ResourceManager] Get<{typeof(T)}>({key})");
            return null;
        }
        return resource as T;
    }
    // 특정 타입 리소스 조회 후 리스트에 추가
    public List<T> GetAll<T>() where T : Object
    {
        // 해당 타입의 리소스 딕셔너리 조회하기
        if (!_resources.TryGetValue(typeof(T), out Dictionary<string, Object> dictionary))
        {
            Debug.LogError($"[ResourceManager] GetAll<{typeof(T)}>()");
            return null;
        }

        var resourceList = new List<T>();
        //딕셔너리의 모든 값을 리스트에 추가하기
        foreach (var value in dictionary.Values)
        {
            resourceList.Add(value as T);
        }
        return resourceList;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Get<GameObject>(key);
        //프리팹
        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] Instantiate({key}): Failed to load prefab.");
            return null;
        }

        bool pooling = prefab.GetComponent<Poolable>().IsUsing;

        if (pooling)
        {
            return Main.Pool.Pop(prefab).GetComponent<GameObject>();
        }
        //풀 매니저

        GameObject obj = UnityEngine.Object.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }


}
