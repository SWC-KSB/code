using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("ü��")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("Hit Effect")]
    [SerializeField] private int numberOfFlashes = 3;  // �����̴� Ƚ��
    [SerializeField] private float flashDuration = 0.1f;  // �����̴� �ӵ�
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        currentHealth = InitialHealth;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // ������ �޴� �Լ�
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

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
        // ���� ���� �� �ִϸ��̼� ���� ��� ��� �� �ı�
        yield return new WaitForSeconds(0.5f);  // ��� ��� �� ������Ʈ �ı�
        Destroy(gameObject); // ������Ʈ ����
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
    }
}
