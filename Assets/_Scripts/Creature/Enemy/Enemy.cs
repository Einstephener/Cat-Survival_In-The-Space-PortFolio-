using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform basePosition;
    private Transform playerPosition;
    private float _sightRange = 7f;
    private float _attackRange = 4f;
    private AIDestinationSetter _target;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private Animator _animator;
    [HideInInspector] public bool _isChasing = false;
    [HideInInspector] public bool _isAttack = false;

    private IEnemyState currentState; // 현재 적의 상태

    private void Awake()
    {
        _target = GetComponent<AIDestinationSetter>();
        _playerLayer = LayerMask.GetMask("Player");
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // 초기 상태를 배회 상태로 설정
        currentState = new EnemyProwlState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        FindTarget();
        if (playerPosition == null)
        {
            _target.target = basePosition;
        }
        // 현재 상태를 매 프레임마다 업데이트
        currentState.UpdateState(this);
    }

    private void FindTarget()
    {
        Collider[] sight = Physics.OverlapSphere(transform.position, _sightRange, _playerLayer);
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer);

        // hits 배열이 비어 있지 않으면 플레이어가 감지된 것.
        if (sight.Length > 0)
        {
            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;

            // A* Pathfinding에서의 타겟을 플레이어로 설정.
            if (_target != null)
            {
                _target.target = _playerTransform;
                playerPosition = _playerTransform;
                
                if (hits.Length > 0)
                {
                    _isAttack = true;
                    _isChasing = false;
                }
                else
                {
                    // 추격.
                    _isChasing = true;
                    _isAttack = false;
                }
            }
        }
        else
        {
            if (_target != null)
            {
                // 감지된 플레이어가 없으면 추격을 멈춤.
                _isChasing = true;
                _isAttack = false;
                _target.target = null;
                playerPosition = null;
            }
        }
    }

    #region Gizmos
    // 시야 범위를 Gizmos로 시각화 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
    #endregion

    #region EnemyStateChange
    // 상태 전환 메서드
    public void TransitionToState(IEnemyState newState)
    {
        currentState.ExitState(this);   // 현재 상태에서 나가고
        currentState = newState;        // 새로운 상태로 전환
        currentState.EnterState(this);  // 새로운 상태로 진입
    }

    public void Walking(bool isWalking)
    {
        _animator.SetBool("IsChasing", isWalking);
    }

    // 적의 공격 메서드
    public void Attack(bool isAttack)
    {
        _animator.SetBool("IsAttack", isAttack);
    }

    // 적의 속도 설정
    public void SetSpeed(float speed)
    {
        // 적의 속도 설정 로직
    }
    #endregion
}
