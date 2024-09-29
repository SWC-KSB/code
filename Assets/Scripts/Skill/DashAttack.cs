using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;         // 대쉬 속도
    public float dashDuration = 0.2f;     // 대쉬 지속 시간
    public float dashCooldown = 1f;       // 대쉬 쿨타임
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
        // 대쉬 방향이 0일 경우 플레이어의 바라보는 방향으로 대쉬
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(transform.localScale.x, 0);  // 기본적으로 플레이어가 바라보는 방향으로 설정
        }

        // 대쉬 시작 애니메이션 트리거 발동
        animator.SetTrigger("DashAttack");

        isDashing = true;
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 감지된 적 오브젝트가 없는 경우 처리
        if (enemies.Length == 0)
        {
            return;
        }

        // 각 적 오브젝트에 대해 범위 확인
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                continue; // 만약 적이 null이면 넘어감
            }

            // 적이 범위 안에 있는지 확인
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= attackRange)
            {
                // 적에게 데미지를 가하기
                Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    // Dash_move 함수 유지
    private void Dash_move()
    {
        float _pos = Player.transform.position.x;
        if (_pos <= 0 && PlayerMovement.key_ == false)
        {
            Player.transform.position = new Vector2(-8f, Player.transform.position.y);
        }
        else if (PlayerMovement.key_ == false)
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

    // 추가된 디버깅을 위한 Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // 대쉬 공격 범위 시각화
    }
}
