using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Middle_Boss_HealthBar : MonoBehaviour
{
    [SerializeField] private Enemy_Health MiddleBossHealth;
    [SerializeField] private Image totalHpBar;
    [SerializeField] private Image currentHPBar;

    private void Start()
    {
        // totalHpBar�� �÷��̾��� �ִ� ü�¿� �°� ä���� �־�� �մϴ�.
        totalHpBar.fillAmount = 1f; // ��ü ü�� �ٴ� 100%�� ����
    }

    private void Update()
    {
        // currentHealth�� maxHealth�� ������ 0���� 1 ������ ������ ��ȯ
        currentHPBar.fillAmount = MiddleBossHealth.currentHealth / MiddleBossHealth.InitialHealth;
    }
}
