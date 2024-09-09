using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Field
    private Animator _playerAnimator;
    #endregion
    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
    }


    public void JumpAnimation()
    {
        _playerAnimator.SetTrigger("IsJump");
    }
    public void AttackAnimation()
    {
        _playerAnimator.SetTrigger("IsAttack");
    }
    public void WalkAnimation(bool IsWalk)
    {
        _playerAnimator.SetBool("IsWalk", IsWalk);
    }
    public void RunAnimation(bool IsRun)
    {
        _playerAnimator.SetBool("IsWalk", IsRun);
    }
    public void CrouchIdleAnimation(bool IsSit)
    {
        _playerAnimator.SetBool("IsSit", IsSit);
    }
    public void CrouchWalkAnimation(bool IsSitWalk)
    {
        _playerAnimator.SetBool("IsSitWalk", IsSitWalk);
    }

}
