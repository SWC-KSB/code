using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBlock : MonoBehaviour
{
    public float respawnTime = 5.0f;
    public GameObject child;
    private bool isActive = true;

    [Header("ȿ����")]
    public AudioClip deactivateSound;    // ��� ��Ȱ��ȭ �� �Ҹ�
    private AudioSource audioSource;     // ����� �ҽ� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // AudioSource ������Ʈ �������� �Ǵ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        // ��� ��Ȱ��ȭ ȿ���� ���
        if (deactivateSound != null)
        {
            audioSource.PlayOneShot(deactivateSound);
        }

        child.SetActive(false); // ������Ʈ ��Ȱ��ȭ
        isActive = false;
        yield return new WaitForSeconds(respawnTime); // respawnTime ���� ���
        child.SetActive(true);  // ������Ʈ �ٽ� Ȱ��ȭ
        isActive = true;
    }
}
