using UnityEngine;

public class GrandfatherMovement2D : MonoBehaviour
{
    public Transform player;  // 플레이어의 위치
    public float speed = 2.0f;  // 할아버지 이동 속도
    private Animator grandfaanim;

    private bool isWalking = false;  // 할아버지가 걷기 시작하는지 여부
    public float stopDistance = 2.0f;  // 플레이어보다 얼마나 떨어져 멈출지

    void Start()
    {
        grandfaanim = GetComponent<Animator>();  // 할아버지의 Animator 컴포넌트 가져오기
    }

    void Update()
    {
        if (isWalking)
        {
            // 플레이어와의 방향 계산 (2D 이동)
            Vector3 direction = (player.position - transform.position).normalized;

            // 플레이어와 일정 거리 이상 남아 있을 때만 이동
            if (Vector3.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                StopWalking();  // 목표 위치에 도달하면 걷기 중지
            }
        }
    }

    // 할아버지가 걷기 시작하는 함수
    public void StartWalking()
    {
        grandfaanim.SetBool("isWalking", true);  // 걷기 애니메이션 시작
        isWalking = true;  // 걷기 시작
    }

    // 할아버지가 멈추는 함수
    public void StopWalking()
    {
        grandfaanim.SetBool("isWalking", false);  // 걷기 애니메이션 중지
        isWalking = false;  // 걷기 중지
    }
}
