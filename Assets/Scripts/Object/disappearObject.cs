using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearObject : MonoBehaviour
{
    public float disappearTime = 2f; // ����� �ð� (n��)
    public float reappearTime = 5f;  // �ٽ� ���� �ð� (x��)
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Handler());
        }
    }
    private IEnumerator Handler()
    {
        yield return new WaitForSeconds(disappearTime);
        obj.SetActive(false);
        yield return new WaitForSeconds(reappearTime);
        obj.SetActive(true);
    }
}
