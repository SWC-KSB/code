using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private float range = 5f;  // �÷��̾� ���� ����
    [SerializeField] private float detectionSpeed = 20f;  // �߰��� ��� �ӵ�
    [SerializeField] private float detectionDecreaseSpeed = 10f;  // �߰��� ���� �ӵ�
    [SerializeField] private float maxDetection = 100f;  // �ִ� �߰���
    private float detectionLevel = 0f;  // ���� �߰���

    [SerializeField] private float colliderDistance = 0.5f;  // Collider�� ������ �Ÿ� ���� ����
    [SerializeField] private LayerMask playerLayer;  // �÷��̾� ���̾�
    [SerializeField] private BoxCollider2D boxCollider;  // ���� BoxCollider

    private Animator anim;  // �� �ִϸ�����
    public Animator detectionAnimator;  // �߰��� UI �ִϸ����� (���� ������ ������ �ý���)
    private bool playerDetected = false;  // �÷��̾� ���� ����

    private void Awake()
    {
        anim = GetComponent<Animator>();

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
                Debug.LogError("BoxCollider2D component is missing on this enemy.");
            }
        }

        // **�߰����� �̹� ������ �������� �ʵ��� ó��**
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
        HandleDetectionAnimation();        // �÷��̾� ���� �� �� �ִϸ��̼� ����
    }

    // �÷��̾ �����ϰ� �߰��� ����/���� ���� ó��
    private void DetectPlayerAndUpdateDetection()
    {
        if (PlayerInSight())  // PlayerInSight �Լ� ȣ��
        {
            detectionLevel += detectionSpeed * Time.deltaTime;
            playerDetected = true;  // �÷��̾ ������
            Debug.Log("Player detected. Detection level increasing: " + detectionLevel);
        }
        else
        {
            detectionLevel -= detectionDecreaseSpeed * Time.deltaTime;
            playerDetected = false;  // �÷��̾� ���� ����
            Debug.Log("Player not detected. Detection level decreasing: " + detectionLevel);
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
            Debug.Log("Updating UI animation with detectionRatio: " + detectionRatio);
        }
        else
        {
            Debug.LogWarning("Detection Animator is not assigned.");
        }
    }

    // �÷��̾� ���� �� ���� �ִϸ��̼� ����
    private void HandleDetectionAnimation()
    {
        if (playerDetected)
        {
            anim.SetTrigger("Detected");
            Debug.Log("Enemy animation triggered.");
        }
        else
        {
            anim.ResetTrigger("Detected");
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
