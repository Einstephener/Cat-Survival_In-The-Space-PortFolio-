using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialSpawn : MonoBehaviour
{
    #region Fields

    // 스폰할 오브젝트들 list로 받아오기.
    public List<Poolable> SpawnMaterials = new List<Poolable>();

    // 스폰 위치 부모 transform.
    private List<Transform> _parents = new List<Transform>();

    #endregion


    // 코루틴 중복 실행 방지용 플래그
    private bool[] _isSpawning;

    private void Awake()
    {

        // 오브젝트 Pool에 생성.
        Main.Pool.Init();

        // 각 프리팹별로 코루틴 중복 실행 방지 플래그 초기화
        _isSpawning = new bool[SpawnMaterials.Count];

        // Create
        for (int i = 0; i < SpawnMaterials.Count; i++)
        {
            Main.Pool.CreatePool(SpawnMaterials[i].Prefab, SpawnMaterials[i]._KeepCount);
            SpawnMaterials[i]._CountNow = 0;
            //프리팹 별로 부모 설정.
            _parents.Add(new GameObject { name = SpawnMaterials[i].Prefab.name + "_Root" }.transform);
            _parents[i].parent = transform;
        }
    }

    private void Update()
    {
        for (int i = 0; i < SpawnMaterials.Count; i++)
        {
            if (CheckSpawnEachMaterials(SpawnMaterials[i]) && !_isSpawning[i])
            {
                StartCoroutine(RespawnMaterial(i));
            }
        }
    }

    private bool CheckSpawnEachMaterials(Poolable _pool)
    {
        return _pool._CountNow < _pool._KeepCount;
    }

    IEnumerator RespawnMaterial(int i)
    {
        _isSpawning[i] = true; // 코루틴 시작 시 플래그 설정

        yield return new WaitForSeconds(SpawnMaterials[i]._spawnTime);

        Poolable spawnedObject = Main.Pool.Pop(SpawnMaterials[i].Prefab, _parents[i]);

        SetRandomPosition(spawnedObject.transform, i);

        SpawnMaterials[i]._CountNow++;

        _isSpawning[i] = false; // 코루틴 종료 시 플래그 해제
    }

    private void SetRandomPosition(Transform objTransform, int index)
    {
        Vector3 randDir = Random.insideUnitSphere * Random.Range(1, SpawnMaterials[index]._spawnRadius);
        randDir.y = 0; // 높이(Y)를 고정하여 평면 상에서 위치를 랜덤하게 설정
        Vector3 randPos = SpawnMaterials[index]._spawnPos + randDir;
        objTransform.position = randPos;
    }



    // 오브젝트 삭제는 다음 코드를 참고
    //public void OnReturnPrefab(Poolable poolable)
    //{
    //    poolable._CountNow--;
    //    Main.Pool.Push(poolable);
    //}

}
