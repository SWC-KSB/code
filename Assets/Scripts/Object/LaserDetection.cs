using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetection : MonoBehaviour
{
    [SerializeField] private float range = 5f;  // �÷��̾� ���� ����
    [SerializeField] private float detectionSpeed = 20f;  // �߰��� ��� �ӵ�
    [SerializeField] private float detectionDecreaseSpeed = 10f;  // �߰��� ���� �ӵ�
    [SerializeField] private float maxDetection = 100f;  // �ִ� �߰���
    private float detectionLevel = 0f;  // ���� �߰���

    [SerializeField] private float colliderDistance = 0.5f;  // Collider�� ������ �Ÿ� ���� ����
    [SerializeField] private LayerMask playerLayer;  // �÷��̾� ���̾�
    [SerializeField] private BoxCollider2D boxCollider;  // ������ ����� BoxCollider

    public Animator detectionAnimator;  // �߰��� UI �ִϸ����� (���� ������ ������ �ý���)

    [Header("ȿ����")]
    public AudioClip detectionSound;    // ���� �Ҹ�
    private AudioSource audioSource;    // ����� �ҽ� ������Ʈ

    private void Awake()
    {
        // UI�� Animator�� �������� ã��
        if (detectionAnimator == null)
        {
            GameObject detectionUI = GameObject.Find("DetectionUI");  // UI ������Ʈ �̸����� ã��
            if (detectionUI != null)
            {
                detectionAnimator = detectionUI.GetComponent<Animator>();
                if (detectionAnimator == null)
                {
                    Debug.LogError("Animator component not found on Detection UI.");
                }
            }
            else
            {
                Debug.LogError("Detection UI object not found in the scene.");
            }
        }

        // BoxCollider2D�� �Ҵ�Ǿ����� Ȯ��
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider2D component is missing on this object.");
            }
        }

        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �߰����� �̹� ������ �������� �ʵ��� ó��
        if (detectionLevel > 0)
        {
            Debug.Log("Detection level already set, no reset.");
            return;  // �߰��� ���� �̹� �����ϸ� �ʱ�ȭ���� ����
        }
    }

    private void Update()
    {
        DetectPlayerAndUpdateDetection();  // �÷��̾� ���� �� �߰��� ������Ʈ
        UpdateDetectionAnimation();        // �߰��� UI �ִϸ��̼� ������Ʈ
    }

    // �÷��̾ �����ϰ� �߰��� ����/���� ���� ó��
    private void DetectPlayerAndUpdateDetection()
    {
        if (PlayerInSight())  // PlayerInSight �Լ� ȣ��
        {
            detectionLevel += detectionSpeed * Time.deltaTime;  // �߰��� ����
        }
        else
        {
            detectionLevel -= detectionDecreaseSpeed * Time.deltaTime;  // �߰��� ����
        }

        detectionLevel = Mathf.Clamp(detectionLevel, 0, maxDetection);  // �߰��� �� ����
    }

    // �÷��̾ ���� ���� �ȿ� �ִ��� Ȯ�� (colliderDistance ����)
    private bool PlayerInSight()
    {
        // BoxCast�� �÷��̾� ����
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // �߾ӿ��� ������ŭ �̵�
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected in BoxCast.");

            // ���� �Ҹ� ���
            if (detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(detectionSound);
            }

            return true;  // �÷��̾ ���� ���� ����
        }

        Debug.Log("Player not detected in BoxCast.");
        return false; // �÷��̾ ���� ���� ����
    }

    // �߰��� UI �ִϸ��̼� ������Ʈ (���� ������ ������ �ִϸ��̼��� ���൵ ����)
    private void UpdateDetectionAnimation()
    {
        if (detectionAnimator != null)
        {
            // �߰��� ������ ���
            float detectionRatio = detectionLevel / maxDetection;

            // Animator�� �Ķ���� "DetectionLevel"�� �߰��� ���� ����
            detectionAnimator.SetFloat("DetectionLevel", detectionRatio);
        }
        else
        {
            Debug.LogWarning("Detection Animator is not assigned.");
        }
    }

    // �þ� ������ Scene �信�� �ð�ȭ (colliderDistance ����)
    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                boxCollider.bounds.center + transform.up * -range * colliderDistance,  // �þ� ���� ǥ��
                new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y));
        }
    }
}
