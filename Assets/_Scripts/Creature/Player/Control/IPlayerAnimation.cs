using UnityEngine;

public interface IPlayerAnimation
{
    void AnimationSet(Animator animator);
}

// 평시
public class IdleMovement : IPlayerAnimation
{
    public void AnimationSet(Animator animator)
    {
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsSit", false);
    }
}

// 걷기
public class WalkingMovement : IPlayerAnimation
{
    public void AnimationSet(Animator animator)
    {
        animator.SetBool("IsWalk", true);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsSit", false);
    }
}

// 달리기
public class RunningMovement : IPlayerAnimation
{
    public void AnimationSet(Animator animator)
    {
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", true);
        animator.SetBool("IsSit", false);
    }
}

//앉기
public class SitMovement : IPlayerAnimation
{
    public void AnimationSet(Animator animator)
    {
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsSit", true);
    }
}

//앉은상태에서 걷기
public class SitWalkMovement : IPlayerAnimation
{
    public void AnimationSet(Animator animator)
    {
        animator.SetBool("IsWalk", true);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsSit", true);
    }
}