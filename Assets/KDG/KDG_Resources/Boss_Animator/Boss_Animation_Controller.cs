using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;

    // �ִϸ��̼� Ʈ����
    private static readonly int ChangeAnimationTrigger = Animator.StringToHash("ChangeAnimation");

    private void Start()
    {
        animator = GetComponent<Animator>();
        // �ʱ� �ִϸ��̼� ���� (��: Idle)
        animator.SetTrigger(ChangeAnimationTrigger);
    }

    public void BossAnimationChange()
    {
        // �ִϸ��̼� ���� (1�� ����)
        animator.SetTrigger(ChangeAnimationTrigger);
        // Idle �ִϸ��̼� ��� ����
        InvokeRepeating("PlayIdleAnimation", 0f, 0.1f);
    }

    private void PlayIdleAnimation()
    {
        // Idle �ִϸ��̼� ���
        animator.SetTrigger("Idle");
    }
}
