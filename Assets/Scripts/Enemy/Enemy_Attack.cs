using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    // 공격 쿨타임
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


    // 드론의 시야에 보이면 드론이 공격함 
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                // 공격
                cooldownTimer = 0;
                anim.SetTrigger("Dron_Attack");
            }
        }
    }

    // Knight 시야
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // 아래쪽으로 이동
            new Vector3(boxCollider.bounds.size.y * range, boxCollider.bounds.size.x, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            // 플레이어인지 확인 (태그로 비교)
            if (hit.collider.CompareTag("Player"))
            {
                playerHealth = hit.transform.GetComponent<Health>();
                return true;  // 플레이어가 맞다면 true 반환
            }
        }
        return false;  // 플레이어가 아니면 false 반환
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // 아래쪽으로 이동
            new Vector2(boxCollider.bounds.size.y * range, boxCollider.bounds.size.x));
    }


    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }

    // 데미지 값 설정
    public int damageAmount = 10;

    // 플레이어와 충돌했을 때 호출되는 함수
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

