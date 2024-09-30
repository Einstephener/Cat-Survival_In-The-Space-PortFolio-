using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private Transform _target;
    private GameObject _player;

    private float _attackTime;
    private float _damage;

    public void Init(Transform target, GameObject player, float attackSpeed, float damage)
    {
        this._target = target;
        this._attackTime = attackSpeed;
        this._damage = damage;
        this._player = player;

        transform.position = transform.position;
    }

    private void Update()
    {
        if (_target == null)
        {
            Main.Pool.Push(this); // 타겟이 없으면 풀로 반환.
            return;
        }

        // 타겟으로 투사체 이동.
        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _attackTime * Time.deltaTime;

        // 타겟에 도달했는지 확인.
        if (Vector3.Distance(transform.position, _target.position) < 0.1f /* ||
            Vector3.Distance(transform.position, transform.position) > 20f */)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        // 타겟에 데미지를 입힘.
        Debug.Log($"원거리 {_damage} 데미지");

        // TODO : 플레이어 Hp 깎기.
        if (_player.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
        {
            playerCondition.UpdateHealth(-_damage); // 체력 감소이므로 - 부호를 곱함.
        }

        // 풀로 반환.
        Main.Pool.Push(this);
    }
}