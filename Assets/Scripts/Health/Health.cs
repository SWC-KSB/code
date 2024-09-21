using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrame")]
    [SerializeField] private float iFrameDuration;  // 무적 시간
    [SerializeField] private int numberOfFlashes;   // 깜박이는 횟수
    private SpriteRenderer spriteRender;
    private bool isInvulnerable = false;            // 무적 상태 플래그

    private void Awake()
    {
        currentHealth = InitialHealth;
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // 데미지 받는 함수
    public void TakeDamage(float damage)
    {
        // 무적 상태가 아닐 때만 데미지 받음
        if (!isInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

            if (currentHealth > 0)
            {
                // 플레이어가 다쳤음
                anim.SetTrigger("Hurt");
                StartCoroutine(Invulnerability());  // 무적 상태 시작
            }
            else
            {
                if (!dead)
                {
                    // 플레이어가 죽었을 때
                    anim.SetTrigger("Die");
                    GetComponent<PlayerMovement>().enabled = false;
                    dead = true;
                }
            }
        }
    }

    // 무적 상태 유지 (데미지 제한, 충돌은 허용)
    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }

        isInvulnerable = false;  // 무적 상태 해제
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
    }
}
