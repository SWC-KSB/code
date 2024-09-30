using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBlock : MonoBehaviour
{
    public float respawnTime = 5.0f;
    public GameObject child;
    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
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
        child.SetActive(false); // 오브젝트 비활성화
        isActive = false;
        yield return new WaitForSeconds(respawnTime); // respawnTime 동안 대기
        child.SetActive(true);  // 오브젝트 다시 활성화
        isActive = true;

    }
}
