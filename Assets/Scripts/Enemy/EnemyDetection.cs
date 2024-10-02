using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public DetectionUIController detectionMeter;
    public float detectionRate = 10f; // 발각도가 증가하는 속도
    private bool playerDetected = false;

    // Animator를 추가하여 애니메이션을 제어
    public Animator enemyAnimator;

    // 감지 범위 설정
    public float detectionRadius = 5f;  // 감지 범위 반지름
    public LayerMask playerLayer;       // 플레이어 레이어를 설정

    void Start()
    {
        // 런타임에서 DetectionMeter를 찾아서 할당
        detectionMeter = FindObjectOfType<DetectionUIController>();
        if (detectionMeter == null)
        {
            Debug.LogError("DetectionMeter not found in the scene!");
        }

        // Animator가 제대로 할당됐는지 확인
        if (enemyAnimator == null)
        {
            Debug.LogError("Animator not assigned to EnemyDetection script.");
        }
    }

    void Update()
    {
        // 감지 범위 내에 플레이어가 있는지 확인
        playerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerDetected)
        {
            detectionMeter.IncreaseDetection(detectionRate * Time.deltaTime);

            // 플레이어를 감지했을 때 애니메이션 트리거 실행
            enemyAnimator.SetTrigger("Detected");
        }
        
    }

    // 감지 범위 시각화를 위한 Gizmos
    private void OnDrawGizmosSelected()
    {
        // 감지 범위의 반경을 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
