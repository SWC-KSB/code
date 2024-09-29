using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    // ���� ������
    [SerializeField] private int damageAmount = 10;

    // �÷��̾�� �浹���� �� ���� ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ����� �÷��̾��� ���� �������� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            // �÷��̾��� Health ������Ʈ�� ���� �� ������ ó��
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player hit by enemy. Damage applied.");
            }
        }
    }
}
