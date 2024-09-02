using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MaterialSpawn : MonoBehaviour
{

    #region Fields

    // 스폰할 오브젝트들 list로 받아오기.
    public List<Poolable> SpawnMaterials = new List<Poolable>();

    #endregion


    private void Awake()
    {
        //// TODO: ResourceManager에서 소환으로 변경.
        //Main.Resource.Instantiate("Tree");
        //Main.Resource.Instantiate("Stone");

        // 오브젝트 Pool에 생성.
        foreach (var poolable in SpawnMaterials)
        {
            Main.Pool.Init();
            Main.Pool.CreatePool(poolable.Prefab, poolable._KeepCount);


        }
    }


    private void Update()
    {
        //foreach(var poolable in SpawnMaterials)
        //{
        //    while (CheckSpawnEachMaterials(poolable))
        //    {
        //        StartCoroutine(ReserveSpawn(poolable));
        //    }
        //}
        
    }

    private bool CheckSpawnEachMaterials(Poolable _pool)
    {
        if (_pool._CountNow < _pool._KeepCount)
            return true;
        else
            return false;
    }

    IEnumerator ReserveSpawn(Poolable _pool)
    {
        // Ensure that we do not exceed the spawn limit
        if (_pool._CountNow >= _pool._KeepCount)
        {
            yield break;
        }
        _pool._spawnAtOneTimeCount++;
        yield return new WaitForSeconds(Random.Range(0, _pool._spawnTime));

        //GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        Poolable spawnedObject = Main.Pool.Pop(_pool.Prefab);

        if (spawnedObject == null)
        {
            Debug.LogError("Failed to spawn object.");
            yield break;
        }
        Vector3 randPos;
        //NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _pool._spawnRadius);
            randDir.y = 0;


            randPos = _pool._spawnPos + randDir;

            //NavMeshPath path = new NavMeshPath();
            //if (nma.CalculatePath(randPos, path))
            break;
        }

        //obj.transform.position = randPos;
        spawnedObject.transform.position = randPos;

        _pool._CountNow++;
        _pool._spawnAtOneTimeCount--;
    }


    //TODO 오브젝트 디스폰 수정중.
    //public void OnObjectDespawn(Poolable poolable)
    //{
    //    var matchingPool = SpawnMaterials.Find(p => p.Prefab == poolable.Prefab);
    //    if (matchingPool != null)
    //    {
    //        matchingPool._CountNow--;
    //    }

    //    Main.Pool.Push(poolable);
    //}

}
