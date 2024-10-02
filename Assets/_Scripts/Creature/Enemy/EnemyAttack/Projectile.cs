using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Projectile : Poolable
{
    private Transform _target;
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private Vector3 _spawnPosition;

    private float _attackSpeed;
    private float _damage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody가 없습니다.");
        }
    }

    public void Init(Transform target, float attackSpeed, float damage)
    {
        this._target = target;
        this._attackSpeed = attackSpeed;
        this._damage = damage;

        _direction = (_target.position - transform.position).normalized;
        _spawnPosition = transform.position;

        _rigidbody.velocity = Vector3.zero; // 기존 속도 초기화. 중첩 방지.
        _rigidbody.AddForce(_direction * _attackSpeed, ForceMode.VelocityChange); // 기존 속도를 무시하고 즉시 속도 적용.
    }

    private void FixedUpdate()
    {
        if (_target == null || Vector3.Distance(_spawnPosition, _target.position) > 10f)
        {
            Main.Pool.Push(this); // 타겟이 없거나 사정거리를 넘어가면 풀로 반환.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit");
            HitTarget(other.gameObject);
            
        }
    }

    private void HitTarget(GameObject gameObject)
    {
        // 타겟에 데미지를 입힘.
        Debug.Log($"원거리 {_damage} 데미지");

        // TODO : 플레이어 Hp 깎기.
        if(gameObject.TryGetComponent<PlayerCondition>(out PlayerCondition playerCondition))
        {
            playerCondition.UpdateHealth(-_damage);
        }


        // 풀로 반환.
        Main.Pool.Push(this);
    }
}