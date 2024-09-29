using UnityEngine;
using UnityEngine.UI;

public class Enemy_Healthbar : MonoBehaviour
{
    public Slider healthBarSlider; // 체력바 슬라이더
    private float maxHealth;

    // 체력바 초기화
    public void Initialize(float InitialHealth)
    {
        maxHealth = InitialHealth;
        healthBarSlider.maxValue = InitialHealth;
        healthBarSlider.value = InitialHealth;
    }

    // 체력바 업데이트 메서드
    public void UpdateHealthBar(float currentHealth)
    {
        Debug.Log("Updating Health Bar: " + currentHealth);  // 로그로 확인
        healthBarSlider.value = currentHealth;
    }

    // 체력바 보이기
    public void ShowHealthBar()
    {
        healthBarSlider.gameObject.SetActive(true);
    }

    // 체력바 숨기기
    public void HideHealthBar()
    {
        healthBarSlider.gameObject.SetActive(false);
    }

    // 임시 테스트를 위한 Start() 함수 추가
    void Start()
    {
        // 체력바의 값을 절반으로 설정하여 테스트
        healthBarSlider.value = healthBarSlider.maxValue / 2;
        Debug.Log("Test: Set healthBarSlider.value to " + healthBarSlider.value);
    }
}
