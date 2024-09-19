using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sideway : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float damage;

    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    // ��ֹ����� ��ġ
    private void Awake() 
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    // ���� ������� �ʰ� �ϴ� �ڵ�
    private void Update()
    {
        if(movingLeft)
        {
            if(transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime,
                    transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if(transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime,
                    transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
    }

    // �÷��̾ ������ �޾��� �� �������� ������ ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
