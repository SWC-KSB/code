using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBlock : MonoBehaviour
{
    public float respawnTime = 5.0f;
    public GameObject child;
    private bool isActive = true;

    [Header("효과음")]
    public AudioClip deactivateSound;    // 블록 비활성화 시 소리
    private AudioSource audioSource;     // 오디오 소스 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && isActive)
        {
            // 점프 카운트 증가
            player.IncreaseJump();
            // 오브젝트 비활성화 및 재생성 코루틴 시작
            StartCoroutine(RespawnBlock());
        }
    }

    private IEnumerator RespawnBlock()
    {
        // 블록 비활성화 효과음 재생
        if (deactivateSound != null)
        {
            audioSource.PlayOneShot(deactivateSound);
        }

        child.SetActive(false); // 오브젝트 비활성화
        isActive = false;
        yield return new WaitForSeconds(respawnTime); // respawnTime 동안 대기
        child.SetActive(true);  // 오브젝트 다시 활성화
        isActive = true;
    }
}
