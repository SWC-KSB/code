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
