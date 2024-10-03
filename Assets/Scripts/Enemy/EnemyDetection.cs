using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public DetectionUIController detectionMeter;
    public float detectionRate = 10f; // �߰����� �����ϴ� �ӵ�
    private bool playerDetected = false;

    // Animator�� �߰��Ͽ� �ִϸ��̼��� ����
    public Animator enemyAnimator;

    // ���� ���� ����
    public float detectionRadius = 5f;  // ���� ���� ������
    public LayerMask playerLayer;       // �÷��̾� ���̾ ����

    [Header("ȿ����")]
    public AudioClip detectionSound;    // ���� �Ҹ�
    private AudioSource audioSource;    // ����� �ҽ� ������Ʈ

    void Start()
    {
        // ��Ÿ�ӿ��� DetectionMeter�� ã�Ƽ� �Ҵ�
        detectionMeter = FindObjectOfType<DetectionUIController>();
        if (detectionMeter == null)
        {
            Debug.LogError("DetectionMeter not found in the scene!");
        }

        // Animator�� ����� �Ҵ�ƴ��� Ȯ��
        if (enemyAnimator == null)
        {
            Debug.LogError("Animator not assigned to EnemyDetection script.");
        }

        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ���� ���� ���� �÷��̾ �ִ��� Ȯ��
        playerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerDetected)
        {
            detectionMeter.IncreaseDetection(detectionRate * Time.deltaTime);

            // �÷��̾ �������� �� �ִϸ��̼� Ʈ���� ����
            enemyAnimator.SetTrigger("Detected");

            // ���� �Ҹ� ���
            if (detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(detectionSound);
            }
        }
    }

    // ���� ���� �ð�ȭ�� ���� Gizmos
    private void OnDrawGizmosSelected()
    {
        // ���� ������ �ݰ��� �ð������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
