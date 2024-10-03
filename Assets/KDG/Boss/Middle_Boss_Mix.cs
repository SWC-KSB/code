using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Middle_Boss_Mix : MonoBehaviour
{
    enum BossState { Idle, AttackUpDown, AttackPlayer }
    BossState currentState;

    [Header("Idle")]
    [SerializeField] float idleMoveSpeed;
    [SerializeField] Vector2 idleMoveDirection;

    [Header("Attack Up/Down")]
    [SerializeField] float attackMoveSpeed;
    [SerializeField] Vector2 attackMoveDirection;

    [Header("Attack Player")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform GroundCheckUp;
    [SerializeField] Transform GroundCheckDown;
    [SerializeField] Transform GroundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    [Header("효과음")]
    public AudioClip PingPongSound;
    private AudioSource audioSource;    // 오디오 소스 컴포넌트


    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool goingUp = true;
    private bool facingLeft = true;
    private Rigidbody2D enemyRB;
    private int WallCount = 0; 

    private float idleTimer = 0f;
    private float attackPlayerTimer = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }
    void Start()
    {
        idleMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        currentState = BossState.Idle; // 초기 상태를 Idle로 설정

        player = GameObject.FindWithTag("Player").transform;

    }

    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(GroundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(GroundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(GroundCheckWall.position, groundCheckRadius, groundLayer);

        switch (currentState)
        {
            case BossState.Idle:
                IdleState();
                idleTimer += Time.deltaTime; // Idle 상태의 시간 카운트

                if (WallCount == 3)
                {
                    AttackPlayer();
                }
                if (idleTimer >= 7f) // 7초가 지나면 AttackUpDown로 전환
                {
                    currentState = BossState.AttackUpDown;
                    idleTimer = 0f; // 타이머 초기화
                }
                break;

            case BossState.AttackPlayer:
                
                
                if (isTouchingWall || isTouchingDown) // 벽이나 바닥에 닿으면 Idle로 전환
                {
                    currentState = BossState.Idle;
                }
                break;

            case BossState.AttackUpDown:
               
                AttackUpDown();
                

                attackPlayerTimer += Time.deltaTime; // AttackPlayer 상태의 시간 카운트

                if (attackPlayerTimer >= 3f) // 5초가 지나면 Idle로 전환
                {
                    currentState = BossState.Idle;
                    attackPlayerTimer = 0f; // 타이머 초기화
                }


                break;
        }

        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            player.GetComponent<Health>().TakeDamage(5f);
        }


    }

    private void PlayPingPongSound(float startTime, float duration)
    {
        audioSource.clip = PingPongSound;
        audioSource.time = startTime; // 시작 시간 설정
        audioSource.Play();
        Invoke("StopSound", duration); // 지정된 시간 후에 정지
    }

    private void StopSound()
    {
        audioSource.Stop();
    }


    void IdleState()
    {
        if (isTouchingUp && goingUp)
        {
            PlayPingPongSound(0,1);
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            PlayPingPongSound(0, 1);
            ChangeDirection();
        }
        if (isTouchingWall)
        {
            PlayPingPongSound(0, 1);
            WallCount += 1;
            Flip();
        }
        enemyRB.velocity = idleMoveSpeed * idleMoveDirection;
    }

    void AttackUpDown()
    {
        if (isTouchingUp && goingUp)
        {
            PlayPingPongSound(0, 1);
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            PlayPingPongSound(0, 1);
            ChangeDirection();
        }
        if (isTouchingWall)

        {
            PlayPingPongSound(0, 1);
            WallCount += 1;
            Flip();
        }
        enemyRB.velocity = attackMoveSpeed * attackMoveDirection;

    }

    void AttackPlayer()
    {
        Vector2 playerPosition = player.position - transform.position;
        playerPosition.Normalize();
        enemyRB.velocity = playerPosition * attackPlayerSpeed;

        if (isTouchingWall || isTouchingDown)
        {
            PlayPingPongSound(0, 1);
            enemyRB.velocity = Vector2.zero;
        }
        WallCount = 0;
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idleMoveDirection.y *= -1;
        attackMoveDirection.y *= -1;
    }

    void FlipTowardPalyer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }
    void Flip()
    {
        facingLeft = !facingLeft;
        idleMoveDirection.x *= -1;
        attackMoveDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GroundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(GroundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(GroundCheckWall.position, groundCheckRadius);
    }
}
