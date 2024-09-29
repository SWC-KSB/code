using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Hit Effect")]
    [SerializeField] private int numberOfFlashes = 3;  // 깜빡이는 횟수
    [SerializeField] private float flashDuration = 0.1f;  // 깜빡이는 속도
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        currentHealth = InitialHealth;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // 데미지 받는 함수
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

        if (currentHealth > 0)
        {
            StartCoroutine(FlashRed());  // 깜빡거리는 효과
        }
        else
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(Die());
            }
        }
    }

    // 적이 빨간색으로 깜빡거리는 효과
    private IEnumerator FlashRed()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 1);  // 빨간색으로 변경
            yield return new WaitForSeconds(flashDuration);  // 깜빡이는 시간
            spriteRender.color = Color.white;  // 원래 색으로 돌아옴
            yield return new WaitForSeconds(flashDuration);  // 깜빡이는 시간
        }
    }

    // 죽는 함수
    private IEnumerator Die()
    {
        // 적이 죽을 때 애니메이션 없이 잠시 대기 후 파괴
        yield return new WaitForSeconds(0.5f);  // 잠시 대기 후 오브젝트 파괴
        Destroy(gameObject); // 오브젝트 삭제
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
    }
}
