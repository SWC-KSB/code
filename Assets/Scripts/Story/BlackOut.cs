using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : MonoBehaviour
{
    public Image fadeImage;  // 화면을 덮는 Image
    public float flickerSpeed = 1.0f;  // 깜빡이는 속도
    public float flickerDuration = 2.0f;  // 깜빡이는 총 시간
    private int flickerCount = 0;  // 깜빡임 횟수 추적
    private int maxFlickerCount = 2;  // 최대 깜빡임 횟수 (화면 깜빡임용)
    private bool playerHasRisen = false;  // 플레이어가 첫 애니메이션 실행 여부 확인용
    private bool secondAnimationTriggered = false;  // 두 번째 애니메이션 실행 여부 확인용
    private bool isSpeechBubbleVisible = false;  // 말풍선 표시 상태

    private bool flickering = false;
    public Animator playerAnimator;  // 플레이어의 애니메이터
    public GrandfatherMovement2D grandfather;  // 할아버지 캐릭터의 이동 스크립트
    public GameObject speechBubble;  // 말풍선 UI 오브젝트
    public Transform customBubblePosition;  // 말풍선을 표시할 수동으로 지정한 위치
    public float bubbleDuration = 2.0f;  // 말풍선 유지 시간

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);  // 처음에 완전히 투명하게 설정
        playerAnimator.SetBool("IsLying", true);  // 처음에 침대에 누워있는 상태로 설정
        speechBubble.SetActive(false);  // 처음에는 말풍선을 비활성화
    }

    void Update()
    {
        // 첫 두 번의 우클릭은 화면 깜빡임만 발생
        if (Input.GetMouseButtonDown(1) && flickerCount < maxFlickerCount && !flickering)
        {
            flickering = true;
            flickerCount++;  // 우클릭 감지 시 깜빡임 횟수 증가
            StartCoroutine(FlickerEffect());
        }

        // 세 번째 우클릭은 할아버지가 플레이어 쪽으로 걸어오며 말풍선을 표시
        else if (Input.GetMouseButtonDown(1) && flickerCount == maxFlickerCount && !playerHasRisen)
        {
            PlayWakeUpAnimation();  // 첫 번째 애니메이션 실행
            grandfather.StartWalking();  // 할아버지가 플레이어 쪽으로 걷기 시작
            playerHasRisen = true;  // 애니메이션 실행 상태로 설정
            ShowSpeechBubble();  // 말풍선 표시
        }

        // 네 번째 우클릭은 두 번째 애니메이션 (StandUp) 트리거
        else if (Input.GetMouseButtonDown(1) && playerHasRisen && !secondAnimationTriggered)
        {
            secondAnimationTriggered = true;
            TriggerStandUpAnimation();  // 두 번째 애니메이션 실행
        }
    }

    // 깜빡임 효과 코루틴 (첫 두 번의 우클릭에서 실행)
    private IEnumerator FlickerEffect()
    {
        float timePassed = 0f;

        while (timePassed < flickerDuration)
        {
            timePassed += Time.deltaTime;
            float alpha = Mathf.PingPong(timePassed * flickerSpeed, 1);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 깜빡임 효과가 끝나면 투명도를 0으로 설정
        fadeImage.color = new Color(0, 0, 0, 0);
        flickering = false;  // 깜빡임 완료 후 다시 우클릭 가능하게 설정
    }

    // 첫 번째 애니메이션 (WakeUp)을 재생하는 함수
    private void PlayWakeUpAnimation()
    {
        playerAnimator.SetBool("IsLying", false);  // 누워있는 상태에서 벗어나도록 설정
        playerAnimator.SetTrigger("WakeUp");  // WakeUp 애니메이션 트리거
    }

    // 두 번째 애니메이션 (StandUp)을 트리거하는 함수
    private void TriggerStandUpAnimation()
    {
        playerAnimator.SetTrigger("StandUp");  // StandUp 애니메이션 트리거
    }

    // 말풍선을 표시하는 함수
    private void ShowSpeechBubble()
    {
        if (!isSpeechBubbleVisible && customBubblePosition != null)
        {
            speechBubble.SetActive(true);  // 말풍선 활성화
            speechBubble.transform.position = customBubblePosition.position;  // 수동으로 지정한 위치에 말풍선 표시
            isSpeechBubbleVisible = true;
            StartCoroutine(HideSpeechBubbleAfterTime());  // 일정 시간 후 말풍선 비활성화
        }
        else if (customBubblePosition == null)
        {
            Debug.LogError("말풍선 위치가 지정되지 않았습니다. customBubblePosition을 확인하세요.");
        }
    }

    // 일정 시간 후 말풍선을 비활성화하는 코루틴
    private IEnumerator HideSpeechBubbleAfterTime()
    {
        yield return new WaitForSeconds(bubbleDuration);
        speechBubble.SetActive(false);  // 말풍선 비활성화
        isSpeechBubbleVisible = false;
    }
}
