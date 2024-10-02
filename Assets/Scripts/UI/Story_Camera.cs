using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class Story_Camera : MonoBehaviour
{
    public CinemachineVirtualCamera firstCam;  // 첫 번째 카메라 (처음 시작할 카메라)
    public CinemachineVirtualCamera secondCam;  // 두 번째 카메라
    public CinemachineVirtualCamera mainCam;  // 메인 카메라
    public Image fadeImage;  // 화면 깜빡임을 위한 UI Image
    public float fadeDuration = 0.5f;  // 깜빡임 지속 시간
    public float transitionDuration = 3.0f;  // 각 화면 전환 간의 시간
    public float shakeDuration = 0.5f;  // 화면 흔들림 지속 시간
    public float shakeAmplitude = 1.5f;  // 화면 흔들림 강도
    public float shakeFrequency = 2.0f;  // 화면 흔들림 속도

    private CinemachineBasicMultiChannelPerlin noise;  // 카메라 흔들림을 위한 Perlin Noise

    void Start()
    {
        // 처음 시작할 때 첫 번째 카메라가 활성화되도록 설정
        firstCam.Priority = 20;  // 첫 번째 카메라 우선순위 설정
        secondCam.Priority = 10;  // 두 번째 카메라는 비활성화
        mainCam.Priority = 10;  // 메인 카메라도 비활성화

        // 시작할 때 화면 전환 실행
        StartCoroutine(ScreenTransitionSequence());
    }

    private IEnumerator ScreenTransitionSequence()
    {
        // 첫 번째 화면으로 전환 (화면 깜빡임 효과 포함)
        yield return StartCoroutine(FadeOut());
        firstCam.Priority = 20;  // 첫 번째 화면 활성화
        secondCam.Priority = 10;
        mainCam.Priority = 10;
        yield return StartCoroutine(FadeIn());

        // 첫 번째 카메라에 흔들림 효과 적용
        ActivateNoise(firstCam, shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeDuration);
        DeactivateNoise(firstCam);

        // 일정 시간 후 두 번째 화면으로 전환
        yield return new WaitForSeconds(transitionDuration);

        yield return StartCoroutine(FadeOut());
        firstCam.Priority = 10;
        secondCam.Priority = 20;  // 두 번째 화면 활성화
        yield return StartCoroutine(FadeIn());

        // 두 번째 카메라에 흔들림 효과 적용
        ActivateNoise(secondCam, shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeDuration);
        DeactivateNoise(secondCam);

        // 일정 시간 후 메인 화면으로 전환
        yield return new WaitForSeconds(transitionDuration);

        yield return StartCoroutine(FadeOut());
        secondCam.Priority = 10;
        mainCam.Priority = 20;  // 메인 화면 활성화
        yield return StartCoroutine(FadeIn());
    }

    // 화면 깜빡임 효과 (페이드 아웃)
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    // 화면 깜빡임 효과 (페이드 인)
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    // 특정 카메라에 Noise 활성화
    private void ActivateNoise(CinemachineVirtualCamera cam, float amplitude, float frequency)
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;  // 흔들림 강도 설정
            noise.m_FrequencyGain = frequency;  // 흔들림 속도 설정
        }
    }

    // 특정 카메라에 Noise 비활성화
    private void DeactivateNoise(CinemachineVirtualCamera cam)
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;  // 흔들림 강도를 0으로 설정하여 중지
        }
    }
}
