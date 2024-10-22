using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public EnemyBossData bossSO;
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
        if (_playerTransform != null && Vector3.Distance(transform.position, _playerTransform.position) <= bossSO.skillRange && !_isCastingSkill)
        {
            StartCoroutine(CastMeleeSkill());
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
        if (_currentHp <= _enemyData.maxHp / 2 && !_isAddSpeed)
        {
            AddSpeed();
            CastSkill();
            _isAddSpeed = true;
        }
    }

    private void AddSpeed()
    {
        float newSpeed = aiPath.maxSpeed + bossSO.addSpeed;
        SetSpeed(newSpeed);
        Debug.Log("이동 속도가 빨라졌습니다!");
    }

    private void CastSkill()
    {
        if (_playerTransform != null)
        {
            Debug.Log($"{bossSO.skillDamage}의 데미지를 입히는 스킬을 시전합니다.");
            
            if (_playerTransform.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
            {
                playerCondition.UpdateHealth(-bossSO.skillDamage);
            }
        }
    }

    private IEnumerator CastMeleeSkill()
    {
        _isCastingSkill = true;

        // Move towards the player
        while (Vector3.Distance(transform.position, _playerTransform.position) > bossSO.skillRange)
        {
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            aiPath.destination = _playerTransform.position;  // Set AIPath destination to player
            yield return null;  // Wait for the next frame
        }

        // Perform MeleeAttack three times
        for (int i = 0; i < 3; i++)
        {
            MeleeAttack();
            yield return new WaitForSeconds(1f);  // Wait for 1 second between attacks
        }

        _isCastingSkill = false;  // Reset the casting flag
    }
}