using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    public DetectionUIController detectionMeter;
    public float detectionRate = 10f; // �߰����� �����ϴ� �ӵ�
    public float frequency = 0.0f;
    public float start = 0.0f;
    private bool playerDetected = false;

    [Header("ȿ����")]
    public AudioClip detectionSound;    // ���� �Ҹ�
    private AudioSource audioSource;    // ����� �ҽ� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        detectionMeter = FindObjectOfType<DetectionUIController>();

        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (frequency != 0)
        {
            InvokeRepeating("ToggleActivation", start, frequency);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleActivation()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            detectionMeter.IncreaseDetection(detectionRate * Time.deltaTime);

            // ���� �Ҹ� ���
            if (detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(detectionSound);
            }
        }
    }
}
