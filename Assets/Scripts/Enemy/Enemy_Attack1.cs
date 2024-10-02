using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack1 : MonoBehaviour
{
    // 데미지 값 설정
    public int damageAmount = 10;

    // 플레이어와 충돌했을 때 호출되는 함수
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
