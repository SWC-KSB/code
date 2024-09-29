using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DetectionUIController : MonoBehaviour
{
    public Slider detectionSlider;   // 발각도 UI 슬라이더
    public Animator detectionAnimator; // 발각도 애니메이터 (눈이 점점 뜨이는 애니메이션)

    private float detectionLevel = 0f; // 현재 발각도
    private float maxDetection = 10f; // 최대 발각도

    // 발각도 UI 슬라이더 및 애니메이션 업데이트 함수
    public void UpdateDetectionUI(float detectionValue, float maxValue)
    {
        detectionLevel = detectionValue;
        maxDetection = maxValue;

        // 발각도 슬라이더 업데이트 (시각적으로 눈이 점점 뜨이는 효과)
        detectionSlider.value = detectionLevel / maxDetection;

        // 발각도 애니메이터 업데이트
        detectionAnimator.SetFloat("DetectionLevel", detectionLevel / maxDetection);
    }
}


