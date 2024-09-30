using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnLocation;
    public float spawnInterval = 5f;
    public Transform player;  // �÷��̾� ����

    public float speed;
    public float idleDuration;
    public Animator anim;
    public float followRange = 5f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("player is not assigned in the Inspector.");
            return;
        }

        StartCoroutine(SpawnEnemiesRepeatedly());
    }

    IEnumerator SpawnEnemiesRepeatedly()
    {
        while (true)
        {
            SpawnEnemy(spawnLocation.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);

        EnemyPatrol patrol = enemyInstance.GetComponent<EnemyPatrol>();
        if (patrol != null)
        {
            patrol.player = player;  // ������ ���� �÷��̾� ���� �Ҵ�
            patrol.speed = speed;
            patrol.idleDuration = idleDuration;
            patrol.followRange = followRange;
            patrol.anim = anim;  // �� �ִϸ����� �Ҵ�
        }
        else
        {
            Debug.LogError("EnemyPatrol script not found on the instantiated enemy.");
        }
    }
}
