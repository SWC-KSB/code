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

        // ���� �ð� �� �� ��° ȭ������ ��ȯ
        yield return new WaitForSeconds(transitionDuration);

        yield return StartCoroutine(FadeOut());
        firstCam.Priority = 10;
        secondCam.Priority = 20;  // �� ��° ȭ�� Ȱ��ȭ
        yield return StartCoroutine(FadeIn());

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
}
