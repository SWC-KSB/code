using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill_Key : MonoBehaviour
{
    public Image img_Skill_1;

    // Use this for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            StartCoroutine(CoolTime(3f));
        }
    }

    IEnumerator CoolTime (float cool)
    {
        print("��Ÿ�� �ڷ�ƾ ����");

        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            img_Skill_1.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }

        print("��Ÿ�� �ڷ�ƾ �Ϸ�");
    }


}
