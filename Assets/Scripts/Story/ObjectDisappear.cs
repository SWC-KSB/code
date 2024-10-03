using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisappear : MonoBehaviour
{
    // ������Ʈ�� �� �� �Ŀ� ��������� �����ϴ� ����
    public float disappearTime = 3.0f;

    void Start()
    {
        // ���� �� disappearTime �Ŀ� ������Ʈ�� ��Ȱ��ȭ
        Invoke("Disappear", disappearTime);
    }

    void Disappear()
    {
        // ���� ������Ʈ�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}

