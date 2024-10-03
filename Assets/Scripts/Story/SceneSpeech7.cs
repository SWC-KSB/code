using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpeech7 : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public int scene;

    [Header("��� ������Ʈ �� ��ġ ����")]
    private List<Tuple<GameObject, Transform>> speechData = new List<Tuple<GameObject, Transform>>();  // ��� ������Ʈ�� ��ġ�� �����ϴ� ����Ʈ
    public float speechDisplayTime = 2f;  // ��� ǥ�� �ð�

    private int currentSpeechIndex = 0;    // ���� ��� �ε���
    private Coroutine speechCoroutine;     // ��� �ڷ�ƾ�� �����ϱ� ���� ����
    private bool hasPortalSpawned = false; // ��Ż�� �̹� �����Ǿ����� ����

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
    public GameObject speechObject9;
    public GameObject speechObject10;
    public GameObject speechObject11;
    public GameObject speechObject12;
    public GameObject speechObject13;


    [Header("��� ��ġ��")]
    public Transform speechPosition1;
    public Transform speechPosition2;
    public Transform speechPosition3;
    public Transform speechPosition4;
    public Transform speechPosition5;
    public Transform speechPosition6;
    public Transform speechPosition7;
    public Transform speechPosition8;
    public Transform speechPosition9;
    public Transform speechPosition10;
    public Transform speechPosition11;
    public Transform speechPosition12;
    public Transform speechPosition13;



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

        // ���� ��� ������Ʈ�� Ȱ��ȭ�Ǿ� ������ ������ ��Ȱ��ȭ
        if (currentSpeechIndex > 0)
        {
            var previousSpeechTuple = speechData[currentSpeechIndex - 1];
            if (previousSpeechTuple.Item1.activeSelf)
            {
                previousSpeechTuple.Item1.SetActive(false);
            }
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
            SpawnPrefab();    // ������ ����
        }
    }

    // ���� �ð��� ������ ��� ������Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator HideSpeechAfterDelay(GameObject speechObject)
    {
        yield return new WaitForSeconds(speechDisplayTime);
        speechObject.SetActive(false);
    }

    // ������ ���� �Լ�
    private void SpawnPrefab()
    {
        // ��Ż�� ���� �������� ���� ��쿡�� ����
        if (!hasPortalSpawned && prefabToSpawn != null && spawnLocation != null)
        {
            Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            hasPortalSpawned = true;  // ��Ż ���� �� �ٽ� �������� �ʰ� ����
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
        AddSpeech(speechObject9, speechPosition9);  // ���� ��° ���
        AddSpeech(speechObject10, speechPosition10);  // ���� ��° ���
        AddSpeech(speechObject11, speechPosition11);  // ���� ��° ���
        AddSpeech(speechObject12, speechPosition12);  // ���� ��° ���
        AddSpeech(speechObject13, speechPosition13);  // ���� ��° ���


        // ��� ��� ������Ʈ ��Ȱ��ȭ
        foreach (var speechTuple in speechData)
        {
            speechTuple.Item1.SetActive(false);
        }
    }
}
