using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Pattern_3 : MonoBehaviour
{
    [Header("idle")]
    [SerializeField] float idleMoveSpeed;
    [SerializeField] Vector2 idleMoveDirection;

    [Header("AttackUpDown")]
    [SerializeField] float attackMoveSpeed;
    [SerializeField] Vector2 attackMoveDirection;


    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;
    private Vector2 playerPosition;
    private bool hasPlayerPosition;

    [Header("Other")]
    [SerializeField] Transform GroundCheckUp;
    [SerializeField] Transform GroundCheckDown;
    [SerializeField] Transform GroundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool goingUp = true;
    private bool facingLeft = true;
    private Rigidbody2D enemyRB;


    // Start is called before the first frame update
    void Start()
    {
        idleMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(GroundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(GroundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(GroundCheckWall.position, groundCheckRadius, groundLayer);


        transform.rotation = Quaternion.identity;


        IdleState();

    }
void IdleState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idleMoveSpeed * idleMoveDirection;
    }
    void AttackUpDwon()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    void AttackPlayer()
    {
        if (!hasPlayerPosition)
        {
            playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPosition = true;
        }
        if (hasPlayerPosition)
        {
            enemyRB.velocity = playerPosition * attackMoveSpeed;
        }
        if (isTouchingWall || isTouchingDown)
        {
            enemyRB.velocity = Vector2.zero;
            hasPlayerPosition = false;
        }
        playerPosition = player.position - transform.position;

        playerPosition.Normalize();

        enemyRB.velocity = playerPosition * attackPlayerSpeed;
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
    void ChangeDirection()
    {
        goingUp = !goingUp;
        idleMoveDirection.y *= -1;
        attackMoveDirection.y *= -1;

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