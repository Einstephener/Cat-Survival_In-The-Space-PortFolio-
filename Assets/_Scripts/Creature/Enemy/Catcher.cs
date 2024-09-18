using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : Enemy
{
    [SerializeField] public Transform basePosition;

    private Transform _playerPosition;
    private AIDestinationSetter _target;

    private float _sightRange = 7f;
    private float _attackRange = 4f;

    private EnemyState enemyState;

    private void Awake()
    {
        _target = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        enemyState = EnemyState.Idle;
        //TransitionToState(new EnemyIdleState());
    }

    private void Update()
    {
        //UpdateState();
        if (_playerPosition == null)
        {
            _target.target = basePosition;
        }

        // 현재 상태를 매 프레임마다 업데이트
        //switch (enemyState)
        //{
        //    case EnemyState.Idle:
        //        // Idle 행동 구현.
        //        if (FindTarget())
        //        {
        //            GetTarget();
        //            TransitionToState(new EnemyChaseState());
        //        }
        //        else
        //        {
        //            // 감지된 플레이어가 없으면 추격을 멈춤.
        //            enemyState = EnemyState.Idle;
        //            _target.target = null;
        //            _playerPosition = null;
        //            _aiPath.canMove = true;
        //        }
        //        break;
        //    case EnemyState.Prowl:
        //        // Prowl 행동 구현.

        //        break;
        //    case EnemyState.Walking:
        //        // Walking 행동 구현.
        //        //_animator.SetFloat("Speed", _aiPath.maxSpeed);
        //        SetSpeed(2f);
        //        break;
        //    case EnemyState.Chasing:
        //        // Chasing 행동 구현.
        //        //_animator.SetFloat("Speed", _aiPath.maxSpeed);
        //        SetSpeed(4f);
        //        break;
        //    case EnemyState.Hit:
        //        // Hit 행동 구현.

        //        break;
        //    case EnemyState.Attack:
        //        // Attack 행동 구현.
        //        //_animator.SetTrigger("OnAttack");
        //        break;
        //}
    }

    #region Gizmos
    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
    #endregion
}
