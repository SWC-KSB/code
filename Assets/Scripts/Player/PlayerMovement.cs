using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] private float jumpPower;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCheck;
    private float horizontalInput;
    public bool key_;
    private int jumpCount;
    public int maxJumpCount;

    [Header("ȿ����")]
    public AudioClip walkSound;         // �ȴ� �Ҹ�
    public AudioClip jumpSound;         // ���� �Ҹ�
    public float walkSoundVolume = 1.0f; // �ȴ� �Ҹ� ����
    public float walkSoundInterval = 0.5f; // �ȴ� �Ҹ� ��� ����
    private float lastWalkSoundTime = 0f;  // ���������� �ȴ� �Ҹ��� ����� �ð�
    private AudioSource audioSource;    // ����� �ҽ� ������Ʈ

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z = 0;
        transform.eulerAngles = rotation;

        if (isGrounded())
        {
            jumpCount = maxJumpCount;
        }

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

        // �ȴ� �Ҹ� ���
        if (horizontalInput != 0 && isGrounded())
        {
            PlayWalkSound();
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                    jumpCount--;
                }
            }
            else
                _rigid.gravityScale = 8f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                jumpCount--;
            }
        }
        else
            wallJumpCheck += Time.deltaTime;

        // �÷��̾ �� ���� �ö󰡴� �� ����
        PreventClimbingOnEnemy();
    }

    private void Jump()
    {
        if (OnWall() && !isGrounded())
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
        else if (jumpCount > 0)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
            _anim.SetTrigger("Jump");

            // ���� �Ҹ� ���
            PlayJumpSound();
        }
    }

    private void PlayWalkSound()
    {
        // ���� ���ݸ��� �ȴ� �Ҹ� ���
        if (Time.time - lastWalkSoundTime >= walkSoundInterval && walkSound != null)
        {
            audioSource.volume = walkSoundVolume;  // �ȴ� �Ҹ� ���� ����
            audioSource.PlayOneShot(walkSound);    // �ȴ� �Ҹ� ���
            lastWalkSoundTime = Time.time;         // ������ �Ҹ� ��� �ð� ������Ʈ
        }
    }

    private void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound); // ���� �Ҹ� ���
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
             boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return raycastHit.collider != null; // ���� ��� �ִ��� ����
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0,
            new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    // �� ���� �ö��� �ʰ� �ϴ� �޼��� (�±� ���)
    private void PreventClimbingOnEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f);

        // ���� �浹�� �����ϸ� �÷��̾� ��ġ�� ����
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            transform.position = new Vector3(transform.position.x, hit.collider.bounds.max.y + 0.5f, transform.position.z);
        }
    }

    // ���� �� �� �ִ��� ������ 
    public bool canAtk()
    {
        return horizontalInput == 0 && isGrounded() && !OnWall();
    }

    public void IncreaseJump()
    {
        jumpCount++;
    }
}
