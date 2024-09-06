using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _sight = 3f;
    [SerializeField] private AIDestinationSetter _target;
    private Transform _playerTransform;
    private bool _isChasing = false;
    [SerializeField] private string _playerTag = "Player";

    private void Update()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        // 시야 범위 안에 있는 모든 콜라이더를 감지.
        Collider[] hits = Physics.OverlapSphere(transform.position, _sight);
        foreach (Collider hit in hits)
        {
            // 태그가 "Player"인지 확인.
            if (hit.CompareTag(_playerTag))
            {
                _playerTransform = hit.transform;
                // 추적할 타겟 설정.
                if (_target != null)
                {
                    _target.target = _playerTransform;
                    _isChasing = true;
                }
                return;
            }
        }
        // 시야 범위 내에 플레이어가 없으면 추적 중지.
        if (_target != null)
        {
            // 감지된 플레이어가 없으면 추격을 멈춤
            _isChasing = false;
            _target.target = null;
        }
    }

    // 시야 범위를 Gizmos로 시각화 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sight);
    }
}
