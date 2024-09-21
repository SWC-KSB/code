using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack1 : MonoBehaviour
{
    // ������ �� ����
    public int damageAmount = 10;

    // �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ENTER");
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
