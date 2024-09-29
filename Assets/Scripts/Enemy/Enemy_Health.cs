using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Hit Effect")]
    [SerializeField] private int numberOfFlashes = 3;  // 깜빡이는 횟수
    [SerializeField] private float flashDuration = 0.1f;  // 깜빡이는 속도
    private SpriteRenderer spriteRender;

    [Header("체력바")]
    public Enemy_Healthbar healthBar; // 적의 체력바를 직접 참조

    private void Awake()
    {
        currentHealth = InitialHealth;
        spriteRender = GetComponent<SpriteRenderer>();

        // Animator 초기화
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator component not found on this game object.");
        }

        // 체력바 초기화: healthBar가 null이 아닐 때만 초기화
        if (healthBar != null)
        {
            healthBar.Initialize(InitialHealth);  // 체력바 초기화
        }
        else
        {
            Debug.LogWarning("Health bar not assigned in the inspector.");
        }
    }

    // 데미지 받는 함수
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);

        // 체력바가 null이 아닐 때만 보이게 설정
        if (healthBar != null)
        {
            healthBar.ShowHealthBar();
        }

        // Animator가 null이 아닐 때만 애니메이션 트리거
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

        Debug.Log("Current Health: " + currentHealth);  // 체력 로그 출력

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth);  // 체력바 업데이트
        }

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

        // 몇 초 뒤에 체력바 숨기기
        StartCoroutine(HideHealthBarAfterDelay());
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
        // 적이 죽을 때 애니메이션이 있을 경우
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        yield return new WaitForSeconds(0.5f);  // 잠시 대기 후 오브젝트 파괴
        Destroy(gameObject); // 오브젝트 삭제
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth); // 체력 추가시 체력바 업데이트
        }
    }

    // 체력바를 일정 시간이 지난 후 숨기는 함수
    private IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(2f); // 2초 뒤 체력바 숨기기
        if (healthBar != null)
        {
            healthBar.HideHealthBar();
        }
    }
}
