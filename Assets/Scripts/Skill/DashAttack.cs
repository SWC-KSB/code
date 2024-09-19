using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDashAttack : MonoBehaviour
{
    public float dashSpeed = 20f;         // �뽬 �ӵ�
    public float dashDuration = 0.2f;     // �뽬 ���� �ð�
    public float dashCooldown = 1f;       // �뽬 ��Ÿ��
    public LayerMask enemyLayer;          // �� Layer ����
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
        // �뽬 ���� �ִϸ��̼� Ʈ���� �ߵ�
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
        // �뽬 ���� �ȿ� �ִ� ���� ã��
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, dashDirection, attackRange, enemyLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // ������ �������� ����
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

