using System.Collections;  // IEnumerator�� ����ϱ� ���� �߰��ؾ� ��
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // ������ �� ������
    public Transform spawnLocation; // ���� ��ġ
    public float spawnInterval = 5f; // ���� ����

    // ���� �̵� ���
    public Transform leftEdge;
    public Transform rightEdge;

    // ���� �̵� �ӵ� �� ��� �ð�
    public float speed;
    public float idleDuration;

    // �ִϸ�����
    public Animator anim;

    // ���� ���� �� �ڷ�ƾ ����
    void Start()
    {
        // �ʿ��� �������� null�� �ƴ��� üũ
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab is not assigned in the Inspector.");
            return;
        }

        if (spawnLocation == null)
        {
            Debug.LogError("spawnLocation is not assigned in the Inspector.");
            return;
        }

        // spawnInterval�� 0 �̻����� üũ
        if (spawnInterval <= 0f)
        {
            Debug.LogError("spawnInterval must be greater than 0.");
            return;
        }

        StartCoroutine(SpawnEnemiesRepeatedly());
    }

    // ���� �������� ���� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnEnemiesRepeatedly()
    {
        Debug.Log("Coroutine started!");

        while (true)
        {
            SpawnEnemy(spawnLocation.position);

            // ���� ���� �ð���ŭ ���
            Debug.Log("Waiting for " + spawnInterval + " seconds.");
            yield return new WaitForSeconds(spawnInterval); // �� �κп��� ������ �߻��� �� ����
        }
    }

    // ���� �����ϰ� �ʱ�ȭ�ϴ� �Լ�
    private void SpawnEnemy(Vector3 position)
    {
        // �� �ν��Ͻ� ����
        GameObject enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);

        // �� ������Ʈ�� EnemyPatrol ��ũ��Ʈ ��������
        EnemyPatrol patrol = enemyInstance.GetComponent<EnemyPatrol>();

        if (patrol != null)
        {
            patrol.Initialize(leftEdge, rightEdge, speed, idleDuration, anim);
        }
        else
        {
            Debug.LogError("EnemyPatrol script not found on the instantiated enemy.");
        }
    }
}
