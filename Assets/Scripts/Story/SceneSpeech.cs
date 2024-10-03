using System;  // Tuple�� ����ϱ� ���� �߰�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpeech : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public int scene;

    [Header("��� ������Ʈ �� ��ġ ����")]
    private List<Tuple<GameObject, Transform>> speechData = new List<Tuple<GameObject, Transform>>();  // ��� ������Ʈ�� ��ġ�� �����ϴ� ����Ʈ
    public float speechDisplayTime = 2f;  // ��� ǥ�� �ð�

    private int currentSpeechIndex = 0;    // ���� ��� �ε���
    private Coroutine speechCoroutine;     // ��� �ڷ�ƾ�� �����ϱ� ���� ����

    [Header("������ ����")]
    public GameObject prefabToSpawn;       // ������ ������
    public Transform spawnLocation;        // �������� ������ ��ġ

    [Header("�Ҿƹ��� ����")]
    public Transform grandfatherNewPosition; // ��ȭ�� ���� �� �Ҿƹ����� �̵��� ��ġ
    public float moveSpeed = 2f;              // �Ҿƹ��� �̵� �ӵ�
    private bool isWalking = false;            // �Ҿƹ��� �ȴ� ���� üũ

    // ��� ������Ʈ�� ��ġ�� ������ ���� ����
    [Header("��� ������Ʈ��")]
    public GameObject speechObject1;
    public GameObject speechObject2;
    public GameObject speechObject3;
    public GameObject speechObject4;
    public GameObject speechObject5;
    public GameObject speechObject6;
    public GameObject speechObject7;
    public GameObject speechObject8;

    [Header("��� ��ġ��")]
    public Transform speechPosition1;
    public Transform speechPosition2;
    public Transform speechPosition3;
    public Transform speechPosition4;
    public Transform speechPosition5;
    public Transform speechPosition6;
    public Transform speechPosition7;
    public Transform speechPosition8;

    // ��� ������Ʈ�� ��ġ�� ����Ʈ�� �߰��ϴ� �Լ�
    public void AddSpeech(GameObject speechObject, Transform position)
    {
        // ��縦 �߰��ϰ� ��� ������Ʈ�� ���� �� ��Ȱ��ȭ
        speechData.Add(new Tuple<GameObject, Transform>(speechObject, position));
        speechObject.SetActive(false);
    }

    // ��縦 ȭ�鿡 ���
    public void DisplaySpeech()
    {
        // ��� ����Ʈ�� ����ִ��� Ȯ��
        if (speechData.Count == 0)
        {
            Debug.LogWarning("��� ����Ʈ�� ��� �ֽ��ϴ�.");
            return;
        }

        if (currentSpeechIndex < speechData.Count)
        {
            var speechTuple = speechData[currentSpeechIndex];
            GameObject speechObject = speechTuple.Item1;
            Transform speechPosition = speechTuple.Item2;

            if (speechObject != null && speechPosition != null)
            {
                speechObject.transform.position = speechPosition.position;
                speechObject.SetActive(true); // ��� ������Ʈ Ȱ��ȭ

                // ���� �ð� �� ��縦 ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
                if (speechCoroutine != null)
                {
                    StopCoroutine(speechCoroutine);
                }
                speechCoroutine = StartCoroutine(HideSpeechAfterDelay(speechObject));

                currentSpeechIndex++;  // ���� ���� �Ѿ
            }
        }
        else
        {
            Debug.Log("��� ��縦 ����߽��ϴ�.");
        }
    }

    // ���� �ð��� ������ ��� ������Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator HideSpeechAfterDelay(GameObject speechObject)
    {
        yield return new WaitForSeconds(speechDisplayTime);
        speechObject.SetActive(false);

        // �ڷ�ƾ�� ����� ���� ó���� �ʿ䰡 ������ �߰� ����
    }

    // ������ ���� �Լ�
    private void SpawnPrefab()
    {
        // ��ȭ�� ������ ������ ����
        if (prefabToSpawn != null && spawnLocation != null)
        {
            Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
        }
    }

    // �Ҿƹ��� �̵� �Լ�
    private void MoveGrandfather()
    {
        if (grandfatherNewPosition != null)
        {
            isWalking = true;
        }
    }

    // Update �Լ����� �Ҿƹ����� ��ǥ ��ġ�� �̵�
    private void Update()
    {
        if (isWalking)
        {
            // �Ҿƹ����� ���� ��ġ�� ��ǥ ��ġ ���� �Ÿ��� ���
            float distance = Vector2.Distance(transform.position, grandfatherNewPosition.position);

            // ��ǥ ��ġ�� �̵�
            transform.position = Vector2.MoveTowards(transform.position, grandfatherNewPosition.position, moveSpeed * Time.deltaTime);

            // �Ҿƹ����� ��ǥ ��ġ�� �������� ��
            if (distance < 0.1f)
            {
                isWalking = false;  // �ȴ� ���� ����
            }
        }

        if (Input.GetMouseButtonDown(1)) // ��Ŭ�� �� ��� ��� �� ������ ����
        {
            DisplaySpeech();  // ��� ���
            SpawnPrefab();    // ������ ����
            MoveGrandfather(); // �Ҿƹ��� �̵�
        }
    }

    // ���� ���� �� ��� ��� ������Ʈ�� ��Ȱ��ȭ
    void Start()
    {
        playerMovement.enabled = false;

        // 8���� ��� ������Ʈ�� ��ġ�� �߰�
        AddSpeech(speechObject1, speechPosition1);  // ù ��° ���
        AddSpeech(speechObject2, speechPosition2);  // �� ��° ���
        AddSpeech(speechObject3, speechPosition3);  // �� ��° ���
        AddSpeech(speechObject4, speechPosition4);  // �� ��° ���
        AddSpeech(speechObject5, speechPosition5);  // �ټ� ��° ���
        AddSpeech(speechObject6, speechPosition6);  // ���� ��° ���
        AddSpeech(speechObject7, speechPosition7);  // �ϰ� ��° ���
        AddSpeech(speechObject8, speechPosition8);  // ���� ��° ���

        // ��� ��� ������Ʈ ��Ȱ��ȭ
        foreach (var speechTuple in speechData)
        {
            speechTuple.Item1.SetActive(false);
        }
    }
}
