using UnityEngine;

public class GrandfatherMovement2D : MonoBehaviour
{
    public Transform player;  // �÷��̾��� ��ġ
    public float speed = 2.0f;  // �Ҿƹ��� �̵� �ӵ�
    private Animator grandfaanim;

    private bool isWalking = false;  // �Ҿƹ����� �ȱ� �����ϴ��� ����
    public float stopDistance = 2.0f;  // �÷��̾�� �󸶳� ������ ������

    void Start()
    {
        grandfaanim = GetComponent<Animator>();  // �Ҿƹ����� Animator ������Ʈ ��������
    }

    void Update()
    {
        if (isWalking)
        {
            // �÷��̾���� ���� ��� (2D �̵�)
            Vector3 direction = (player.position - transform.position).normalized;

            // �÷��̾�� ���� �Ÿ� �̻� ���� ���� ���� �̵�
            if (Vector3.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                StopWalking();  // ��ǥ ��ġ�� �����ϸ� �ȱ� ����
            }
        }
    }

    // �Ҿƹ����� �ȱ� �����ϴ� �Լ�
    public void StartWalking()
    {
        grandfaanim.SetBool("isWalking", true);  // �ȱ� �ִϸ��̼� ����
        isWalking = true;  // �ȱ� ����
    }

    // �Ҿƹ����� ���ߴ� �Լ�
    public void StopWalking()
    {
        grandfaanim.SetBool("isWalking", false);  // �ȱ� �ִϸ��̼� ����
        isWalking = false;  // �ȱ� ����
    }
}
