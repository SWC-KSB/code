using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    // ���� ��Ÿ��
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    // ����� �þ߿� ���̸� ����� ������ 
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                // ����
                cooldownTimer = 0;
                anim.SetTrigger("Dron_Attack");
            }
        }
    }

    // Knight �þ�
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // �Ʒ������� �̵�
            new Vector3(boxCollider.bounds.size.y * range, boxCollider.bounds.size.x, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            // �÷��̾����� Ȯ�� (�±׷� ��)
            if (hit.collider.CompareTag("Player"))
            {
                playerHealth = hit.transform.GetComponent<Health>();
                return true;  // �÷��̾ �´ٸ� true ��ȯ
            }
        }
        return false;  // �÷��̾ �ƴϸ� false ��ȯ
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // �Ʒ������� �̵�
            new Vector2(boxCollider.bounds.size.y * range, boxCollider.bounds.size.x));
    }


    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }

    // ������ �� ����
    public int damageAmount = 10;

    // �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}

