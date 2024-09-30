using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Health : MonoBehaviour
{
    [Header("ü��")]
    // �ʱ� ü��
    [SerializeField] private float InitialHealth;
    // get : currenthealth �������� �����ͼ� ������ �� �ִ�.
    // private set�� health ������Ʈ�� ������ �ִ� ������Ʈ������ ������ ������ �� �ִ�.
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

    // ������ �޴� �Լ�
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

        if (currentHealth > 0)
        {
            // �÷��̾ ������
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
        // ��� ���ӽð�? 
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
