using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public EnemyBossData bossSO;
    public float speed;
    private float _lastSkillTime;
    private bool _isCastingSkill;

    protected override void Awake()
    {
        base.Awake();
        Init(bossSO);
        _isCastingSkill = false;

        if (_isCastingSkill) { }
    }

    private void Update()
    {
        UpdateState();
        speed = aiPath.maxSpeed;

        if (_playerTransform != null && _currentState is EnemyAttackState)
        {
            Vector3 direction = _playerTransform.position - transform.position;
            direction.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(direction);
            transform.rotation = quaternion;
        }
    }

    public override void GetReward()
    {
        base.GetReward();
        Main.Inventory.AddItem(_enemyData.rewardItem, 5);
        Debug.Log($"{_enemyData.rewardItem} 획득했습니다.");
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);

        // 체력이 절반 아래가 되면.
        if (IsCastingSkill())
        {
            StartCoroutine(CastMeleeSkill());
        }
    }

    protected override void MeleeAttack()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) <= bossSO.skillRange)
        {
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, bossSO.skillRange, _playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log($"근거리 {bossSO.skillDamage} 데미지");

                    if (hit.collider.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
                    {
                        playerCondition.UpdateHealth(-bossSO.skillDamage);
                    }
                }
            }
        }
    }

    public bool IsCastingSkill()
    {
        return _currentHp <= _enemyData.maxHp / 2;
    }

    public bool IsSkillCooldownCheck()
    {
        if (Time.time >= _lastSkillTime + bossSO.skillCooldown)
        {
            _lastSkillTime = Time.time;
            return true;
        }
        return false;
    }

    public bool IsSkillRange()
    {
        if(Vector3.Distance(transform.position, _playerTransform.position) <= bossSO.skillRange)
        {
            Debug.Log("범위 도착");
            return true;
        }
        return false;
    }

    private void MoveTowardsPlayer()
    {
        if (aiPath != null && _playerTransform != null)
        {
            Debug.Log("플레이어에게 스킬 사용을 위한 이동 중");
            aiPath.destination = _playerTransform.position;
            aiPath.canMove = true;
        }
    }

    public IEnumerator CastMeleeSkill()
    {
        _isCastingSkill = true;

        while (_currentHp <= _enemyData.maxHp / 2)
        {
            if (IsSkillRange())
            {
                if (IsSkillCooldownCheck())
                {
                    MeleeAttack();
                }
            }
            else
            {
                MoveTowardsPlayer();
            }

            yield return null;
        }

        _isCastingSkill = false;
    }

    #region Gizmos
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bossSO.skillRange);
    }
    #endregion
}