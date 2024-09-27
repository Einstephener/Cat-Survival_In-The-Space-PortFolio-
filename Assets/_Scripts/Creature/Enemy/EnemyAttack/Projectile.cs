using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private Transform _target;
    private Transform _parent;

    private float _attackTime;
    private float _damage;

    private IAttackType _attackType;

    public void Init(Transform target, float attackSpeed, float damage, IAttackType attackType, Transform parent = null)
    {
        this._target = target;
        this._parent = parent;
        this._attackTime = attackSpeed;
        this._damage = damage;
        this._attackType = attackType;

        if (_attackType == IAttackType.Melee)
        {
            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(null);
        }
    }

    private void Update()
    {
        if (_attackType == IAttackType.Melee) return;

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

        // 풀로 반환.
        Main.Pool.Push(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_attackType == IAttackType.Melee && other.CompareTag("Player"))
        {
            Debug.Log("뭐지");
        }
    }
}