using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    public float damage = 0.01f;
    public float frequency = 0.0f;
    public float start = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (frequency != 0)
        {
            InvokeRepeating("ToggleActivation", start, frequency);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleActivation()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {

            Health playerHealth = collider.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }



}