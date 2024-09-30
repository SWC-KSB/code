using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Key1 : MonoBehaviour
{
    public Image img_Skill_2;
    private bool isCoolingDown = false; // 쿨타임 상태를 관리할 변수

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        // 쿨타임이 돌아가는 중이 아니라면 키 입력을 받음
        if (!isCoolingDown && Input.GetKeyUp(KeyCode.E))
        {
            StartCoroutine(CoolTime(3f));
        }
    }

    IEnumerator CoolTime(float cool)
    {
        isCoolingDown = true; // 쿨타임 시작
        print("쿨타임 코루틴 실행");

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img_Skill_2.fillAmount = cool / 3.0f; // 3초 기준으로 계산
            yield return new WaitForFixedUpdate();
        }

        isCoolingDown = false; // 쿨타임 종료
        print("쿨타임 코루틴 완료");
    }
}
