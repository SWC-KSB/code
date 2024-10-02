using System.Collections;
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
    private bool isSpeechBubbleVisible = false;  // ��ǳ�� ǥ�� ����

    private bool flickering = false;
    public Animator playerAnimator;  // �÷��̾��� �ִϸ�����
    public GrandfatherMovement2D grandfather;  // �Ҿƹ��� ĳ������ �̵� ��ũ��Ʈ
    public GameObject speechBubble;  // ��ǳ�� UI ������Ʈ
    public Transform customBubblePosition;  // ��ǳ���� ǥ���� �������� ������ ��ġ
    public float bubbleDuration = 2.0f;  // ��ǳ�� ���� �ð�

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);  // ó���� ������ �����ϰ� ����
        playerAnimator.SetBool("IsLying", true);  // ó���� ħ�뿡 �����ִ� ���·� ����
        speechBubble.SetActive(false);  // ó������ ��ǳ���� ��Ȱ��ȭ
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

        // �� ��° ��Ŭ���� �Ҿƹ����� �÷��̾� ������ �ɾ���� ��ǳ���� ǥ��
        else if (Input.GetMouseButtonDown(1) && flickerCount == maxFlickerCount && !playerHasRisen)
        {
            PlayWakeUpAnimation();  // ù ��° �ִϸ��̼� ����
            grandfather.StartWalking();  // �Ҿƹ����� �÷��̾� ������ �ȱ� ����
            playerHasRisen = true;  // �ִϸ��̼� ���� ���·� ����
            ShowSpeechBubble();  // ��ǳ�� ǥ��
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

    // ��ǳ���� ǥ���ϴ� �Լ�
    private void ShowSpeechBubble()
    {
        if (!isSpeechBubbleVisible && customBubblePosition != null)
        {
            speechBubble.SetActive(true);  // ��ǳ�� Ȱ��ȭ
            speechBubble.transform.position = customBubblePosition.position;  // �������� ������ ��ġ�� ��ǳ�� ǥ��
            isSpeechBubbleVisible = true;
            StartCoroutine(HideSpeechBubbleAfterTime());  // ���� �ð� �� ��ǳ�� ��Ȱ��ȭ
        }
        else if (customBubblePosition == null)
        {
            Debug.LogError("��ǳ�� ��ġ�� �������� �ʾҽ��ϴ�. customBubblePosition�� Ȯ���ϼ���.");
        }
    }

    // ���� �ð� �� ��ǳ���� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator HideSpeechBubbleAfterTime()
    {
        yield return new WaitForSeconds(bubbleDuration);
        speechBubble.SetActive(false);  // ��ǳ�� ��Ȱ��ȭ
        isSpeechBubbleVisible = false;
    }
}
