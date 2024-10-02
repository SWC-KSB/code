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
    // Start is called before the first frame update
    void Start()
    {
        detectionMeter = FindObjectOfType<DetectionUIController>();
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
        }
    }



}