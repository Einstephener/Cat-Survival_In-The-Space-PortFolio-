using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Field
    [SerializeField] private Transform _projectileSpawnPoint;

    public Animator animator { get; private set; }
    public AIPath aiPath { get; private set; }

    private Vector3 _basePosition;
    private AIDestinationSetter _target;

    protected LayerMask _playerLayer;
    protected IEnemyState _currentState;

    protected Transform _playerTransform;
    protected EnemyData _enemyData;
    protected Collider _collider;

    private float _lastAttackTime;
    protected float _currentSightRange;
    protected float _currentHp;

    #endregion

    protected virtual void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponentInChildren<Animator>();
        _target = GetComponent<AIDestinationSetter>();
        _collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        _basePosition = transform.position;
        _collider.enabled = true;

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
    }

    protected virtual void Init(EnemyData enemyData)
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

    // 쿨타임 체크.
    public bool AttackCooldownCheck()
    {
        if (Time.time >= _lastAttackTime + _enemyData.attackCooldown)
        {
            _lastAttackTime = Time.time;
            return true;
        }
        return false;
    }

    protected virtual void MeleeAttack()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _enemyData.attackRange)
        {
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _enemyData.attackRange, _playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log($"근거리 {_enemyData.damage} 데미지");

                    if (hit.collider.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
                    {
                        playerCondition.UpdateHealth(-_enemyData.damage);
                    }
                }
            }
        }
    }

    protected virtual void FireProjectile()
    {
        SoundManager.Instance.PlaySFX("SFX_Swoosh", .5f);
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
            aiPath.maxSpeed = speed + _enemyData.addSpeed;  // AIPath의 최대 속도 설정.
        }
    }

    public PlayerCondition GetPlayerCondition()
    {
        if (_playerTransform == null) return null;
        return _playerTransform.GetComponent<PlayerCondition>();
    }

    // 플레이어가 시야에 있는지 체크 후 타겟 설정.
    public bool IsTarget()
    {
        Collider[] sight = Physics.OverlapSphere(transform.position, _currentSightRange, _playerLayer);
        // 배열이 비어 있지 않으면 플레이어가 감지된 것.
        if (sight.Length > 0)
        {
            // 첫 번째로 감지된 플레이어를 대상으로 설정.
            _playerTransform = sight[0].transform;

            PlayerCondition playerCondition = GetPlayerCondition();
            if (playerCondition != null && playerCondition.IsDead())
            {
                // 플레이어가 죽었다면 타겟 해제.
                _currentSightRange = _enemyData.sightRange;
                Debug.Log("플레이어가 죽었습니다.");
                return false;
            }

            // 플레이어가 감지되면 시야가 넓어짐.
            _currentSightRange = _enemyData.sightRange + _enemyData.exitBuffer;
            _target.target = _playerTransform;
            //Debug.Log("플레이어 감지");

            return true;
        }

        _currentSightRange = _enemyData.sightRange;
        return false;
    }

    // 플레이어가 공격범위 내에 있는가 확인.
    public bool IsAttackRange()
    {
        if (_playerTransform == null && _currentState is EnemyWalkingState) return false;

        // 플레이어 죽었는지 체크.
        PlayerCondition playerCondition = GetPlayerCondition();
        if (playerCondition != null && playerCondition.IsDead())
        {
            Debug.Log("플레이어가 죽어서 공격 중단");
            aiPath.canMove = true;
            return false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= _enemyData.attackRange)
        {
            aiPath.canMove = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // 현재 리스폰지역에 있는가 확인.
    public bool IsHome()
    {
        if (Vector3.Distance(transform.position, _basePosition) < 0.5f)
        {
            //Debug.Log("도착");
            _currentSightRange = _enemyData.sightRange;
            return true;
        }

        //Debug.Log("집 가는 중");
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

        TransitionToState(new EnemyHitState());
        _currentHp -= damage;
        Debug.Log(_currentHp);
        if (_currentHp <= 0) IsDead();
    }

    public virtual void GetReward()
    {
        // 기본 보상.
    }

    public bool IsDead()
    {
        if(_currentHp <= 0)
        {
            _target.target = null;
            _collider.enabled = false;
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
    public void TransitionToState(IEnemyState newState)
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
