using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    // 공격 데미지
    [SerializeField] private int damageAmount = 10;

    // 공격 효과음
    public AudioClip attackSound;
    private AudioSource audioSource;

    private void Start()
    {
        // AudioSource 컴포넌트를 가져오기 또는 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 플레이어와 충돌했을 때 공격 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 대상이 플레이어일 때만 데미지를 입힘
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            // 플레이어의 Health 컴포넌트가 있을 때 데미지 처리
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player hit by enemy. Damage applied.");

                // 공격 소리 재생
                if (attackSound != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }
            }
        }
    }
}
