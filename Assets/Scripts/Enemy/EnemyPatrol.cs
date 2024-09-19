using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;  // 이 변수는 필요 없을 수 있음
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;
    [SerializeField] private Animator anim;

    private Vector3 iniScale;
    private bool movingLeft;
    private float idleTimer;

    private void Awake()
    {
        // enemy가 null인지 확인
        if (enemy == null)
        {
            Debug.LogError("Enemy reference is missing in the EnemyPatrol script.");
            return;
        }

        iniScale = enemy.localScale;
        movingLeft = true;  // 초기 방향 설정
    }

    private void Update()
    {
        if (anim.GetBool("IsMoving") == false && idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;  // 대기 시간 감소
            return;
        }

        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x) // 경계값을 넘지 않도록 조건 추가
                MoveInDirection(-1);
            else
                StartIdle();
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x) // 경계값을 넘지 않도록 조건 추가
                MoveInDirection(1);
            else
                StartIdle();
        }
    }

    private void StartIdle()
    {
        anim.SetBool("IsMoving", false);  // 이동 애니메이션 중지
        idleTimer = idleDuration;  // idleTimer를 idleDuration으로 설정하여 대기 시작
    }

    private void EndIdleAndChangeDirection()
    {
        movingLeft = !movingLeft;  // 방향 전환
        if (enemy != null)
        {
            enemy.localScale = new Vector3(Mathf.Abs(iniScale.x) * (movingLeft ? -1 : 1), iniScale.y, iniScale.z);  // 방향에 맞게 적 크기 조정
        }
        anim.SetBool("IsMoving", true);  // 애니메이션 다시 시작
    }

    private void MoveInDirection(int direction)
    {
        if (idleTimer <= 0)  // 대기 시간이 끝났을 때만 이동
        {
            if (enemy != null)
            {
                enemy.position += new Vector3(Time.deltaTime * direction * speed, 0, 0);  // 적 이동
            }
        }
        else if (idleTimer <= 0)  // 대기 후 이동 시작
        {
            EndIdleAndChangeDirection();  // 대기 끝난 후 방향 전환
        }
    }

    // 적을 초기화하는 메서드
    public void Initialize(Transform left, Transform right, float spd, float idleDur, Animator animator)
    {
        leftEdge = left;
        rightEdge = right;
        speed = spd;
        idleDuration = idleDur;
        anim = animator;
        // 적의 Transform을 직접 할당하지 않고 이 메서드에서 참조할 수 있도록 수정 가능
    }
}
