using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BaseAttack : MonoBehaviour
{
    public Animator animator;  // 공격 애니메이션을 위한 Animator 변수다긔
    public float attackCooldown = 0.5f;  // 공격 사이 쿨타임 설정이긔
    public float attackRange = 1.5f;     // 공격 범위 설정이긔
    public int attackDamage = 10;        // 공격 데미지 설정이긔
    public LayerMask enemyLayer;         // 적 레이어 설정이긔

    private bool isAttacking = false;  // 공격 중인지 확인하는 변수다긔

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)  // 좌클릭하면 공격 발동이긔
        {
            Attack();  // 공격 함수 호출이긔
        }
    }

    void Attack()
    {
        isAttacking = true;  // 공격 중으로 변경하긔
        animator.SetTrigger("Base_Attack");  // 공격 애니메이션 실행하긔

        // 적에게 데미지 가하기
        PerformAttack();

        Invoke("ResetAttack", attackCooldown);  // 쿨타임 후 공격 초기화 호출하긔
    }

    void PerformAttack()
    {
        // 공격 범위 내의 적을 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // 감지된 적에게 데미지를 가하기
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
            BossStateMachine bossStateMachine = enemy.GetComponent<BossStateMachine>(); // BossStateMachine 추가

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);  // 적에게 데미지를 가하기
            }
            else if (bossStateMachine != null)
            {
                bossStateMachine.TakeDamage(attackDamage); // BossStateMachine에게 데미지를 가하기
            }
        }
    }


    void ResetAttack()
    {
        isAttacking = false;  // 공격 가능 상태로 돌아오긔
    }

    // 공격 범위를 Gizmo로 시각적으로 확인할 수 있도록 추가하긔
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // 공격 범위 시각화하긔
    }
}
