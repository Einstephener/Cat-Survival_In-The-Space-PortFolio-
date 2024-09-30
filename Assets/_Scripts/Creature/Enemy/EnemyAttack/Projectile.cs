using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Projectile : Poolable
{
    private Rigidbody _rigidbody;
    private Transform _target;

    private float _attackSpeed;
    private float _damage;
    public float maxDistance = 50f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("프리팹의 Rigidbody를 가져올 수 없습니다.");
        }
    }

    public void Init(Transform target, float attackSpeed, float damage)
    {
        this._target = target;
        this._attackSpeed = attackSpeed;
        this._damage = damage;

        _rigidbody.velocity = Vector3.zero;
        transform.position = transform.position;

        Vector3 direction = (_target.position - transform.position).normalized;
        _rigidbody.AddForce(direction * _attackSpeed, ForceMode.Impulse);
        Debug.Log(direction);
    }

    private void FixedUpdate()
    {
        if (_target == null || Vector3.Distance(transform.position, _target.position) > maxDistance)
        {
            Main.Pool.Push(this); // 타겟이 없거나 사정거리를 넘어가면 풀로 반환.
        }

        //// 타겟으로 투사체 이동.
        //Vector3 direction = (_target.position - transform.position).normalized;
        //transform.position += direction * _attackSpeed * Time.deltaTime;

        //// 타겟에 도달했는지 확인.
        //if (Vector3.Distance(transform.position, _target.position) < 0.1f /* ||
        //    Vector3.Distance(transform.position, transform.position) > 20f */)
        //{
        //    HitTarget();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = _target.gameObject;
            if (player.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
            {
                playerCondition.UpdateHealth(-_damage); // 체력 감소이므로 - 부호를 곱함.
            }

            Main.Pool.Push(this);
        }
    }

//    private void HitTarget()
//    {
//        // 타겟에 데미지를 입힘.
//        Debug.Log($"원거리 {_damage} 데미지");

//        // TODO : 플레이어 Hp 깎기.

//        // 풀로 반환.
//        Main.Pool.Push(this);
//    }
}