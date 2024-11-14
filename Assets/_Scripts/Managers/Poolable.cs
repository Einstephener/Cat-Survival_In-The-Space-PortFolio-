using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    // 오브젝트 풀링 대상에 붙이기.

    #region Field

    public bool IsUsing;

    public GameObject Prefab;

    // 현재 스폰되어 있는 자원 수.
    public int _CountNow = 0;

    // 최소 유지 자원 수.
    public int _KeepCount = 0;

    // 소환 한번에 생성되는 자원 수.
    //public int _spawnAtOneTimeCount = 1;

    // 좌표
    public Vector3 _spawnPos;

    public float _spawnRadius = 15.0f; // 소환 좌표 간격.
    public float _spawnTime = 5.0f; // 소환 주기.


    #endregion
}