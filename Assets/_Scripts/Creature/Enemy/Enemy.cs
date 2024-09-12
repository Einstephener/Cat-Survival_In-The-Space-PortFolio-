using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum EnemyStateType
//{
//    Idle,
//    Prowl,
//    Walking,
//    Chasing,
//    Hit,
//    Attack,
//}

public class Enemy : MonoBehaviour
{
    public Transform basePosition;
    private Transform playerPosition;
    private float _sightRange = 7f;
    private float _attackRange = 4f;
    private AIDestinationSetter _target;
    private AIPath _aiPath;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private Animator _animator;

    //public EnemyStateType enemyStateType;
    [HideInInspector] public bool _isChasing = false;
    [HideInInspector] public bool _isAttack = false;
    [HideInInspector] public bool _isWalk = false;

    private IEnemyState _currentState; // 현재 적의 상태

    private void Awake()
    {
        _target = GetComponent<AIDestinationSetter>();
        _aiPath = GetComponent<AIPath>();
        _playerLayer = LayerMask.GetMask("Player");
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentState = new EnemyIdleState();
        _currentState.EnterState(this);
    }

    private void Update()
    {
        FindTarget();
        if (playerPosition == null)
        {
            _target.target = basePosition;
        }
        // 현재 상태를 매 프레임마다 업데이트
        _currentState.UpdateState(this);
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
                    // 이동을 멈추고 공격 상태로 전환
                    //enemyStateType = EnemyStateType.Attack;
                    _isAttack = true;
                    _isChasing = false;
                    _isWalk = false;
                    _aiPath.canMove = false;  // AIPath 이동 중지
                }
                else
                {
                    //enemyStateType = EnemyStateType.Chasing;
                    _isAttack = false;
                    _isChasing = false;
                    _isWalk = true;

                    _aiPath.canMove = true;
                    SetSpeed(4f);
                }
            }
        }
        else
        {
            if (_target != null)
            {
                // 감지된 플레이어가 없으면 추격을 멈춤.
                _target.target = null;
                playerPosition = null;
                _aiPath.canMove = true;
                SetSpeed(2f);
            }
        }
    }

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        // 시야 범위.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        // 공격 범위.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    #endregion

    #region EnemyStateChange
    // 상태 전환 메서드
    public void TransitionToState(IEnemyState newState)
    {
        _currentState.ExitState(this);   // 현재 상태에서 나가고
        _currentState = newState;        // 새로운 상태로 전환
        _currentState.EnterState(this);  // 새로운 상태로 진입
    }

    public void Idle()
    {
        _animator.SetFloat("Speed", 0.1f);
    }

    public void Chasing()
    {
        _animator.SetFloat("Speed", _aiPath.maxSpeed);
    }

    // 적의 공격 메서드
    public void Attack()
    {
        _animator.SetTrigger("OnAttack");
    }

    // 적의 속도 설정
    public void SetSpeed(float speed)
    {
        if (_aiPath != null)
        {
            _aiPath.maxSpeed = speed;  // AIPath의 최대 속도 설정.
        }
    }
    #endregion
}
