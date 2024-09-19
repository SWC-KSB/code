using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HackingAttack : MonoBehaviour
{
    public float radius = 5f; // 광역 공격 범위
    public float damage = 10f; // 공격 데미지
    public LayerMask enemyLayer; // 적이 속한 레이어
    public Animator animator; // 애니메이터 컴포넌트

    void Update()
    {
        // 공격 입력 (예: 마우스 오른쪽 클릭)
        if (Input.GetKeyDown(KeyCode.E)) // 마우스 오른쪽 클릭으로 공격
        {
            PerformHackingAttack();
        }
    }

    void PerformHackingAttack()
    {
        // 애니메이션 트리거 실행
        animator.SetTrigger("HackingAttack");

        // 현재 위치를 중심으로 설정된 반경 내의 모든 콜라이더 감지
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);


    }


}

