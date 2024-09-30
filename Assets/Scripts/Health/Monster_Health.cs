using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Health : MonoBehaviour
{
    [Header("체력")]
    // 초기 체력
    [SerializeField] private float InitialHealth;
    // get : currenthealth 정보값을 가져와서 보여줄 수 있다.
    // private set은 health 컴포넌트를 가지고 있는 오브젝트에서만 정보를 변경할 수 있다.
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrame")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        currentHealth = InitialHealth;
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // 데미지 받는 함수
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

        if (currentHealth > 0)
        {
            // 플레이어가 다쳤음
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());

        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");
                GetComponent<EnemyPatrol>().enabled = false;
                dead = true;
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
    }
    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        // 취약 지속시간? 
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
