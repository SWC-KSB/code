using UnityEngine;
using UnityEngine.SceneManagement;

public class back_potal : MonoBehaviour
{
    // �� �Լ��� ȣ��Ǹ� ���� ���� �ٽ� �ε�˴ϴ�.
    public void Die()
    {
        // ���� Ȱ��ȭ�� ���� �ҷ����� �ٽ� �ε�
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
