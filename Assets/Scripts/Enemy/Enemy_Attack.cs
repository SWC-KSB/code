using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    // ���� ������
    [SerializeField] private int damageAmount = 10;

    // ���� ȿ����
    public AudioClip attackSound;
    private AudioSource audioSource;

    private void Start()
    {
        // AudioSource ������Ʈ�� �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �÷��̾�� �浹���� �� ���� ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ����� �÷��̾��� ���� �������� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            // �÷��̾��� Health ������Ʈ�� ���� �� ������ ó��
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Player hit by enemy. Damage applied.");

                // ���� �Ҹ� ���
                if (attackSound != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }
            }
        }
    }
}
