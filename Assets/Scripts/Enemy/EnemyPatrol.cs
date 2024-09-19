using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;  // �� ������ �ʿ� ���� �� ����
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;
    [SerializeField] private Animator anim;

    private Vector3 iniScale;
    private bool movingLeft;
    private float idleTimer;

    private void Awake()
    {
        // enemy�� null���� Ȯ��
        if (enemy == null)
        {
            Debug.LogError("Enemy reference is missing in the EnemyPatrol script.");
            return;
        }

        iniScale = enemy.localScale;
        movingLeft = true;  // �ʱ� ���� ����
    }

    private void Update()
    {
        if (anim.GetBool("IsMoving") == false && idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;  // ��� �ð� ����
            return;
        }

        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x) // ��谪�� ���� �ʵ��� ���� �߰�
                MoveInDirection(-1);
            else
                StartIdle();
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x) // ��谪�� ���� �ʵ��� ���� �߰�
                MoveInDirection(1);
            else
                StartIdle();
        }
    }

    private void StartIdle()
    {
        anim.SetBool("IsMoving", false);  // �̵� �ִϸ��̼� ����
        idleTimer = idleDuration;  // idleTimer�� idleDuration���� �����Ͽ� ��� ����
    }

    private void EndIdleAndChangeDirection()
    {
        movingLeft = !movingLeft;  // ���� ��ȯ
        if (enemy != null)
        {
            enemy.localScale = new Vector3(Mathf.Abs(iniScale.x) * (movingLeft ? -1 : 1), iniScale.y, iniScale.z);  // ���⿡ �°� �� ũ�� ����
        }
        anim.SetBool("IsMoving", true);  // �ִϸ��̼� �ٽ� ����
    }

    private void MoveInDirection(int direction)
    {
        if (idleTimer <= 0)  // ��� �ð��� ������ ���� �̵�
        {
            if (enemy != null)
            {
                enemy.position += new Vector3(Time.deltaTime * direction * speed, 0, 0);  // �� �̵�
            }
        }
        else if (idleTimer <= 0)  // ��� �� �̵� ����
        {
            EndIdleAndChangeDirection();  // ��� ���� �� ���� ��ȯ
        }
    }

    // ���� �ʱ�ȭ�ϴ� �޼���
    public void Initialize(Transform left, Transform right, float spd, float idleDur, Animator animator)
    {
        leftEdge = left;
        rightEdge = right;
        speed = spd;
        idleDuration = idleDur;
        anim = animator;
        // ���� Transform�� ���� �Ҵ����� �ʰ� �� �޼��忡�� ������ �� �ֵ��� ���� ����
    }
}
