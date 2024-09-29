using System.Collections;
using System.Collections.Generic;
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
    private SpriteRenderer _spriteRenderer; // 깜박이는 효과를 위한 스프라이트 렌더러
    private bool isInvincible = false; // 무적 상태 여부
    [SerializeField] private float invincibleDuration = 2f; // 무적 지속 시간
    [SerializeField] private float blinkFrequency = 0.1f; // 깜박이는 주기
    private float wallJumpCheck;
    private float horizontalInput;
    public bool key_;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 초기화
    }

    private void FixedUpdate()
    {
        // 플레이어 회전 방지
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0f;
        transform.eulerAngles = currentRotation;

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
        if (isGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
            _anim.SetTrigger("Jump");
        }
        else if (OnWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
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

    // 적과 충돌하면 깜박이면서 적을 통과하는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            StartCoroutine(BlinkAndPassThrough());
        }
    }

    // 깜박이며 적을 통과하는 코루틴
    private IEnumerator BlinkAndPassThrough()
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true); // 적과의 충돌 비활성화

        float elapsedTime = 0f;
        while (elapsedTime < invincibleDuration)
        {
            // 깜박이기: 스프라이트의 알파 값 조절
            _spriteRenderer.color = new Color(1, 1, 1, 0.2f); // 투명도 낮춤
            yield return new WaitForSeconds(blinkFrequency);
            _spriteRenderer.color = new Color(1, 1, 1, 1f); // 원래대로
            yield return new WaitForSeconds(blinkFrequency);

            elapsedTime += blinkFrequency * 2;
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false); // 적과의 충돌 다시 활성화
        isInvincible = false;
    }

    // 공격 할 수 있는지 없는지 체크
    public bool canAtk()
    {
        return horizontalInput == 0 && isGrounded() && !OnWall();
    }
}
