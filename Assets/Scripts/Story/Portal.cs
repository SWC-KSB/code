using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad; // 이동할 씬의 이름

    // 플레이어가 포탈에 들어오면 씬을 로드
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 포탈에 들어온 것이 플레이어일 때만
        {
            LoadScene();
        }
    }

    // 씬 로드 함수
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
