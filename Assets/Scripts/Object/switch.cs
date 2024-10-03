using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject swch;       // ����ġ ������Ʈ
    public GameObject obj;        // �� ������Ʈ
    private bool isTrigger = false;  // �÷��̾ �� ��ó�� �ִ��� ����

    [Header("ȿ����")]
    public AudioClip switchSound;   // ����ġ ȿ����
    public AudioClip doorSound;     // �� �����ų� ���� �� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    [Header("���� ����")]
    [Range(0f, 1f)] public float switchVolume = 1f;  // ����ġ ȿ���� ����
    [Range(0f, 1f)] public float doorVolume = 1f;    // �� �����ų� ���� �� ȿ���� ����

    // Start is called before the first frame update
    void Start()
    {
        swch.SetActive(false);  // ���� ���� �� ����ġ�� ��Ȱ��ȭ

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
        // �÷��̾ �� ��ó�� �ְ� 'F' Ű�� ������ ���� ���ų� �ݱ�
        if (isTrigger && Input.GetKeyDown(KeyCode.F))
        {
            obj.SetActive(!obj.activeSelf);  // �� ���� �Ǵ� �ݱ�

            // �� ����/�ݱ� ȿ���� ���
            if (doorSound != null)
            {
                audioSource.PlayOneShot(doorSound, doorVolume);  // �� ȿ���� ���� ����
            }
        }
    }

    // �÷��̾ �� ��ó�� ������ ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        swch.SetActive(true);   // ����ġ Ȱ��ȭ
        isTrigger = true;

        // ����ġ ȿ���� ���
        if (switchSound != null)
        {
            audioSource.PlayOneShot(switchSound, switchVolume);  // ����ġ ȿ���� ���� ����
        }
    }

    // �÷��̾ �� ��ó���� ����� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrigger = false;
        swch.SetActive(false);  // ����ġ ��Ȱ��ȭ
    }
}
