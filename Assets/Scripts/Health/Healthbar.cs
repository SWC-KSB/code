using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHpBar;
    [SerializeField] private Image currentHPBar;

    private void Start()
    {
        totalHpBar.fillAmount = playerHealth.currentHealth ;
    }
    
    private void Update()
    {
        currentHPBar.fillAmount = playerHealth.currentHealth ;
    }
}
