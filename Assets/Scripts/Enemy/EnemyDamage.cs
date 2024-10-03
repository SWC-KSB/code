using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int maxHealth = 100;   // ���� �ִ� ü��
    private int currentHealth;    // ���� ���� ü��

    public AudioClip damageSound; // ������ ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private void Start()
    {
        // ���� ü���� �ִ� ü������ �ʱ�ȭ
        currentHealth = maxHealth;

        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �������� �޴� �Լ�
    public void TakeDamage(int damage)
    {
        // ��������ŭ ü���� ����
        currentHealth -= damage;
        Debug.Log("���� " + damage + " ��ŭ�� �������� ����. ���� ü��: " + currentHealth);

        // ������ ȿ���� ���
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // ü���� 0 ���ϰ� �Ǹ� ���� ���
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ���� ������� �� ȣ��Ǵ� �Լ�
    private void Die()
    {
        Debug.Log("���� ����߽��ϴ�!");

        // ���� ��� ó�� (��: ���� ������Ʈ ����)
        Destroy(gameObject);
    }
}
