using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour
{
    public Vector2 range = new Vector2(5f, 3f); // X��� Y������ �̵��� ����
    public Vector2 speed = new Vector2(2f, 1.5f); // X��� Y�� �̵� �ӵ�
    public float waitTime = 1f; // ������ ���ߴ� �ð�
    private Vector3 startPosition; // �ʱ� ��ġ
    private bool isWaiting = false; // ��� ������ ����
    private bool movingRight = true; // X�� �̵� ����
    private bool movingUp = true; // Y�� �̵� ����

    void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
    }

    void Update()
    {
        if (!isWaiting) // ��� ���� �ƴ� ���� �̵�
        {
            Vector3 newPosition = transform.position;

            // X�� �̵�
            if (range.x > 0)
            {
                if (movingRight)
                {
                    newPosition.x += speed.x * Time.deltaTime;
                    if (newPosition.x >= startPosition.x + range.x)
                    {
                        movingRight = false; // X�� ���� ��ȯ
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
                else
                {
                    newPosition.x -= speed.x * Time.deltaTime;
                    if (newPosition.x <= startPosition.x - range.x)
                    {
                        movingRight = true; // X�� ���� ��ȯ
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
            }
            // Y�� �̵�
            if (range.y>0)
            {
                if (movingUp)
                {
                    newPosition.y += speed.y * Time.deltaTime;
                    if (newPosition.y >= startPosition.y + range.y)
                    {
                        movingUp = false; // Y�� ���� ��ȯ
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
                else
                {
                    newPosition.y -= speed.y * Time.deltaTime;
                    if (newPosition.y <= startPosition.y - range.y)
                    {
                        movingUp = true; // Y�� ���� ��ȯ
                        StartCoroutine(WaitAndChangeDirection());
                    }
                }
            }

            transform.position = newPosition; // �� ��ġ ����
        }
    }

    private IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true; // ��� ���·� ����
        yield return new WaitForSeconds(waitTime); // waitTime ���� ���
        isWaiting = false; // ��� ���� �� �ٽ� ������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // �÷��̾ �ڽ����� ����
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // �÷��̾ ������ �� �ڽ� ���� ����
        }
    }
}
