using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionUIController : MonoBehaviour
{
    public float maxDetection = 100f;
    public bool isDecrease=false;
    public float currentDetection;
    public Animator eyeAnimator;  // 눈 애니메이션을 제어할 애니메이터
    private Coroutine detectionCoroutine;
    void Start()
    {
        currentDetection = 0f;
        UpdateEyeAnimation();  // 시작 시 애니메이션 업데이트
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
        UpdateEyeAnimation();  // 발각도 변화에 따른 애니메이션 업데이트
    }

    public void DecreaseDetection(float amount)
    {
        currentDetection -= amount;
        if (currentDetection < 0)
        {
            currentDetection = 0;
        }
        UpdateEyeAnimation();  // 발각도 변화에 따른 애니메이션 업데이트
    }
    private void Update()
    {
        if (isDecrease) DecreaseDetection(10 * Time.deltaTime);
    }
    private void UpdateEyeAnimation()
    {
        // 발각도에 따라 애니메이터의 "DetectionLevel" 파라미터를 설정
        float normalizedDetection = currentDetection / maxDetection;
        eyeAnimator.SetFloat("DetectionLevel", normalizedDetection);
    }
    private IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        isDecrease = true;
    }
}
