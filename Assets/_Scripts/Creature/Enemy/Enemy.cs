using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _sight = 5f;
    private AIDestinationSetter _target;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private bool _isChasing = false;

    private void Awake()
    {
        _target = GetComponent<AIDestinationSetter>();
        _playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _sight, _playerLayer);
        // hits 배열이 비어 있지 않으면 플레이어가 감지된 것
        if (hits.Length > 0)
        {
            // 첫 번째로 감지된 플레이어를 대상으로 설정
            _playerTransform = hits[0].transform;

            // A* Pathfinding에서의 타겟을 플레이어로 설정
            if (_target != null)
            {
                Debug.Log("Target Set: " + _target.target);
                _target.target = _playerTransform;

                // 추격 시작
                _isChasing = true;
            }
        }
        else
        {
            if (_target != null)
            {
                // 감지된 플레이어가 없으면 추격을 멈춤
                _isChasing = false;
                _target.target = null;
            }
        }
    }

    // 시야 범위를 Gizmos로 시각화 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sight);
    }
}
