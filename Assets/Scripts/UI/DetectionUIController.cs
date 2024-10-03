using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionUIController : MonoBehaviour
{
    public float maxDetection = 100f;
    public bool isDecrease=false;
    public float currentDetection;
    public Animator eyeAnimator;  // �� �ִϸ��̼��� ������ �ִϸ�����
    private Coroutine detectionCoroutine;
    void Start()
    {
        currentDetection = 0f;
        UpdateEyeAnimation();  // ���� �� �ִϸ��̼� ������Ʈ
    }

    public void IncreaseDetection(float amount)
    {
        Debug.Log("??");
        currentDetection += amount;
        isDecrease = false;
        if (currentDetection > maxDetection)
        {
            currentDetection = maxDetection;
        }
        if (detectionCoroutine != null)
        {
            StopCoroutine(detectionCoroutine);
        }
        detectionCoroutine = StartCoroutine(DetectionCoroutine());
        UpdateEyeAnimation();  // �߰��� ��ȭ�� ���� �ִϸ��̼� ������Ʈ
    }

    public void DecreaseDetection(float amount)
    {
        currentDetection -= amount;
        if (currentDetection < 0)
        {
            currentDetection = 0;
        }
        UpdateEyeAnimation();  // �߰��� ��ȭ�� ���� �ִϸ��̼� ������Ʈ
    }
    private void Update()
    {
        if (isDecrease) DecreaseDetection(10 * Time.deltaTime);
    }
    private void UpdateEyeAnimation()
    {
        // �߰����� ���� �ִϸ������� "DetectionLevel" �Ķ���͸� ����
        float normalizedDetection = currentDetection / maxDetection;
        eyeAnimator.SetFloat("DetectionLevel", normalizedDetection);
    }
    private IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        isDecrease = true;
    }
}
