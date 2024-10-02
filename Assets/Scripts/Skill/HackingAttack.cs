using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingAttackPrefab : MonoBehaviour
{
    public float radius = 5f; // 광역 공격 범위
    public float damage = 10f; // 공격 데미지
    public float cooldownTime = 2f; // 공격 쿨타임 (초 단위)
    private float nextAttackTime = 0f; // 다음 공격이 가능한 시간

    public Animator animator; // 애니메이터 컴포넌트 (선택 사항)

    void Update()
    {
        // 공격 쿨타임 확인 및 E 키로 공격 실행
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime)
        {
            PerformHackingAttack();
            nextAttackTime = Time.time + cooldownTime; // 다음 공격 시간 설정
        }
    }

    void PerformHackingAttack()
    {
        // 애니메이터 트리거 실행 (선택 사항)
        if (animator != null)
        {
            animator.SetTrigger("HackingAttack");
        }

        // "Enemy" 태그를 가진 모든 적 오브젝트를 찾음
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 감지된 적 오브젝트가 없는 경우 처리
        if (enemies.Length == 0)
        {
            return;
        }

        // 각 적 오브젝트에 대해 범위 확인
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                continue; // 만약 적이 null이면 넘어감
            }

            // 적이 범위 안에 있는지 확인
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= radius)
            {
                // 적에게 데미지를 가하기
                Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                BossStateMachine bossStateMachine = enemy.GetComponent<BossStateMachine>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else if (bossStateMachine != null)
                {
                    bossStateMachine.TakeDamage(damage); // BossStateMachine에게 데미지를 가하기
                }
            }
        }
    }

    // 공격 범위를 Gizmo로 시각적으로 확인 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius); // 범위 시각화
    }
}
