using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetection : MonoBehaviour
{
    [SerializeField] private float range = 5f;  // 플레이어 감지 범위
    [SerializeField] private float detectionSpeed = 20f;  // 발각도 상승 속도
    [SerializeField] private float detectionDecreaseSpeed = 10f;  // 발각도 감소 속도
    [SerializeField] private float maxDetection = 100f;  // 최대 발각도
    private float detectionLevel = 0f;  // 현재 발각도

    [SerializeField] private float colliderDistance = 0.5f;  // Collider로 감지할 거리 조절 변수
    [SerializeField] private LayerMask playerLayer;  // 플레이어 레이어
    [SerializeField] private BoxCollider2D boxCollider;  // 감지에 사용할 BoxCollider

    public Animator detectionAnimator;  // 발각도 UI 애니메이터 (눈이 서서히 떠지는 시스템)

    [Header("효과음")]
    public AudioClip detectionSound;    // 감지 소리
    private AudioSource audioSource;    // 오디오 소스 컴포넌트

    private void Awake()
    {
        // UI의 Animator를 동적으로 찾기
        if (detectionAnimator == null)
        {
            GameObject detectionUI = GameObject.Find("DetectionUI");  // UI 오브젝트 이름으로 찾기
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

        // BoxCollider2D가 할당되었는지 확인
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider2D component is missing on this object.");
            }
        }

        // AudioSource 컴포넌트 가져오기 또는 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 발각도가 이미 있으면 리셋하지 않도록 처리
        if (detectionLevel > 0)
        {
            Debug.Log("Detection level already set, no reset.");
            return;  // 발각도 값이 이미 존재하면 초기화하지 않음
        }
    }

    private void Update()
    {
        DetectPlayerAndUpdateDetection();  // 플레이어 감지 및 발각도 업데이트
        UpdateDetectionAnimation();        // 발각도 UI 애니메이션 업데이트
    }

    // 플레이어를 감지하고 발각도 증가/감소 로직 처리
    private void DetectPlayerAndUpdateDetection()
    {
        if (PlayerInSight())  // PlayerInSight 함수 호출
        {
            detectionLevel += detectionSpeed * Time.deltaTime;  // 발각도 증가
        }
        else
        {
            detectionLevel -= detectionDecreaseSpeed * Time.deltaTime;  // 발각도 감소
        }

        detectionLevel = Mathf.Clamp(detectionLevel, 0, maxDetection);  // 발각도 값 제한
    }

    // 플레이어가 감지 범위 안에 있는지 확인 (colliderDistance 포함)
    private bool PlayerInSight()
    {
        // BoxCast로 플레이어 감지
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.up * -range * transform.localScale.y * colliderDistance,  // 중앙에서 범위만큼 이동
            new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected in BoxCast.");

            // 감지 소리 재생
            if (detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(detectionSound);
            }

            return true;  // 플레이어가 범위 내에 있음
        }

        Debug.Log("Player not detected in BoxCast.");
        return false; // 플레이어가 범위 내에 없음
    }

    // 발각도 UI 애니메이션 업데이트 (눈이 서서히 떠지는 애니메이션의 진행도 제어)
    private void UpdateDetectionAnimation()
    {
        if (detectionAnimator != null)
        {
            // 발각도 비율을 계산
            float detectionRatio = detectionLevel / maxDetection;

            // Animator의 파라미터 "DetectionLevel"에 발각도 값을 전달
            detectionAnimator.SetFloat("DetectionLevel", detectionRatio);
        }
        else
        {
            Debug.LogWarning("Detection Animator is not assigned.");
        }
    }

    // 시야 범위를 Scene 뷰에서 시각화 (colliderDistance 적용)
    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                boxCollider.bounds.center + transform.up * -range * colliderDistance,  // 시야 범위 표시
                new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y));
        }
    }
}
