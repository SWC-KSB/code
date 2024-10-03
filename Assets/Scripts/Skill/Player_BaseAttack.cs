using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BaseAttack : MonoBehaviour
{
    public Animator animator;  // ���� �ִϸ��̼��� ���� Animator ����
    public float attackCooldown = 0.5f;  // ���� ���� ��Ÿ�� ����
    public float attackRange = 1.5f;     // ���� ���� ����
    public int attackDamage = 10;        // ���� ������ ����
    public LayerMask enemyLayer;         // �� ���̾� ����
    public AudioClip attackSound;        // ���� ȿ���� �߰�
    private AudioSource audioSource;     // AudioSource ���� �߰�

    private bool isAttacking = false;  // ���� ������ Ȯ���ϴ� ����

    void Start()
    {
        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)  // ��Ŭ���ϸ� ���� �ߵ�
        {
            Attack();  // ���� �Լ� ȣ��
        }
    }

    void Attack()
    {
        isAttacking = true;  // ���� ������ ����
        animator.SetTrigger("Base_Attack");  // ���� �ִϸ��̼� ����

        // ���� ȿ���� ���
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // ������ ������ ���ϱ�
        PerformAttack();

        Invoke("ResetAttack", attackCooldown);  // ��Ÿ�� �� ���� �ʱ�ȭ ȣ��
    }


    void PerformAttack()
    {
        // ���� ���� ���� ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // ������ ������ �������� ���ϱ�
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
            BossStateMachine bossStateMachine = enemy.GetComponent<BossStateMachine>(); // BossStateMachine �߰�

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);  // ������ �������� ���ϱ�
            }
            else if (bossStateMachine != null)
            {
                bossStateMachine.TakeDamage(attackDamage); // BossStateMachine���� �������� ���ϱ�
            }
        }
    }


    void ResetAttack()
    {
        isAttacking = false;  // ���� ���� ���·� ���ư�
    }

    // ���� ������ Gizmo�� �ð������� Ȯ���� �� �ֵ��� �߰�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // ���� ���� �ð�ȭ
    }
}
