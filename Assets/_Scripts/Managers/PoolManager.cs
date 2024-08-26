using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PoolManager
{
    #region Pool
    private class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; } // 풀링 할 오브젝트별 정리용 루트.

        private Stack<Poolable> _poolStack = new();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++) Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate(Original); // 원본을 복사하여 루트 생성.
            go.name = Original.name;
            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable == null)
                poolable = go.AddComponent<Poolable>();
            return poolable;
        }

        // 만들어진 오브젝트를 종류별로 넣어주기.
        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        // 사용 할 오브젝트 내보내기.
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0) 
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    private Dictionary<string, Pool> _pool = new();
    private Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform; // 풀링 할 오브젝트들의 루트.
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new();
        pool.Init(original, count);
        pool.Root.parent = _root; // 풀링 된 오브젝트들의 루트인 @Pool_Root에 연결.

        _pool.Add(original.name, pool);
    }

    // 사용하지 않는 오브젝트 반환.
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        // 이미 만들어졌던 오브젝트인지 확인.
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    // 오브젝트 삭제.
    // 씬과 씬에서 넘어가는데 다른 구역으로 넘어가서 오브젝트들 구성이 크게 다르다 등
    public void Clear()
    {
        foreach(Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
