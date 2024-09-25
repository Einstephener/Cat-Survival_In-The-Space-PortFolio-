using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum EnemyState
//{
//    Idle,
//    Prowl,
//    Walking,
//    Chasing,
//    Hit,
//    Attack,
//    Dead,
//}

public class Enemy : MonoBehaviour
{
    #region Field
    public Animator animator { get; private set; }
    public AIPath aiPath { get; private set; }

    private Vector3 _basePosition;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private AIDestinationSetter _target;

    // private EnemyState _enemyState;
    private IEnemyState _currentState;

    protected float _currentSightRange;
    protected float _sightRange = 8f;
    protected float _exitBuffer = 5f;  // 플레이어가 감지 되었을 때 추가 거리.(시야 경계선에 있을 때, 버그 방지.)
    protected float _attackRange = 3f;

    protected float _hp = 200f;
    protected float _currentHp;
    protected float _damage = 10f;
    #endregion

    protected virtual void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponentInChildren<Animator>();
        _target = GetComponent<AIDestinationSetter>();
    }

    protected virtual void Start()
    {
        _basePosition = transform.position;
        _currentSightRange = _sightRange;
        _currentHp = _hp;
        // _enemyState = EnemyState.Idle;
        _playerLayer = LayerMask.GetMask("Player");
        TransitionToState(new EnemyIdleState());
    }

    private void Update()
    {
        UpdateState();

        //switch (_enemyState)
        //{
        //    case EnemyState.Idle:
        //        // Idle 행동 구현.
        //        UpdateIdle();
        //        break;
        //    case EnemyState.Prowl:
        //        // Prowl 행동 구현.

        //        break;
        //    case EnemyState.Walking:
        //        // Walking 행동 구현.
        //        UpdateWalking();
        //        break;
        //    case EnemyState.Chasing:
        //        // Chasing 행동 구현.
        //        UpdateChasing();
        //        break;
        //    case EnemyState.Hit:
        //        // Hit 행동 구현.
        //        UpdateHit();
        //        break;
        //    case EnemyState.Attack:
        //        // Attack 행동 구현.
        //        UpdateAttack();
        //        break;
        //    case EnemyState.Dead:
        //        UpdateDead();
        //        break;
        //}
    }

    public void SetSpeed(float speed)
    {
        if (aiPath != null)
        {
            aiPath.maxSpeed = speed;  // AIPath의 최대 속도 설정.
        }
    }

    public virtual void OnAttack()
    {
        Debug.Log($"{_target.target} 에게 {_damage} 를 입혔습니다.");
    }

    public virtual void OnHit(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0) IsDead();
    }

    // 플레이어가 시야에 있는지 체크 후 타겟 설정.
    public bool IsTarget()
    {
        Collider[] sight = Physics.OverlapSphere(transform.position, _currentSightRange, _playerLayer);
        // 배열이 비어 있지 않으면 플레이어가 감지된 것.
        if (sight.Length > 0)
        {
            // 플레이어가 감지되면 시야가 넓어짐.
            _currentSightRange = _sightRange + _exitBuffer;

            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;
            _target.target = _playerTransform;
            aiPath.canMove = true;
            Debug.Log("플레이어 감지");

            //float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            //// 시야 범위를 벗어났는지 체크
            //if (distanceToPlayer > _sightRange + _exitBuffer)
            //{
            //    Debug.Log("플레이어 시야에서 벗어남, 복귀");
            //    _playerTransform = null;
            //    _target.target = null;
            //}
            return true;
        }

        _currentSightRange = _sightRange;
        return false;
    }

    // 플레이어가 공격범위 내에 있는가 확인.
    public bool IsAttackRange()
    {
        if (_playerTransform == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        return distanceToPlayer <= _attackRange;
    }

    // 현재 리스폰지역에 있는가 확인.
    public bool IsHome()
    {
        if (Vector3.Distance(transform.position, _basePosition) < 0.1f)
        {
            _currentSightRange = _sightRange;
            return true;
        }

        _target.target = null;
        aiPath.destination = _basePosition;
        return false;
    }

    public bool IsDead()
    {
        // TODO.
        return false;
    }

    #region Gizmos
    protected virtual void OnDrawGizmosSelected()
    {
        // 시야 범위.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _currentSightRange);

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

    protected virtual void UpdateState()
    {
        _currentState?.UpdateState(this);
    }

    //private void UpdateIdle()
    //{
    //    if (IsDead())
    //        return;

    //    Debug.Log("대기");
    //    animator.SetFloat("Speed", 0.1f);

    //    Collider[] sight = Physics.OverlapSphere(transform.position, _currentSightRange, _playerLayer);
    //    // 배열이 비어 있지 않으면 플레이어가 감지된 것.
    //    if (sight.Length > 0)
    //    {
    //        // 첫 번째로 감지된 플레이어를 대상으로 설정.
    //        _playerTransform = sight[0].transform;
    //        Debug.Log("플레이어 감지");
    //        _enemyState = EnemyState.Chasing;
    //    }
    //}

    //private void UpdateWalking()
    //{
    //    if (IsDead())
    //        return;

    //    if (Vector3.Distance(transform.position, _basePosition) < 0.1f)
    //    {
    //        _enemyState = EnemyState.Idle;
    //    }
    //    else
    //    {
    //        SetSpeed(2f);
    //        animator.SetFloat("Speed", aiPath.maxSpeed);
    //        _target.target = null;
    //        aiPath.destination = _basePosition;
    //    }
    //}

    //private void UpdateChasing()
    //{
    //    Debug.Log("추적");
    //    if (IsDead())
    //        return;

    //    // A* Pathfinding에서의 타겟을 플레이어로 설정.
    //    if (_playerTransform != null)
    //    {
    //        _target.target = _playerTransform;
    //        aiPath.canMove = true;
    //        SetSpeed(4f);
    //        animator.SetFloat("Speed", aiPath.maxSpeed);

    //        // 공격 범위 체크
    //        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
    //        if (distanceToPlayer <= _attackRange)
    //        {
    //            _enemyState = EnemyState.Attack;
    //            return;  // 공격 상태로 전환했으니 추적 로직을 종료.
    //        }

    //        // 시야 범위를 벗어났는지 체크
    //        if (distanceToPlayer > _currentSightRange + _exitBuffer)
    //        {
    //            Debug.Log("플레이어 시야에서 벗어남, 복귀");
    //            _playerTransform = null;
    //            _enemyState = EnemyState.Walking;
    //            _target.target = null;
    //            aiPath.destination = _basePosition;
    //        }
    //    }
    //    else
    //    {
    //        _enemyState = EnemyState.Walking;
    //        aiPath.canMove = true;
    //    }
    //}

    //private void UpdateHit()
    //{
    //    animator.SetTrigger("OnHit");
    //}

    //private void UpdateAttack()
    //{
    //    if (IsDead()) return;

    //    animator.SetTrigger("OnAttack");

    //    // 플레이어와의 거리 체크 (만약 플레이어가 멀어졌다면 다시 추적)
    //    float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
    //    if (distanceToPlayer > _attackRange)
    //    {
    //        _enemyState = EnemyState.Chasing;
    //        aiPath.canMove = true;
    //    }
    //    else
    //    {
    //        aiPath.canMove = false;  // 공격 중에는 이동하지 않음
    //    }
    //}

    //private void UpdateDead()
    //{
    //    // 죽음.
    //}
    #endregion
}
