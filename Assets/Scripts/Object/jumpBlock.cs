using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBlock : MonoBehaviour
{
    public float respawnTime = 5.0f;
    public GameObject child;
    private bool isActive = true;
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
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && isActive)
        {
            // ���� ī��Ʈ ����
            player.IncreaseJump();
            // ������Ʈ ��Ȱ��ȭ �� ����� �ڷ�ƾ ����
            StartCoroutine(RespawnBlock());
        }
    }
    private IEnumerator RespawnBlock()
    {
        child.SetActive(false); // ������Ʈ ��Ȱ��ȭ
        isActive = false;
        yield return new WaitForSeconds(respawnTime); // respawnTime ���� ���
        child.SetActive(true);  // ������Ʈ �ٽ� Ȱ��ȭ
        isActive = true;

    }
}
