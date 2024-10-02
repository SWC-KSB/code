using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class Story_Camera : MonoBehaviour
{
    public CinemachineVirtualCamera firstCam;  // ù ��° ī�޶� (ó�� ������ ī�޶�)
    public CinemachineVirtualCamera secondCam;  // �� ��° ī�޶�
    public CinemachineVirtualCamera mainCam;  // ���� ī�޶�
    public Image fadeImage;  // ȭ�� �������� ���� UI Image
    public float fadeDuration = 0.5f;  // ������ ���� �ð�
    public float transitionDuration = 3.0f;  // �� ȭ�� ��ȯ ���� �ð�
    public float shakeDuration = 0.5f;  // ȭ�� ��鸲 ���� �ð�
    public float shakeAmplitude = 1.5f;  // ȭ�� ��鸲 ����
    public float shakeFrequency = 2.0f;  // ȭ�� ��鸲 �ӵ�

    private CinemachineBasicMultiChannelPerlin noise;  // ī�޶� ��鸲�� ���� Perlin Noise

    void Start()
    {
        // ó�� ������ �� ù ��° ī�޶� Ȱ��ȭ�ǵ��� ����
        firstCam.Priority = 20;  // ù ��° ī�޶� �켱���� ����
        secondCam.Priority = 10;  // �� ��° ī�޶�� ��Ȱ��ȭ
        mainCam.Priority = 10;  // ���� ī�޶� ��Ȱ��ȭ

        // ������ �� ȭ�� ��ȯ ����
        StartCoroutine(ScreenTransitionSequence());
    }

    private IEnumerator ScreenTransitionSequence()
    {
        // ù ��° ȭ������ ��ȯ (ȭ�� ������ ȿ�� ����)
        yield return StartCoroutine(FadeOut());
        firstCam.Priority = 20;  // ù ��° ȭ�� Ȱ��ȭ
        secondCam.Priority = 10;
        mainCam.Priority = 10;
        yield return StartCoroutine(FadeIn());

        // ù ��° ī�޶� ��鸲 ȿ�� ����
        ActivateNoise(firstCam, shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeDuration);
        DeactivateNoise(firstCam);

        // ���� �ð� �� �� ��° ȭ������ ��ȯ
        yield return new WaitForSeconds(transitionDuration);

        yield return StartCoroutine(FadeOut());
        firstCam.Priority = 10;
        secondCam.Priority = 20;  // �� ��° ȭ�� Ȱ��ȭ
        yield return StartCoroutine(FadeIn());

        // �� ��° ī�޶� ��鸲 ȿ�� ����
        ActivateNoise(secondCam, shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeDuration);
        DeactivateNoise(secondCam);

        // ���� �ð� �� ���� ȭ������ ��ȯ
        yield return new WaitForSeconds(transitionDuration);

        yield return StartCoroutine(FadeOut());
        secondCam.Priority = 10;
        mainCam.Priority = 20;  // ���� ȭ�� Ȱ��ȭ
        yield return StartCoroutine(FadeIn());
    }

    // ȭ�� ������ ȿ�� (���̵� �ƿ�)
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

    // ȭ�� ������ ȿ�� (���̵� ��)
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

    // Ư�� ī�޶� Noise Ȱ��ȭ
    private void ActivateNoise(CinemachineVirtualCamera cam, float amplitude, float frequency)
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;  // ��鸲 ���� ����
            noise.m_FrequencyGain = frequency;  // ��鸲 �ӵ� ����
        }
    }

    // Ư�� ī�޶� Noise ��Ȱ��ȭ
    private void DeactivateNoise(CinemachineVirtualCamera cam)
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;  // ��鸲 ������ 0���� �����Ͽ� ����
        }
    }
}
