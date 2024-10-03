using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    public DetectionUIController detectionMeter;
    public float detectionRate = 10f; // 발각도가 증가하는 속도
    public float frequency = 0.0f;
    public float start = 0.0f;
    private bool playerDetected = false;

    [Header("효과음")]
    public AudioClip detectionSound;    // 감지 소리
    private AudioSource audioSource;    // 오디오 소스 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        detectionMeter = FindObjectOfType<DetectionUIController>();

        // AudioSource 컴포넌트 가져오기 또는 없으면 추가
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

            // 감지 소리 재생
            if (detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(detectionSound);
            }
        }
    }
}
