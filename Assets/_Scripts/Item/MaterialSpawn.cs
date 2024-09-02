using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSpawn : MonoBehaviour
{
    #region Fields

    // 자원 프리팹
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject Stone;

    // 현재 스폰되어 있는 자원 수.
    [SerializeField] private int _treeCountNow = 0;
    [SerializeField] private int _stoneCountNow = 0;

    // 최소 유지 자원 수.
    [SerializeField] private int _treeKeepCount = 0;
    [SerializeField] private int _stoneKeepCount = 0;

    // 소환 한번에 생성되는 자원 수.
    private int _spawnAtOneTimeTree = 1;
    private int _spawnAtOneTimeStone = 1;

    // 좌표
    [SerializeField] private Vector3 _spawnPosTree;
    [SerializeField] private Vector3 _spawnPosStone;

    [SerializeField] float _spawnRadius = 15.0f; // 소환 좌표 간격.
    [SerializeField] float _spawnTime = 5.0f; // 소환 주기.
    #endregion


    private void Awake()
    {
        // 오브젝트 Pool에 생성.
        Main.Pool.Init();
        Main.Pool.CreatePool(Tree, _treeKeepCount);
        Main.Pool.CreatePool(Stone, _stoneKeepCount);
    }


}
