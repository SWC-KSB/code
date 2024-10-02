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
        // totalHpBar는 플레이어의 최대 체력에 맞게 채워져 있어야 합니다.
        totalHpBar.fillAmount = 1f; // 전체 체력 바는 100%로 시작
    }

    private void Update()
    {
        // currentHealth를 maxHealth로 나누어 0에서 1 사이의 값으로 변환
        currentHPBar.fillAmount = MiddleBossHealth.currentHealth / MiddleBossHealth.InitialHealth;
    }
}
