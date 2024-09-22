using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCheck;
    private float horizontalInput;
    public bool key_;

    private void Awake() 
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        // WASD 키 입력 처리
        horizontalInput = 0; // 기본적으로 입력 값 초기화
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1; // 왼쪽으로 이동
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1; // 오른쪽으로 이동
        }


        // 캐릭터 좌우 반전
        if (horizontalInput > 0.01f)
        {
            key_ = true;
            transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            key_ = false;
            transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        }

        // true : 1, false : 0
        _anim.SetBool("Run", horizontalInput != 0);
        _anim.SetBool("IsGround", isGrounded());

        if (wallJumpCheck > 0.2f)
        {
            _rigid.velocity = new Vector2(horizontalInput * moveSpeed, _rigid.velocity.y);

            if (OnWall() && !isGrounded())
            {
                _rigid.gravityScale = 0;
                _rigid.velocity = Vector2.zero;
            }
            else
                _rigid.gravityScale = 8f;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

        }
        else
            wallJumpCheck += Time.deltaTime;
    }

    private void Jump()
    {
        Debug.Log("g");
        if (isGrounded())
        {
            
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
            _anim.SetTrigger("Jump");
        }
        else if (OnWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                // Mathf.Sign(float f) : f의 부호를 반환하는 함수
                // 0이나 양수 1을 음수일때는 -1 
                _rigid.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),
                    transform.localScale.y, transform.localScale.z);
            }
            else
            {
                _rigid.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCheck = 0;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0,
            new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    // 공격 할 수 있는지 없는지 
    public bool canAtk()
    {
        return horizontalInput == 0 && isGrounded() && !OnWall();
    }
}




