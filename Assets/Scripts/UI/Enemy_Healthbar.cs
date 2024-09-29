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
        Debug.Log("Before updating, healthBarSlider.value: " + healthBarSlider.value);
        healthBarSlider.value = currentHealth;
        Debug.Log("After updating, healthBarSlider.value: " + healthBarSlider.value);
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

    
}
