using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DetectionUIController : MonoBehaviour
{
    public Slider detectionSlider;   // �߰��� UI �����̴�
    public Animator detectionAnimator; // �߰��� �ִϸ����� (���� ���� ���̴� �ִϸ��̼�)

    private float detectionLevel = 0f; // ���� �߰���
    private float maxDetection = 10f; // �ִ� �߰���

    // �߰��� UI �����̴� �� �ִϸ��̼� ������Ʈ �Լ�
    public void UpdateDetectionUI(float detectionValue, float maxValue)
    {
        detectionLevel = detectionValue;
        maxDetection = maxValue;

        // �߰��� �����̴� ������Ʈ (�ð������� ���� ���� ���̴� ȿ��)
        detectionSlider.value = detectionLevel / maxDetection;

        // �߰��� �ִϸ����� ������Ʈ
        detectionAnimator.SetFloat("DetectionLevel", detectionLevel / maxDetection);
    }
}


