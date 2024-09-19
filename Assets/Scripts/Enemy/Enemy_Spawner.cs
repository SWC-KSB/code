using System.Collections;  // IEnumerator를 사용하기 위해 추가해야 함
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // 스폰할 적 프리팹
    public Transform spawnLocation; // 스폰 위치
    public float spawnInterval = 5f; // 스폰 간격

    // 적의 이동 경계
    public Transform leftEdge;
    public Transform rightEdge;

    // 적의 이동 속도 및 대기 시간
    public float speed;
    public float idleDuration;

    // 애니메이터
    public Animator anim;

    // 게임 시작 시 코루틴 시작
    void Start()
    {
        // 필요한 변수들이 null이 아닌지 체크
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

        // spawnInterval이 0 이상인지 체크
        if (spawnInterval <= 0f)
        {
            Debug.LogError("spawnInterval must be greater than 0.");
            return;
        }

        StartCoroutine(SpawnEnemiesRepeatedly());
    }

    // 일정 간격으로 적을 스폰하는 코루틴
    IEnumerator SpawnEnemiesRepeatedly()
    {
        Debug.Log("Coroutine started!");

        while (true)
        {
            SpawnEnemy(spawnLocation.position);

            // 생성 간격 시간만큼 대기
            Debug.Log("Waiting for " + spawnInterval + " seconds.");
            yield return new WaitForSeconds(spawnInterval); // 이 부분에서 문제가 발생할 수 있음
        }
    }

    // 적을 스폰하고 초기화하는 함수
    private void SpawnEnemy(Vector3 position)
    {
        // 적 인스턴스 생성
        GameObject enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);

        // 적 오브젝트의 EnemyPatrol 스크립트 가져오기
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
