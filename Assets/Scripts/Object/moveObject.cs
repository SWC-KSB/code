using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour
{
    public Vector2 range = new Vector2(5f, 3f); // X축과 Y축으로 이동할 범위
    public Vector2 speed = new Vector2(2f, 1.5f); // X축과 Y축 이동 속도
    public float waitTime = 1f; // 끝에서 멈추는 시간
    private Vector3 startPosition; // 초기 위치
    private bool isWaiting = false; // 대기 중인지 여부
    private bool movingRight = true; // X축 이동 방향
    private bool movingUp = true; // Y축 이동 방향

    void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        if (!isWaiting) // 대기 중이 아닐 때만 이동
        {
            Vector3 newPosition = transform.position;

            // X축 이동
            if (range.x > 0)
            {
                if (movingRight)
                {
                    newPosition.x += speed.x * Time.deltaTime;
                    if (newPosition.x >= startPosition.x + range.x)
                    {
                        movingRight = false; // X축 방향 전환
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
                else
                {
                    newPosition.x -= speed.x * Time.deltaTime;
                    if (newPosition.x <= startPosition.x - range.x)
                    {
                        movingRight = true; // X축 방향 전환
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
            }
            // Y축 이동
            if (range.y>0)
            {
                if (movingUp)
                {
                    newPosition.y += speed.y * Time.deltaTime;
                    if (newPosition.y >= startPosition.y + range.y)
                    {
                        movingUp = false; // Y축 방향 전환
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
                else
                {
                    newPosition.y -= speed.y * Time.deltaTime;
                    if (newPosition.y <= startPosition.y - range.y)
                    {
                        movingUp = true; // Y축 방향 전환
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
            }

            transform.position = newPosition; // 새 위치 적용
        }
    }

    private IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true; // 대기 상태로 변경
        yield return new WaitForSeconds(waitTime); // waitTime 동안 대기
        isWaiting = false; // 대기 종료 후 다시 움직임
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // 플레이어를 자식으로 설정
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // 플레이어가 내렸을 때 자식 관계 해제
        }
    }
}
