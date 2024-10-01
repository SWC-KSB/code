using System.Collections;
using System.Collections.Generic;
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

    private float flickerTime = 0f;
    private bool flickering = false;
    public Animator playerAnimator;  // 플레이어의 애니메이터
    public GrandfatherMovement2D grandfather;  // 할아버지 캐릭터의 이동 스크립트

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);  // 처음에 완전히 투명하게 설정
        playerAnimator.SetBool("IsLying", true);  // 처음에 침대에 누워있는 상태로 설정
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

        // 세 번째 우클릭은 할아버지가 플레이어 쪽으로 걸어옴
        else if (Input.GetMouseButtonDown(1) && flickerCount == maxFlickerCount && !playerHasRisen)
        {
            PlayWakeUpAnimation();  // 첫 번째 애니메이션 실행
            grandfather.StartWalking();  // 할아버지가 플레이어 쪽으로 걷기 시작
            playerHasRisen = true;  // 애니메이션 실행 상태로 설정
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
}
