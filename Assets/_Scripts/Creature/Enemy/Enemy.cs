using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Field
    public Animator animator { get; private set; }
    public AIPath aiPath { get; private set; }
    protected EnemyData enemyData;

    private Vector3 _basePosition;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private AIDestinationSetter _target;

    private IEnemyState _currentState;

    protected float _currentSightRange;

    protected float _currentHp;
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
        _playerLayer = LayerMask.GetMask("Player");
        TransitionToState(new EnemyIdleState());
    }

    private void Update()
    {
        UpdateState();
    }

    protected void Init(EnemyData enemy)
    {
        enemyData = enemy;
        _currentSightRange = enemyData.sightRange;
        _currentHp = enemyData.maxHp;
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
        Debug.Log($"{_target.target} 에게 {enemyData.damage} 를 입혔습니다.");
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
            _currentSightRange = enemyData.sightRange + enemyData.exitBuffer;

            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;
            _target.target = _playerTransform;
            aiPath.canMove = true;
            Debug.Log("플레이어 감지");

            return true;
        }

        _currentSightRange = enemyData.sightRange;
        return false;
    }

    // 플레이어가 공격범위 내에 있는가 확인.
    public bool IsAttackRange()
    {
        if (_playerTransform == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        return distanceToPlayer <= enemyData.attackRange;
    }

    // 현재 리스폰지역에 있는가 확인.
    public bool IsHome()
    {
        if (Vector3.Distance(transform.position, _basePosition) < 0.1f)
        {
            _currentSightRange = enemyData.sightRange;
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
        if (enemyData == null) return;

        // 시야 범위.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _currentSightRange);

        // 공격 범위.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
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
    #endregion
}
