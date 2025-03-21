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

    protected override void OnAttack()
    {
        if (IsCastingSkill())
        {
            CastSkill();
        }
        else
        {
            base.OnAttack();
        }
    }

    protected override void FireProjectile()
    {
        if (_isCastingSkill) return; // 스킬 상태일 경우 투사체 공격 중단.

        base.FireProjectile();
    }

    public void CastSkill()
    {
        if (_playerTransform == null) return;

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
        if (_playerTransform == null) return false;

        // 플레이어 죽었는지 체크.
        PlayerCondition playerCondition = GetPlayerCondition();
        if (playerCondition != null && playerCondition.IsDead())
        {
            Debug.Log("플레이어가 죽어서 공격 중단");
            aiPath.canMove = true;
            return false;
        }

        if (Vector3.Distance(transform.position, _playerTransform.position) <= bossSO.skillRange)
        {
            aiPath.canMove = false;
            Debug.Log("범위 도착");
            return true;
        }
        return false;
    }

    public void MoveTowardsPlayer()
    {
        if (aiPath != null && _playerTransform != null)
        {
            Debug.Log("플레이어에게 스킬 사용을 위한 이동 중");
            aiPath.destination = _playerTransform.position;
            aiPath.canMove = true;
        }
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