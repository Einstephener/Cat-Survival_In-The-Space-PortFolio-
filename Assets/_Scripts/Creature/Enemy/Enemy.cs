using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Field
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private GameObject _melee;
    [SerializeField] private float HP; // 추후 삭제.

    private MeleeHitbox _meleeHitbox;

    public Animator animator { get; private set; }
    public AIPath aiPath { get; private set; }

    private Vector3 _basePosition;
    private LayerMask _playerLayer;
    private Transform _playerTransform;
    private AIDestinationSetter _target;

    private IEnemyState _currentState;

    protected EnemyData _enemyData;

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

        if (_enemyData.attackType == IAttackType.Melee || _enemyData.attackType == IAttackType.Both)
        {
            _meleeHitbox = _melee.GetComponent<MeleeHitbox>();
        }

        _playerLayer = LayerMask.GetMask("Player");
        TransitionToState(new EnemyIdleState());
    }

    private void Update()
    {
        UpdateState();

        if (_playerTransform != null && _currentState is EnemyAttackState) 
        {
            Vector3 direction = _playerTransform.position - transform.position;
            direction.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(direction);
            transform.rotation = quaternion;
        }

        HP = _currentHp; // 추후 삭제.
    }

    protected void Init(EnemyData enemyData)
    {
        _enemyData = enemyData;
        _currentSightRange = _enemyData.sightRange;
        _currentHp = _enemyData.maxHp;
    }

    // 애니메이션 이벤트.
    protected virtual void OnAttack()
    {
        if (_enemyData.attackType == IAttackType.Melee || _enemyData.attackType == IAttackType.Both)
        {
            MeleeAttack();
        }
        else if (_enemyData.attackType == IAttackType.Ranged || _enemyData.attackType == IAttackType.Both)
        {
            FireProjectile();
        }
    }

    protected virtual void MeleeAttack()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _enemyData.attackRange)
        {
            if (_meleeHitbox != null)
            {
                _meleeHitbox.Activate(_enemyData.damage);
            }
        }
    }

    // 애니메이션 이벤트 : 공격 시간이 지난 후 히트박스 비활성화.
    protected virtual void MeleeHitboxDeactivate()
    {
        if (_meleeHitbox != null)
        {
            _meleeHitbox.Deactivate();
        }
    }

    protected virtual void FireProjectile()
    {
        // 투사체 생성 및 발사.
        Projectile projectile = Main.Pool.Pop(_enemyData.projectilePrefab).GetComponent<Projectile>();

        // 투사체의 위치와 방향 설정.
        projectile.transform.position = _projectileSpawnPoint.position;
        projectile.Init(_playerTransform, _enemyData.damage); // 투사체 초기화.
    }

    public void SetSpeed(float speed)
    {
        if (aiPath != null)
        {
            aiPath.maxSpeed = speed;  // AIPath의 최대 속도 설정.
        }
    }

    // 플레이어가 시야에 있는지 체크 후 타겟 설정.
    public bool IsTarget()
    {
        Collider[] sight = Physics.OverlapSphere(transform.position, _currentSightRange, _playerLayer);
        // 배열이 비어 있지 않으면 플레이어가 감지된 것.
        if (sight.Length > 0)
        {
            // 플레이어가 감지되면 시야가 넓어짐.
            _currentSightRange = _enemyData.sightRange + _enemyData.exitBuffer;

            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;
            _target.target = _playerTransform;
            aiPath.canMove = true;
            Debug.Log("플레이어 감지");

            return true;
        }

        // TODO : 플레이어가 죽었다면 타겟이 되지 않도록 처리.
        _currentSightRange = _enemyData.sightRange;
        return false;
    }

    // 플레이어가 공격범위 내에 있는가 확인.
    public bool IsAttackRange()
    {
        if (_playerTransform == null && _currentState is EnemyWalkingState) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        
        return distanceToPlayer <= _enemyData.attackRange;
    }

    // 현재 리스폰지역에 있는가 확인.
    public bool IsHome()
    {
        if (Vector3.Distance(transform.position, _basePosition) < 0.1f)
        {
            _currentSightRange = _enemyData.sightRange;
            return true;
        }

        _target.target = null;
        aiPath.destination = _basePosition;
        return false;
    }

    public void RegainHp()
    {
        _currentHp = _enemyData.maxHp;
        Debug.Log("체력 리젠 : " + _currentHp);
    }

    public virtual void OnHit(float damage)
    {
        if (_currentState is EnemyWalkingState || IsDead())
        {
            return;
        }

        _currentHp -= damage;
        Debug.Log(_currentHp);
        if (_currentHp <= 0) IsDead();
    }

    public virtual void GetReward()
    {
        // 기본 보상.
    }

    public virtual bool IsDead()
    {
        if(_currentHp <= 0)
        {
            _target.target = null;
            return true;
        }
        else return false;
    }

    #region Gizmos
    protected virtual void OnDrawGizmosSelected()
    {
        if (_enemyData == null) return;

        // 시야 범위.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _currentSightRange);

        // 공격 범위.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyData.attackRange);
    }
    #endregion

    #region EnemyStateChange
    // 상태 전환 메서드
    public virtual void TransitionToState(IEnemyState newState)
    {
        if (_currentState is EnemyWalkingState && newState is EnemyHitState)
        {
            return;
        }

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
