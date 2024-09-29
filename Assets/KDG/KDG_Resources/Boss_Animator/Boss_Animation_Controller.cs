using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;

    // 애니메이션 트리거
    private static readonly int ChangeAnimationTrigger = Animator.StringToHash("ChangeAnimation");

    private void Start()
    {
        animator = GetComponent<Animator>();
        // 초기 애니메이션 설정 (예: Idle)
        animator.SetTrigger(ChangeAnimationTrigger);
    }

    public void BossAnimationChange()
    {
        // 애니메이션 변경 (1번 실행)
        animator.SetTrigger(ChangeAnimationTrigger);
        // Idle 애니메이션 계속 실행
        InvokeRepeating("PlayIdleAnimation", 0f, 0.1f);
    }

    private void PlayIdleAnimation()
    {
        // Idle 애니메이션 재생
        animator.SetTrigger("Idle");
    }
}
