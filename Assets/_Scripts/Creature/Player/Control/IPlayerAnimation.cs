// 이동 전략 인터페이스
using UnityEngine;

public interface IPlayerAnimation
{
    void AnimationSet(PlayerInputController player, Animator animator);
}

// 걷기 전략
public class WalkingMovement : IPlayerAnimation
{
    public void AnimationSet(PlayerInputController player, Animator animator)
    {
        animator.SetBool("IsWalk", true);
    }
}

// 달리기 전략
public class RunningMovement : IPlayerAnimation
{
    public void AnimationSet(PlayerInputController player, Animator animator)
    {
        animator.SetBool("IsRun", true);
    }
}

public class SitMovement : IPlayerAnimation
{
    public void AnimationSet(PlayerInputController player, Animator animator)
    {
        animator.SetBool("IsSit", true);
    }
}