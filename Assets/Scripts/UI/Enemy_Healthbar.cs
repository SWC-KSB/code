using UnityEngine;
using UnityEngine.UI;

public class Enemy_Healthbar : MonoBehaviour
{
    public Slider healthBarSlider; // ü�¹� �����̴�
    private float maxHealth;

    // ü�¹� �ʱ�ȭ
    public void Initialize(float InitialHealth)
    {
        maxHealth = InitialHealth;
        healthBarSlider.maxValue = InitialHealth;
        healthBarSlider.value = InitialHealth;
    }

    // ü�¹� ������Ʈ �޼���
    public void UpdateHealthBar(float currentHealth)
    {
        Debug.Log("Before updating, healthBarSlider.value: " + healthBarSlider.value);
        healthBarSlider.value = currentHealth;
        Debug.Log("After updating, healthBarSlider.value: " + healthBarSlider.value);
    }

    // ü�¹� ���̱�
    public void ShowHealthBar()
    {
        healthBarSlider.gameObject.SetActive(true);
    }

    // ü�¹� �����
    public void HideHealthBar()
    {
        healthBarSlider.gameObject.SetActive(false);
    }

    
}
