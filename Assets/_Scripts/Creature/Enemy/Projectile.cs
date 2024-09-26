using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private Vector3 _target;
    private float _attackSpeed;
    private float _damage;

    public void Init(Transform target, float attackSpeed, float damage)
    {
        this._target = target.position;
        this._attackSpeed = attackSpeed;
        this._damage = damage;

        transform.position = transform.position;
    }

    private void Update()
    {
        if (_target == Vector3.zero)
        {
            Main.Pool.Push(this); // 타겟이 없으면 풀로 반환.
            return;
        }

        // 타겟으로 투사체 이동.
        Vector3 direction = (_target - transform.position).normalized;
        transform.position += direction * _attackSpeed * Time.deltaTime;

        // 타겟에 도달했는지 확인.
        if (Vector3.Distance(transform.position, _target) < 0.1f ||
            Vector3.Distance(transform.position, transform.position) > 20f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        // 타겟에 데미지를 입힘.

        // 풀로 반환.
        Main.Pool.Push(this);
    }
}