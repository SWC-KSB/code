using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad; // �̵��� ���� �̸�

    // �÷��̾ ��Ż�� ������ ���� �ε�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ��Ż�� ���� ���� �÷��̾��� ����
        {
            LoadScene();
        }
    }

    // �� �ε� �Լ�
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
