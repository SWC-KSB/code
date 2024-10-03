using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisappear : MonoBehaviour
{
    // 오브젝트가 몇 초 후에 사라질지를 설정하는 변수
    public float disappearTime = 3.0f;

    void Start()
    {
        // 시작 후 disappearTime 후에 오브젝트를 비활성화
        Invoke("Disappear", disappearTime);
    }

    void Disappear()
    {
        // 현재 오브젝트를 비활성화
        gameObject.SetActive(false);
    }
}

