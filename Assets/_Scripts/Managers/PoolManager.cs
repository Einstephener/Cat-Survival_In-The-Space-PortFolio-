using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    private class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; } // Ǯ�� �� ������Ʈ�� ������ ��Ʈ.

        private Stack<Poolable> _poolStack = new();

        private void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++) Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate(Original); // ������ �����Ͽ� ��Ʈ ����.
            go.name = Original.name;
            //return go.GetOrAddComponent<Poolable>();
            return null;
        }

        //public void GetOrAddComponent<Poolable>(GameObject go)
        //{
        //    Poolable poolable = go.GetComponent<Poolable>();
        //    if (poolable == null)
        //        poolable = go.AddComponent<T>();
        //    return poolable;
        //}

        // ������� ������Ʈ�� �������� �־��ֱ�.
        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        // ��� �� ������Ʈ ��������.
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
            _root = new GameObject { name = "@Pool_Root" }.transform; // Ǯ�� �� ������Ʈ���� ��Ʈ.
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Push(Poolable poolable)
    {

    }

    public Poolable Pop(GameObject original, GameObject parent = null)
    {
        return null;
    }

    public GameObject GetOriginal(string name)
    {
        return null;
    }
}
