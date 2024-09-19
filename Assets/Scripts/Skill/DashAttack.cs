using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;         // 대쉬 속도
    public float dashDuration = 0.2f;     // 대쉬 지속 시간
    public float dashCooldown = 1f;       // 대쉬 쿨타임
    public LayerMask enemyLayer;          // 적 Layer 설정
    public float attackRange = 1f;        // 공격 범위
    public int attackDamage = 10;         // 공격 데미지

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTime = 0f;
    private Vector2 dashDirection;
    private Animator animator;            // 애니메이터 추가
    public GameObject Player;
    PlayerMovement PlayerMovement;
    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // 애니메이터 컴포넌트 가져오기
    }

    private void Update()
    {
        // 대쉬를 위한 입력 처리
        if (Input.GetKeyDown(KeyCode.Q) && dashCooldownTime <= 0)
        {
            StartDash();
        }

        // 대쉬 중인지 확인
        if (isDashing)
        {
            Dash();
        }
        else
        {
            dashCooldownTime -= Time.deltaTime;
        }
    }

    private void StartDash()
    {
        // 대쉬 시작 애니메이션 트리거 발동
        animator.SetTrigger("DashAttack");

        isDashing = true;
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        dashTime = dashDuration;
        dashCooldownTime = dashCooldown;
    }

    private void Dash()
    {
        if (dashTime > 0)
        {
            rb.velocity = dashDirection * dashSpeed;
            dashTime -= Time.deltaTime;

            // 대쉬 중 공격 처리
            PerformDashAttack();
        }
        else
        {
            StopDash();
        }
    }

    private void StopDash()
    {
        

        isDashing = false;
        rb.velocity = Vector2.zero;
    }

    private void PerformDashAttack()
    {
        // 대쉬 범위 안에 있는 적을 찾음
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, dashDirection, attackRange, enemyLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // 적에게 데미지를 가함
                Monster_Health enemy = hit.collider.GetComponent<Monster_Health>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

    private void Dash_move()
    {
        float _pos = Player.transform.position.x;
        if(_pos <= 0 && PlayerMovement.key_==false)
        {
            Player.transform.position = new Vector2(-8f, Player.transform.position.y);
        }else if(PlayerMovement.key_ == false)
        {
            _pos -= 7.7f;
            Player.transform.position = new Vector2(_pos, Player.transform.position.y);
        }
        else if (PlayerMovement.key_)
        {
            _pos += 7.7f;
            Player.transform.position = new Vector2(_pos, Player.transform.position.y);
        }
    }
}

