using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOut : MonoBehaviour
{
    public Image fadeImage;  // ȭ���� ���� Image
    public float flickerSpeed = 1.0f;  // �����̴� �ӵ�
    public float flickerDuration = 2.0f;  // �����̴� �� �ð�
    private int flickerCount = 0;  // ������ Ƚ�� ����
    private int maxFlickerCount = 2;  // �ִ� ������ Ƚ�� (ȭ�� �����ӿ�)
    private bool playerHasRisen = false;  // �÷��̾ ù �ִϸ��̼� ���� ���� Ȯ�ο�
    private bool secondAnimationTriggered = false;  // �� ��° �ִϸ��̼� ���� ���� Ȯ�ο�

    private float flickerTime = 0f;
    private bool flickering = false;
    public Animator playerAnimator;  // �÷��̾��� �ִϸ�����
    public GrandfatherMovement2D grandfather;  // �Ҿƹ��� ĳ������ �̵� ��ũ��Ʈ

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);  // ó���� ������ �����ϰ� ����
        playerAnimator.SetBool("IsLying", true);  // ó���� ħ�뿡 �����ִ� ���·� ����
    }

    void Update()
    {
        // ù �� ���� ��Ŭ���� ȭ�� �����Ӹ� �߻�
        if (Input.GetMouseButtonDown(1) && flickerCount < maxFlickerCount && !flickering)
        {
            flickering = true;
            flickerCount++;  // ��Ŭ�� ���� �� ������ Ƚ�� ����
            StartCoroutine(FlickerEffect());
        }

        // �� ��° ��Ŭ���� �Ҿƹ����� �÷��̾� ������ �ɾ��
        else if (Input.GetMouseButtonDown(1) && flickerCount == maxFlickerCount && !playerHasRisen)
        {
            PlayWakeUpAnimation();  // ù ��° �ִϸ��̼� ����
            grandfather.StartWalking();  // �Ҿƹ����� �÷��̾� ������ �ȱ� ����
            playerHasRisen = true;  // �ִϸ��̼� ���� ���·� ����
        }

        // �� ��° ��Ŭ���� �� ��° �ִϸ��̼� (StandUp) Ʈ����
        else if (Input.GetMouseButtonDown(1) && playerHasRisen && !secondAnimationTriggered)
        {
            secondAnimationTriggered = true;
            TriggerStandUpAnimation();  // �� ��° �ִϸ��̼� ����
        }
    }

    // ������ ȿ�� �ڷ�ƾ (ù �� ���� ��Ŭ������ ����)
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

        // ������ ȿ���� ������ ������ 0���� ����
        fadeImage.color = new Color(0, 0, 0, 0);
        flickering = false;  // ������ �Ϸ� �� �ٽ� ��Ŭ�� �����ϰ� ����
    }

    // ù ��° �ִϸ��̼� (WakeUp)�� ����ϴ� �Լ�
    private void PlayWakeUpAnimation()
    {
        playerAnimator.SetBool("IsLying", false);  // �����ִ� ���¿��� ������� ����
        playerAnimator.SetTrigger("WakeUp");  // WakeUp �ִϸ��̼� Ʈ����
    }

    // �� ��° �ִϸ��̼� (StandUp)�� Ʈ�����ϴ� �Լ�
    private void TriggerStandUpAnimation()
    {
        playerAnimator.SetTrigger("StandUp");  // StandUp �ִϸ��̼� Ʈ����
    }
}
