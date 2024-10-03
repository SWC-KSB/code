using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int maxHealth = 100;   // 적의 최대 체력
    private int currentHealth;    // 적의 현재 체력

    public AudioClip damageSound; // 데미지 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Start()
    {
        // 적의 체력을 최대 체력으로 초기화
        currentHealth = maxHealth;

        // AudioSource 컴포넌트 가져오기 또는 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 데미지를 받는 함수
    public void TakeDamage(int damage)
    {
        // 데미지만큼 체력을 감소
        currentHealth -= damage;
        Debug.Log("적이 " + damage + " 만큼의 데미지를 받음. 현재 체력: " + currentHealth);

        // 데미지 효과음 재생
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // 체력이 0 이하가 되면 적이 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 적이 사망했을 때 호출되는 함수
    private void Die()
    {
        Debug.Log("적이 사망했습니다!");

        // 적을 사망 처리 (예: 게임 오브젝트 삭제)
        Destroy(gameObject);
    }
}
