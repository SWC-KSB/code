using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;         // �뽬 �ӵ�
    public float dashDuration = 0.2f;     // �뽬 ���� �ð�
    public float dashCooldown = 1f;       // �뽬 ��Ÿ��
    public float attackRange = 1f;        // ���� ����
    public int attackDamage = 10;         // ���� ������

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTime = 0f;
    private Vector2 dashDirection;
    private Animator animator;            // �ִϸ����� �߰�
    public GameObject Player;
    PlayerMovement PlayerMovement;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // �ִϸ����� ������Ʈ ��������
    }

    private void Update()
    {
        // �뽬�� ���� �Է� ó��
        if (Input.GetKeyDown(KeyCode.Q) && dashCooldownTime <= 0)
        {
            StartDash();
        }

        // �뽬 ������ Ȯ��
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
        // �뽬 ������ 0�� ��� �÷��̾��� �ٶ󺸴� �������� �뽬
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(transform.localScale.x, 0);  // �⺻������ �÷��̾ �ٶ󺸴� �������� ����
        }

        // �뽬 ���� �ִϸ��̼� Ʈ���� �ߵ�
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

            // �뽬 �� ���� ó��
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

        // ������ �� ������Ʈ�� ���� ��� ó��
        if (enemies.Length == 0)
        {
            return;
        }

        // �� �� ������Ʈ�� ���� ���� Ȯ��
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                continue; // ���� ���� null�̸� �Ѿ
            }

            // ���� ���� �ȿ� �ִ��� Ȯ��
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= attackRange)
            {
                // ������ �������� ���ϱ�
                Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    // Dash_move �Լ� ����
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

    // �߰��� ������� ���� Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // �뽬 ���� ���� �ð�ȭ
    }
}
