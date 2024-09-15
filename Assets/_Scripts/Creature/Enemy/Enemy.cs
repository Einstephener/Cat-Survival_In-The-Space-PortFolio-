using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Prowl,
    Walking,
    Chasing,
    Hit,
    Attack,
    Dead,
}

public class Enemy : MonoBehaviour
{
    private Vector3 _basePosition;
    private AIPath _aiPath;
    private Animator _animator;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private AIDestinationSetter _target;

    private EnemyState _enemyState;
    //private IEnemyState _currentState;

    private float _sightRange = 8f;
    private float exitBuffer = 2f;  // 플레이어가 나갈 때 추가 거리.(시야 경계선에 있을 때, 버그 방지.)
    private float _attackRange = 4f;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
        _animator = GetComponentInChildren<Animator>();
        _target = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        _basePosition = transform.position;
        _enemyState = EnemyState.Idle;
        _playerLayer = LayerMask.GetMask("Player");
        //TransitionToState(new EnemyIdleState());
    }

    private void Update()
    {
        //UpdateState();

        switch (_enemyState)
        {
            case EnemyState.Idle:
                // Idle 행동 구현.
                UpdateIdle();
                break;
            case EnemyState.Prowl:
                // Prowl 행동 구현.

                break;
            case EnemyState.Walking:
                // Walking 행동 구현.
                UpdateWalking();
                break;
            case EnemyState.Chasing:
                // Chasing 행동 구현.
                UpdateChasing();
                break;
            case EnemyState.Hit:
                // Hit 행동 구현.
                UpdateHit();
                break;
            case EnemyState.Attack:
                // Attack 행동 구현.
                UpdateAttack();
                break;
            case EnemyState.Dead:
                UpdateDead();
                break;
        }
    }

    public void SetSpeed(float speed)
    {
        if (_aiPath != null)
        {
            _aiPath.maxSpeed = speed;  // AIPath의 최대 속도 설정.
        }
    }

    #region Gizmos
    public virtual void OnDrawGizmosSelected()
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
    //public virtual void TransitionToState(IEnemyState newState)
    //{
    //    if (newState == _currentState) return;
    //    _currentState?.ExitState(this);

    //    _currentState = newState;
    //    _currentState.EnterState(this);
    //}

    //public virtual void UpdateState()
    //{
    //    _currentState?.UpdateState(this);
    //}

    private void UpdateIdle()
    {
        if (IsDead())
            return;

        Debug.Log("대기");
        _animator.SetFloat("Speed", 0.1f);

        Collider[] sight = Physics.OverlapSphere(transform.position, _sightRange, _playerLayer);
        // 배열이 비어 있지 않으면 플레이어가 감지된 것.
        if (sight.Length > 0)
        {
            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;
            Debug.Log("플레이어 감지");
            _enemyState = EnemyState.Chasing;
        }
    }

    private void UpdateWalking()
    {
        if (IsDead())
            return;

        if (Vector3.Distance(transform.position, _basePosition) < 0.1f)
        {
            _enemyState = EnemyState.Idle;
        }
        else
        {
            SetSpeed(2f);
            _animator.SetFloat("Speed", _aiPath.maxSpeed);
            _target.target = null;
            _aiPath.destination = _basePosition;
        }
    }

    private void UpdateChasing()
    {
        Debug.Log("추적");
        if (IsDead())
            return;

        // A* Pathfinding에서의 타겟을 플레이어로 설정.
        if (_playerTransform != null)
        {
            _target.target = _playerTransform;
            _aiPath.canMove = true;
            SetSpeed(4f);
            _animator.SetFloat("Speed", _aiPath.maxSpeed);

            // 공격 범위 체크
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= _attackRange)
            {
                _enemyState = EnemyState.Attack;
                return;  // 공격 상태로 전환했으니 추적 로직을 종료.
            }

            // 시야 범위를 벗어났는지 체크
            if (distanceToPlayer > _sightRange + exitBuffer)
            {
                Debug.Log("플레이어 시야에서 벗어남, 복귀");
                _playerTransform = null;
                _enemyState = EnemyState.Walking;
                _target.target = null;
                _aiPath.destination = _basePosition;
            }
        }
        else
        {
            _enemyState = EnemyState.Walking;
            _aiPath.canMove = true;
        }
    }

    private void UpdateHit()
    {
        _animator.SetTrigger("OnHit");
    }

    private void UpdateAttack()
    {
        if (IsDead()) return;

        _animator.SetTrigger("OnAttack");

        // 플레이어와의 거리 체크 (만약 플레이어가 멀어졌다면 다시 추적)
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer > _attackRange)
        {
            _enemyState = EnemyState.Chasing;
            _aiPath.canMove = true;
        }
        else
        {
            _aiPath.canMove = false;  // 공격 중에는 이동하지 않음
        }
    }

    private void UpdateDead()
    {
        // 죽음.
    }

    private bool IsDead()
    {
        return _enemyState == EnemyState.Dead;
    }
    #endregion
}
