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
    private SpriteRenderer _spriteRenderer; // �����̴� ȿ���� ���� ��������Ʈ ������
    private bool isInvincible = false; // ���� ���� ����
    [SerializeField] private float invincibleDuration = 2f; // ���� ���� �ð�
    [SerializeField] private float blinkFrequency = 0.1f; // �����̴� �ֱ�
    private float wallJumpCheck;
    private float horizontalInput;
    public bool key_;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ �ʱ�ȭ
    }

    private void FixedUpdate()
    {
        // �÷��̾� ȸ�� ����
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0f;
        transform.eulerAngles = currentRotation;

        // WASD Ű �Է� ó��
        horizontalInput = 0; // �⺻������ �Է� �� �ʱ�ȭ
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1; // �������� �̵�
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1; // ���������� �̵�
        }

        // ĳ���� �¿� ����
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

    // ���� �浹�ϸ� �����̸鼭 ���� ����ϴ� �޼���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            StartCoroutine(BlinkAndPassThrough());
        }
    }

    // �����̸� ���� ����ϴ� �ڷ�ƾ
    private IEnumerator BlinkAndPassThrough()
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true); // ������ �浹 ��Ȱ��ȭ

        float elapsedTime = 0f;
        while (elapsedTime < invincibleDuration)
        {
            // �����̱�: ��������Ʈ�� ���� �� ����
            _spriteRenderer.color = new Color(1, 1, 1, 0.2f); // ���� ����
            yield return new WaitForSeconds(blinkFrequency);
            _spriteRenderer.color = new Color(1, 1, 1, 1f); // �������
            yield return new WaitForSeconds(blinkFrequency);

            elapsedTime += blinkFrequency * 2;
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false); // ������ �浹 �ٽ� Ȱ��ȭ
        isInvincible = false;
    }

    // ���� �� �� �ִ��� ������ üũ
    public bool canAtk()
    {
        return horizontalInput == 0 && isGrounded() && !OnWall();
    }
}
