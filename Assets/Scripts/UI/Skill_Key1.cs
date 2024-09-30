using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Key1 : MonoBehaviour
{
    public Image img_Skill_2;
    private bool isCoolingDown = false; // ��Ÿ�� ���¸� ������ ����

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        // ��Ÿ���� ���ư��� ���� �ƴ϶�� Ű �Է��� ����
        if (!isCoolingDown && Input.GetKeyUp(KeyCode.E))
        {
            StartCoroutine(CoolTime(3f));
        }
    }

    IEnumerator CoolTime(float cool)
    {
        isCoolingDown = true; // ��Ÿ�� ����
        print("��Ÿ�� �ڷ�ƾ ����");

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            img_Skill_2.fillAmount = cool / 3.0f; // 3�� �������� ���
            yield return new WaitForFixedUpdate();
        }

        isCoolingDown = false; // ��Ÿ�� ����
        print("��Ÿ�� �ڷ�ƾ �Ϸ�");
    }
}
