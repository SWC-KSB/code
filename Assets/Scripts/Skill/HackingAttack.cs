using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingAttackPrefab : MonoBehaviour
{
    public float radius = 5f; // ���� ���� ����
    public float damage = 10f; // ���� ������
    public float cooldownTime = 2f; // ���� ��Ÿ�� (�� ����)
    private float nextAttackTime = 0f; // ���� ������ ������ �ð�

    public Animator animator; // �ִϸ����� ������Ʈ (���� ����)

    void Update()
    {
        // ���� ��Ÿ�� Ȯ�� �� E Ű�� ���� ����
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime)
        {
            PerformHackingAttack();
            nextAttackTime = Time.time + cooldownTime; // ���� ���� �ð� ����
        }
    }

    void PerformHackingAttack()
    {
        // �ִϸ����� Ʈ���� ���� (���� ����)
        if (animator != null)
        {
            animator.SetTrigger("HackingAttack");
        }

        // "Enemy" �±׸� ���� ��� �� ������Ʈ�� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ������ �� ������Ʈ�� ���� ��� ó��
        if (enemies.Length == 0)
        {
            return;
        }

        // �� �� ������Ʈ�� ���� ���� Ȯ��
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                continue; // ���� ���� null�̸� �Ѿ
            }

            // ���� ���� �ȿ� �ִ��� Ȯ��
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= radius)
            {
                // ������ �������� ���ϱ�
                Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                BossStateMachine bossStateMachine = enemy.GetComponent<BossStateMachine>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else if (bossStateMachine != null)
                {
                    bossStateMachine.TakeDamage(damage); // BossStateMachine���� �������� ���ϱ�
                }
            }
        }
    }

    // ���� ������ Gizmo�� �ð������� Ȯ�� (������)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius); // ���� �ð�ȭ
    }
}
