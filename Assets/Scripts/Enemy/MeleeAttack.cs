using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
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


    // Knight의 시야에 보이면 Knight가 공격함 
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if(cooldownTimer >= attackCooldown)
            {
                // 공격
                cooldownTimer = 0;
                anim.SetTrigger("MeleeAttack");
            }
        }
    }

    // Knight 시야
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range *
            transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left,0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range *
            transform.localScale.x * colliderDistance, new Vector2(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}