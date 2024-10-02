using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NatureSpawn : MonoBehaviour
{
    #region Fields

    // 스폰할 오브젝트들 list로 받아오기.
    public List<Poolable> SpawnMaterials = new List<Poolable>();

    // 스폰 위치 .
    public List<Transform> _spawnPosition = new List<Transform>();

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
            _parents[i].transform.position = _spawnPosition[i].transform.position;
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
        Vector3 spawnCenter = _spawnPosition[index].position;

        Vector3 randDir = Random.insideUnitSphere * Random.Range(1, SpawnMaterials[index]._spawnRadius);
        randDir.y = 0; // 높이(Y)를 고정하여 평면 상에서 위치를 랜덤하게 설정

        //Vector3 randPos = SpawnMaterials[index]._spawnPos + randDir; 나중에 자원별로 정해져있는 스폰 포인트로 소환.
        // 랜덤 위치 아래의 지형이나 바닥을 감지하기 위해 Raycast 사용

        Vector3 randPos = spawnCenter + randDir;

        // 울퉁불퉁한 지형일 때, 오브젝트가 겹쳐질때를 위한 코드.
        //RaycastHit hit;
        //if (Physics.Raycast(randPos + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity)) // 위에서 50 유닛 떨어진 곳에서 Raycast 실행
        //{
        //    // 감지된 지형의 높이에 맞춰 y좌표를 설정
        //    randPos.y = hit.point.y;
        //}

        objTransform.position = randPos;
    }



    // 오브젝트 삭제는 다음 코드를 참고
    //public void OnReturnPrefab(Poolable poolable)
    //{
    //    poolable._CountNow--;
    //    Main.Pool.Push(poolable);
    //}

}
