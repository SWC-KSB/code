using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HackingAttack : MonoBehaviour
{
    public float radius = 5f; // ���� ���� ����
    public float damage = 10f; // ���� ������
    public LayerMask enemyLayer; // ���� ���� ���̾�
    public Animator animator; // �ִϸ����� ������Ʈ

    void Update()
    {
        // ���� �Է� (��: ���콺 ������ Ŭ��)
        if (Input.GetKeyDown(KeyCode.E)) // ���콺 ������ Ŭ������ ����
        {
            PerformHackingAttack();
        }
    }

    void PerformHackingAttack()
    {
        // �ִϸ��̼� Ʈ���� ����
        animator.SetTrigger("HackingAttack");

        // ���� ��ġ�� �߽����� ������ �ݰ� ���� ��� �ݶ��̴� ����
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);


    }


}

