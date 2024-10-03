using UnityEngine;
using UnityEngine.SceneManagement;

public class back_potal : MonoBehaviour
{
    // 이 함수가 호출되면 현재 씬이 다시 로드됩니다.
    public void Die()
    {
        // 현재 활성화된 씬을 불러오고 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
