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
        Debug.Log("Updating Health Bar: " + currentHealth);  // �α׷� Ȯ��
        healthBarSlider.value = currentHealth;
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

    // �ӽ� �׽�Ʈ�� ���� Start() �Լ� �߰�
    void Start()
    {
        // ü�¹��� ���� �������� �����Ͽ� �׽�Ʈ
        healthBarSlider.value = healthBarSlider.maxValue / 2;
        Debug.Log("Test: Set healthBarSlider.value to " + healthBarSlider.value);
    }
}
