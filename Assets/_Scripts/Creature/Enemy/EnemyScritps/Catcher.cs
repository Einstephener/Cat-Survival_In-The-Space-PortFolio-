using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public EnemyBossData bossSO;
    private float _lastSkillTime;
    private bool _isAddSpeed;
    private bool _isCastingSkill;

    protected override void Awake()
    {
        base.Awake();
        Init(bossSO);
        _isAddSpeed = false;
        _isCastingSkill = false;
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
            AddSpeed();
            StartCoroutine(CastMeleeSkill());
        }
    }

    public bool IsCastingSkill()
    {
        return _currentHp <= _enemyData.maxHp / 2 && !_isAddSpeed;
    }

    private void AddSpeed()
    {
        if (!_isAddSpeed) // 중복 증가 방지
        {
            float newSpeed = aiPath.maxSpeed + bossSO.addSpeed;
            SetSpeed(newSpeed);
            _isAddSpeed = true;
            Debug.Log("이동 속도가 빨라졌습니다!");
        }
    }

    public bool SkillCooldownCheck()
    {
        if (Time.time >= _lastSkillTime + bossSO.skillCooldown)
        {
            _lastSkillTime = Time.time;
            return true;
        }
        return false;
    }

    public IEnumerator CastMeleeSkill()
    {
        _isCastingSkill = true;

        while (Vector3.Distance(transform.position, _playerTransform.position) > bossSO.skillRange)
        {
            Debug.Log("플레이어에게 스킬 사용을 위한 이동 중");
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            aiPath.destination = _playerTransform.position; ;
            aiPath.canMove = true;
            yield return null;
        }

        aiPath.canMove = false;

        if (SkillCooldownCheck())
        {
            MeleeAttack();
        }
        else if(IsAttackRange())
        {
            FireProjectile();
        }

        yield return new WaitForSeconds(bossSO.skillCooldown);
        aiPath.canMove = true;
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