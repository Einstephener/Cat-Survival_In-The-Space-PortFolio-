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
    [SerializeField] public Transform basePosition;

    private AIPath _aiPath;
    private Animator _animator;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private AIDestinationSetter _target;

    private EnemyState _enemyState;
    private IEnemyState _currentState;

    private float _sightRange = 5f;
    private float _attackRange = 3f;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _enemyState = EnemyState.Idle;
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
    public virtual void TransitionToState(IEnemyState newState)
    {
        if (newState == _currentState) return;
        _currentState?.ExitState(this);

        _currentState = newState;
        _currentState.EnterState(this);
    }

    public virtual void UpdateState()
    {
        _currentState?.UpdateState(this);
    }

    private void UpdateIdle()
    {
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
        if (_enemyState == EnemyState.Dead)
            return;

        if (transform == basePosition)
        {
            _enemyState = EnemyState.Idle;
        }
        else
        {
            SetSpeed(2f);
            _animator.SetFloat("Speed", _aiPath.maxSpeed);
        }
    }

    private void UpdateChasing()
    {
        Debug.Log("추적");
        if (_enemyState == EnemyState.Dead)
            return;

        // A* Pathfinding에서의 타겟을 플레이어로 설정.
        if (_playerTransform != null)
        {
            _target.target = _playerTransform;
            _aiPath.canMove = true;
            SetSpeed(4f);
            _animator.SetFloat("Speed", _aiPath.maxSpeed);
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
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer);
        if (hits.Length > 0)
        {
            // 이동을 멈추고 공격 상태로 전환
            _enemyState = EnemyState.Attack;
            _aiPath.canMove = false;  // AIPath 이동 중지
            _animator.SetTrigger("OnAttack");
        }
        else
        {
            _enemyState = EnemyState.Chasing;
            _aiPath.canMove = true;
        }
    }

    private void UpdateDead()
    {
        // 죽음.
    }
    #endregion
}
