using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("ü��")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Hit Effect")]
    [SerializeField] private int numberOfFlashes = 3;  // �����̴� Ƚ��
    [SerializeField] private float flashDuration = 0.1f;  // �����̴� �ӵ�
    private SpriteRenderer spriteRender;

    [Header("ü�¹�")]
    public Enemy_Healthbar healthBar; // ���� ü�¹ٸ� ���� ����

    private void Awake()
    {
        currentHealth = InitialHealth;
        spriteRender = GetComponent<SpriteRenderer>();

        // Animator �ʱ�ȭ
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator component not found on this game object.");
        }

        // ü�¹� �ʱ�ȭ: healthBar�� null�� �ƴ� ���� �ʱ�ȭ
        if (healthBar != null)
        {
            healthBar.Initialize(InitialHealth);  // ü�¹� �ʱ�ȭ
        }
        else
        {
            Debug.LogWarning("Health bar not assigned in the inspector.");
        }
    }

    // ������ �޴� �Լ�
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);

        // ü�¹ٰ� null�� �ƴ� ���� ���̰� ����
        if (healthBar != null)
        {
            healthBar.ShowHealthBar();
        }

        // Animator�� null�� �ƴ� ���� �ִϸ��̼� Ʈ����
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

        Debug.Log("Current Health: " + currentHealth);  // ü�� �α� ���

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth);  // ü�¹� ������Ʈ
        }

        if (currentHealth > 0)
        {
            StartCoroutine(FlashRed());  // �����Ÿ��� ȿ��
        }
        else
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(Die());
            }
        }

        // �� �� �ڿ� ü�¹� �����
        StartCoroutine(HideHealthBarAfterDelay());
    }

    // ���� ���������� �����Ÿ��� ȿ��
    private IEnumerator FlashRed()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 1);  // ���������� ����
            yield return new WaitForSeconds(flashDuration);  // �����̴� �ð�
            spriteRender.color = Color.white;  // ���� ������ ���ƿ�
            yield return new WaitForSeconds(flashDuration);  // �����̴� �ð�
        }
    }

    // �״� �Լ�
    private IEnumerator Die()
    {
        // ���� ���� �� �ִϸ��̼��� ���� ���
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        yield return new WaitForSeconds(0.5f);  // ��� ��� �� ������Ʈ �ı�
        Destroy(gameObject); // ������Ʈ ����
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth); // ü�� �߰��� ü�¹� ������Ʈ
        }
    }

    // ü�¹ٸ� ���� �ð��� ���� �� ����� �Լ�
    private IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(2f); // 2�� �� ü�¹� �����
        if (healthBar != null)
        {
            healthBar.HideHealthBar();
        }
    }
}
