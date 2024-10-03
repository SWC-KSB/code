using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpeech7 : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public int scene;

    [Header("대사 오브젝트 및 위치 설정")]
    private List<Tuple<GameObject, Transform>> speechData = new List<Tuple<GameObject, Transform>>();  // 대사 오브젝트와 위치를 저장하는 리스트
    public float speechDisplayTime = 2f;  // 대사 표시 시간

    private int currentSpeechIndex = 0;    // 현재 대사 인덱스
    private Coroutine speechCoroutine;     // 대사 코루틴을 관리하기 위한 변수
    private bool hasPortalSpawned = false; // 포탈이 이미 생성되었는지 여부

    [Header("프리팹 설정")]
    public GameObject prefabToSpawn;       // 생성할 프리팹
    public Transform spawnLocation;        // 프리팹을 생성할 위치

    [Header("할아버지 설정")]
    public Transform grandfatherNewPosition; // 대화가 끝난 후 할아버지가 이동할 위치
    public float moveSpeed = 2f;              // 할아버지 이동 속도
    private bool isWalking = false;            // 할아버지 걷는 상태 체크

    // 대사 오브젝트와 위치를 설정할 변수 선언
    [Header("대사 오브젝트들")]
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


    [Header("대사 위치들")]
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



    // 대사 오브젝트와 위치를 리스트에 추가하는 함수
    public void AddSpeech(GameObject speechObject, Transform position)
    {
        // 대사를 추가하고 대사 오브젝트는 시작 시 비활성화
        speechData.Add(new Tuple<GameObject, Transform>(speechObject, position));
        speechObject.SetActive(false);
    }

    // 대사를 화면에 출력
    public void DisplaySpeech()
    {
        // 대사 리스트가 비어있는지 확인
        if (speechData.Count == 0)
        {
            Debug.LogWarning("대사 리스트가 비어 있습니다.");
            return;
        }

        // 이전 대사 오브젝트가 활성화되어 있으면 강제로 비활성화
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
                speechObject.SetActive(true); // 대사 오브젝트 활성화

                // 일정 시간 후 대사를 비활성화하는 코루틴 시작
                if (speechCoroutine != null)
                {
                    StopCoroutine(speechCoroutine);
                }
                speechCoroutine = StartCoroutine(HideSpeechAfterDelay(speechObject));

                currentSpeechIndex++;  // 다음 대사로 넘어감
            }
        }
        else
        {
            Debug.Log("모든 대사를 출력했습니다.");
            SpawnPrefab();    // 프리팹 생성
        }
    }

    // 일정 시간이 지나면 대사 오브젝트를 비활성화하는 코루틴
    private IEnumerator HideSpeechAfterDelay(GameObject speechObject)
    {
        yield return new WaitForSeconds(speechDisplayTime);
        speechObject.SetActive(false);
    }

    // 프리팹 생성 함수
    private void SpawnPrefab()
    {
        // 포탈이 아직 생성되지 않은 경우에만 생성
        if (!hasPortalSpawned && prefabToSpawn != null && spawnLocation != null)
        {
            Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            hasPortalSpawned = true;  // 포탈 생성 후 다시 생성되지 않게 설정
        }
    }

    // 할아버지 이동 함수
    private void MoveGrandfather()
    {
        if (grandfatherNewPosition != null)
        {
            isWalking = true;
        }
    }

    // Update 함수에서 할아버지를 목표 위치로 이동
    private void Update()
    {
        if (isWalking)
        {
            // 할아버지의 현재 위치와 목표 위치 간의 거리를 계산
            float distance = Vector2.Distance(transform.position, grandfatherNewPosition.position);

            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(transform.position, grandfatherNewPosition.position, moveSpeed * Time.deltaTime);

            // 할아버지가 목표 위치에 도달했을 때
            if (distance < 0.1f)
            {
                isWalking = false;  // 걷는 상태 종료
            }
        }

        if (Input.GetMouseButtonDown(1)) // 우클릭 시 대사 출력 및 프리팹 생성
        {
            DisplaySpeech();  // 대사 출력

            MoveGrandfather(); // 할아버지 이동
        }
    }

    // 게임 시작 시 모든 대사 오브젝트를 비활성화
    void Start()
    {
        playerMovement.enabled = false;

        // 8개의 대사 오브젝트와 위치를 추가
        AddSpeech(speechObject1, speechPosition1);  // 첫 번째 대사
        AddSpeech(speechObject2, speechPosition2);  // 두 번째 대사
        AddSpeech(speechObject3, speechPosition3);  // 세 번째 대사
        AddSpeech(speechObject4, speechPosition4);  // 네 번째 대사
        AddSpeech(speechObject5, speechPosition5);  // 다섯 번째 대사
        AddSpeech(speechObject6, speechPosition6);  // 여섯 번째 대사
        AddSpeech(speechObject7, speechPosition7);  // 일곱 번째 대사
        AddSpeech(speechObject8, speechPosition8);  // 여덟 번째 대사
        AddSpeech(speechObject9, speechPosition9);  // 여덟 번째 대사
        AddSpeech(speechObject10, speechPosition10);  // 여덟 번째 대사
        AddSpeech(speechObject11, speechPosition11);  // 여덟 번째 대사
        AddSpeech(speechObject12, speechPosition12);  // 여덟 번째 대사
        AddSpeech(speechObject13, speechPosition13);  // 여덟 번째 대사


        // 모든 대사 오브젝트 비활성화
        foreach (var speechTuple in speechData)
        {
            speechTuple.Item1.SetActive(false);
        }
    }
}
