using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject swch;       // 스위치 오브젝트
    public GameObject obj;        // 문 오브젝트
    private bool isTrigger = false;  // 플레이어가 문 근처에 있는지 여부

    [Header("효과음")]
    public AudioClip switchSound;   // 스위치 효과음
    public AudioClip doorSound;     // 문 열리거나 닫힐 때 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    [Header("음량 조절")]
    [Range(0f, 1f)] public float switchVolume = 1f;  // 스위치 효과음 볼륨
    [Range(0f, 1f)] public float doorVolume = 1f;    // 문 열리거나 닫힐 때 효과음 볼륨

    // Start is called before the first frame update
    void Start()
    {
        swch.SetActive(false);  // 게임 시작 시 스위치를 비활성화

        // AudioSource 컴포넌트 가져오기 또는 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 문 근처에 있고 'F' 키를 누르면 문을 열거나 닫기
        if (isTrigger && Input.GetKeyDown(KeyCode.F))
        {
            obj.SetActive(!obj.activeSelf);  // 문 열기 또는 닫기

            // 문 열기/닫기 효과음 재생
            if (doorSound != null)
            {
                audioSource.PlayOneShot(doorSound, doorVolume);  // 문 효과음 볼륨 조절
            }
        }
    }

    // 플레이어가 문 근처에 들어왔을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        swch.SetActive(true);   // 스위치 활성화
        isTrigger = true;

        // 스위치 효과음 재생
        if (switchSound != null)
        {
            audioSource.PlayOneShot(switchSound, switchVolume);  // 스위치 효과음 볼륨 조절
        }
    }

    // 플레이어가 문 근처에서 벗어났을 때
    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrigger = false;
        swch.SetActive(false);  // 스위치 비활성화
    }
}
