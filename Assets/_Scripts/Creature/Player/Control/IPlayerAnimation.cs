// 이동 전략 인터페이스
using UnityEngine;

public interface IPlayerAnimation
{
    void Move(PlayerInputController player, Animator animator);
}

// 걷기 전략
public class WalkingMovement : IPlayerAnimation
{
    public void Move(PlayerInputController player, Animator animator)
    {
        animator.SetBool("IsWalking", true);
    }
}

// 달리기 전략
public class RunningMovement : IPlayerAnimation
{
    public void Move(PlayerInputController player, Animator animator)
    {
        animator.SetBool("IsRunning", true);
    }
}