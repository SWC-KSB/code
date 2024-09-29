using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthBar : MonoBehaviour
{
    [SerializeField] private BossStateMachine BossHealth;
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
        currentHPBar.fillAmount = BossHealth.currentHealth / BossHealth.maxHealth;
    }
}
