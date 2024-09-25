using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Attack,
    Pattern,
    Defense
}

public class BossStateMachine : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private float moveSpeed = 3f;
    public float attackRange = 10f;
    private int hitsDuringPattern = 0;
    private float damageReceived = 0f;
    private float timeInCurrentState = 0f; 
    private int patternsPerformed = 0;
    private bool isPerformingPattern = false;
    public PlayerMovement playermovement;

    public Transform player;
    public Transform Boss;
    public GameObject platformPrefab;
    public GameObject spherePrefab;
    private BossState currentState;


    private GameObject object1;
    private GameObject object2;
    private GameObject Pattern3Room;

    private void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
        StartCoroutine(StateMachine());
        StartCoroutine(DecreaseHealthOverTime());
         

        // 오브젝트 비활성화
        if (object1 != null) object1.SetActive(false);
        if (object2 != null) object2.SetActive(false);
        if (Pattern3Room != null) Pattern3Room.SetActive(false);
    }

    private void Update()
    {
        
        if (Vector3.Distance(transform.position, player.position) < 1f) 
        {
            player.GetComponent<Health>().TakeDamage(5f);
        }

        // 시간 경과 체크
        timeInCurrentState += Time.deltaTime;

        
    }

    private void SwitchState()
    {
        timeInCurrentState = 0f; // 상태 전환 시 시간 초기화
        switch (currentState)
        {
            case BossState.Idle:
                currentState = BossState.Attack;
                break;

            case BossState.Attack:
                if (hitsDuringPattern >= 5) // 데미지를 5번 입으면 상태 전환
                {
                    currentState = BossState.Pattern;
                    hitsDuringPattern = 0;
                }
                break;

            case BossState.Pattern:
                if (hitsDuringPattern >= 5)
                {
                    currentState = BossState.Defense;
                }
                else
                {
                    patternsPerformed++;
                    if (patternsPerformed >= 3) // 패턴을 3번 수행했을 때
                    {
                        currentState = BossState.Idle;
                    }
                }
                break;

            case BossState.Defense:
                currentState = BossState.Attack; // 방어 상태에서 공격 상태로 전환
                break;
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    yield return IdleBehavior();
                    break;

                case BossState.Attack:
                    yield return AttackBehavior();
                    break;

                case BossState.Pattern:
                    yield return PatternBehavior();
                    break;

                case BossState.Defense:
                    yield return DefenseBehavior();
                    break;
            }
            yield return null;
        }
    }

    private IEnumerator IdleBehavior()
    {
        Debug.Log("Boss is waiting.");

        while (true)
        { 
            if (Vector3.Distance(transform.position, player.position) < attackRange)
            {
                Debug.Log("Player in range, switching to Attack state.");
                currentState = BossState.Attack; 
                yield break;
            }

            yield return null; 
        }
    }

    private IEnumerator AttackBehavior()
    {
        Debug.Log("Boss is attacking.");

        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                // 공격 로직 구현
                Vector3 squarePosition = player.position;
                GameObject square = GameObject.CreatePrimitive(PrimitiveType.Cube);
                square.transform.position = squarePosition;
                square.transform.localScale = new Vector3(1, 1, 1);
                Renderer squareRenderer = square.GetComponent<Renderer>();
                squareRenderer.material.color = Color.blue;

                yield return new WaitForSeconds(1f);
                squareRenderer.material.color = Color.red;

                float timer = 0f;
                while (timer < 0.5f)
                {
                    if (Vector3.Distance(square.transform.position, player.position) < 1f)
                    {
                        player.GetComponent<Health>().TakeDamage(5f);
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                Destroy(square);
            }
            else
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
            }

           
            if (hitsDuringPattern >= 5) // 데미지를 5번 입으면 상태 전환
            {
                currentState = BossState.Pattern; // 다음 상태로 전환
                hitsDuringPattern = 0; // 패턴 수행 수 초기화
                yield break; // AttackBehavior 종료
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private HashSet<int> executedPatterns = new HashSet<int>(); // 실행된 패턴을 저장하는 집합

    private IEnumerator PatternBehavior()
    {
        Debug.Log("Boss is performing a pattern.");
        isPerformingPattern = true; 

        int randomPattern;

        // 모든 패턴이 수행될 때까지 반복
        while (executedPatterns.Count < 5)
        {
            randomPattern = Random.Range(1, 6); // 1부터 5까지의 랜덤 패턴 선택

            // 이미 실행된 패턴인지 확인
            if (!executedPatterns.Contains(randomPattern))
            {
                executedPatterns.Add(randomPattern);
                switch (randomPattern)
                {
                    case 1:
                        yield return StartCoroutine(Pattern1());
                        break;
                    case 2:
                        yield return StartCoroutine(Pattern2());
                        break;
                    case 3:
                        yield return StartCoroutine(Pattern3());
                        break;
                    case 4:
                        yield return StartCoroutine(Pattern4());
                        break;
                    case 5:
                        yield return StartCoroutine(Pattern5());
                        break;
                }
                hitsDuringPattern = 0; // 패턴 종료 후 리셋
                break; 
            }
        }

        // 모든 패턴이 수행된 경우 초기화
        if (executedPatterns.Count >= 5)
        {
            executedPatterns.Clear(); // 실행된 패턴 초기화
        }

        isPerformingPattern = false; 
    }


    private IEnumerator DefenseBehavior()
    {
        Debug.Log("Boss is in defense mode.");
        yield return StartCoroutine(DefensePattern()); // 방어 패턴 실행
    }

    private IEnumerator Pattern1()
    {
        Debug.Log("Pattern 1: Pulling player towards center.");
        Vector3 center = Vector3.zero; // 맵 중앙
        float pullSpeed = 5f; 

       
        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 0; 

        while (hitsDuringPattern < 5)
        {
            
            player.position = Vector3.MoveTowards(player.position, center, pullSpeed * Time.deltaTime);

            
            yield return null; 
        }

        
        Vector3 randomOffset = Random.insideUnitCircle * 3f;
        transform.position = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        
        bossRB.gravityScale = 1; 
    }



    private IEnumerator Pattern2()
    {
        Debug.Log("Pattern 2: Creating platforms.");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 0; // 중력 비활성화

        Vector3 initialPlayerPosition = player.position;
        Vector3 initialBossPosition = Boss.position;

        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(false);
        }

        HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

        // 처음 생성할 발판 위치
        for (int j = 0; j < 3; j++)
        {
            Vector3 spawnPosition = new Vector3(player.position.x, player.position.y - 5, player.position.z);
            occupiedPositions.Add(spawnPosition);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            Destroy(platform, 5f);
        }

        // hitsDuringPattern이 5가 될 때까지 발판 생성
        while (hitsDuringPattern < 5)
        {
            // 플레이어의 Y 위치 체크 및 처리
            if (player.position.y < -40)
            {
                TakeDamage(5); // 데미지 입히기
                MoveToPlatform(); // 현재 발판 위로 이동
            }

            for (int j = 0; j < 5; j++)
            {
                Vector3 spawnPosition;
                int attempt = 0;
                bool positionFound = false;

                while (attempt < 10) // 최대 10번 시도
                {
                    float offsetX = Random.Range(-3f, 3f);
                    float offsetY = Random.Range(-3f, 3f);

                    // 발판 생성 위치 계산
                    spawnPosition = player.position + new Vector3(offsetX, offsetY, 0);

                    // 주변 범위에 발판이 없는지 확인
                    if (!IsPositionOccupied(spawnPosition, occupiedPositions))
                    {
                        occupiedPositions.Add(spawnPosition); // 유효한 위치 추가
                        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                        Destroy(platform, 4f);
                        positionFound = true;
                        break;
                    }

                    attempt++;
                }

                if (!positionFound)
                {
                    Debug.LogWarning("Could not find a valid spawn position for the platform after 10 attempts.");
                }
            }

            yield return new WaitForSeconds(3f);

            Vector3 randomOffset = Random.insideUnitCircle * Random.Range(5f, 8f);
            transform.position = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        }

        // 중력 스케일을 원래 값으로 복원
        bossRB.gravityScale = 1;

        // 비활성화했던 ground 레이어의 오브젝트 다시 활성화
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }

        // 플레이어를 초기 위치로 이동 (Y 방향으로 3 위로)
        player.position = new Vector3(initialPlayerPosition.x, initialPlayerPosition.y + 3, initialPlayerPosition.z);
        Boss.position = new Vector3(initialBossPosition.x, initialBossPosition.y + 3, initialBossPosition.z);
    }

    // 주어진 위치 주변에 발판이 있는지 확인하는 메서드
    private bool IsPositionOccupied(Vector3 position, HashSet<Vector3> occupiedPositions)
    {
        // 기본 위치와 주변 범위 2를 고려
        for (int xOffset = -5; xOffset <= 5; xOffset++)
        {
            for (int yOffset = -5; yOffset <= 5; yOffset++)
            {
                Vector3 checkPosition = position + new Vector3(xOffset, yOffset, 0);
                if (occupiedPositions.Contains(checkPosition))
                {
                    return true; // 주변 범위에 발판이 존재
                }
            }
        }
        return false; // 주변 범위에 발판이 없음
    }

    // 플레이어를 현재 발판 위로 이동시키는 메서드
    private void MoveToPlatform()
    {
        // 발판 찾기 
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform"); // 발판의 태그가 "Platform"이라고 가정
        if (platforms.Length > 0)
        {
            // 마지막으로 생성된 발판 위로 플레이어 이동
            Vector3 lastPlatformPosition = platforms[platforms.Length - 1].transform.position;
            player.position = new Vector3(lastPlatformPosition.x, lastPlatformPosition.y + 1f, lastPlatformPosition.z); // 발판 위로 이동
        }
    }



    private IEnumerator Pattern3()
    {
        Debug.Log("Pattern 3");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();

        bossRB.gravityScale = 1;

        // 보스의 위치에서 각각 x방향으로 -1, +1 만큼 떨어진 곳에 오브젝트 활성화
        Vector3 position1 = transform.position + new Vector3(-2, 1, 0);
        Vector3 position2 = transform.position + new Vector3(2, 1, 0);
        Vector3 position3 = transform.position + new Vector3(-10, 0, 0);

        object1.transform.position = position1; // object1 위치 설정
        object2.transform.position = position2; // object2 위치 설정
        player.transform.position = position3; // 플레이어 위치 설정

        object1.SetActive(true);
        object2.SetActive(true);
        Pattern3Room.SetActive(true);
        yield return new WaitForSeconds(5f);

        object1.SetActive(false);
        object2.SetActive(false);
        Pattern3Room.SetActive(false);
    }
    private IEnumerator Pattern4()
    {
        Debug.Log("Pattern 4: Boss moves along X-axis.");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 1;

        float initialY = player.position.y;
        float minX = -10f;
        float maxX = 10f;
        yield return new WaitForSeconds(3f);

        for (float x = minX; x <= maxX; x += moveSpeed * 5 * Time.deltaTime)
        {
            transform.position = new Vector3(x, initialY, transform.position.z);
            if (Vector3.Distance(transform.position, player.position) <= 1f)
            {
                player.GetComponent<Health>().TakeDamage(5f);
            }
            yield return null;
        }
    }

    private IEnumerator Pattern5()
    {
        Debug.Log("Pattern 5: Player is immobilized.");
        

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 1; 


        // 플레이어 Y 위치 이동
        player.position += new Vector3(0, 5, 0);

        // 플레이어의 Rigidbody2D를 잠금 (이동을 방지)
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        yield return new WaitForSeconds(3f); // 조작 불가 시간

        // 플레이어의 Rigidbody2D 제약 해제
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints2D.None;
        }

        Coroutine sphereCoroutine = StartCoroutine(SpawnSpheres()); // 구형 오브젝트 생성 코루틴 시작

        while (hitsDuringPattern < 5) // 보스에게 5번 타격할 때까지 대기
        {
            yield return null;
        }

        // 패턴이 끝나면 sphere 생성 멈춤
        StopCoroutine(sphereCoroutine);


    }


    private IEnumerator SpawnSpheres()
    {
        while (true)
        {
            GameObject sphere = Instantiate(spherePrefab, player.position, Quaternion.identity);
            float randomSize = Random.Range(1f, 3f);
            sphere.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            sphere.GetComponent<Renderer>().material.color = Color.yellow; // 초기 색상 설정

            // 플레이어와의 접촉 처리
            StartCoroutine(CheckPlayerCollision(sphere));

            Destroy(sphere, 7f); // 5초 후에 구형 오브젝트 제거
            yield return new WaitForSeconds(2f); // 3초마다 생성
        }
    }

    private IEnumerator CheckPlayerCollision(GameObject sphere)
    {
        float contactTime = 0f;
        bool playerInContact = false;

        while (sphere != null)
        {
            if (Vector3.Distance(sphere.transform.position, player.position) < 1f)
            {
                if (!playerInContact)
                {
                    playerInContact = true;
                    //playermovement.moveSpeed *= 0.7f; // 이동 속도 30% 감소
                }

                contactTime += Time.deltaTime;

                if (contactTime >= 1.5f)
                {
                    sphere.GetComponent<Renderer>().material.color = Color.red; // 3초 후 색상 변경
                    player.GetComponent<Health>().TakeDamage(5f); // 플레이어에게 5의 데미지 주기
                }
            }
            else
            {
                if (playerInContact)
                {
                    playerInContact = false;
                    //playermovement.moveSpeed /= 0.7f; // 이동 속도 복구
                    contactTime = 0f; // 접촉 시간 초기화
                }
            }

            yield return null;
        }

        // 구형 오브젝트가 파괴될 때 이동 속도 복구
        if (playerInContact)
        {
            playerInContact = false;
            moveSpeed /= 0.7f; // 이동 속도 복구
        }
    }


    private IEnumerator DefensePattern()
    {
        int randomDefensePattern = Random.Range(1, 3); // 1 또는 2 선택

        if (randomDefensePattern == 1)
        {
            // Defense Pattern 1: 플레이어에게 받은 데미지 * 2 만큼 회복
            Debug.Log("Defense Pattern 1: Healing.");
            currentHealth += damageReceived * 2;
            currentHealth = Mathf.Min(currentHealth, maxHealth); 
            yield return new WaitForSeconds(5f); 
        }
        else if (randomDefensePattern == 2)
        {
            // Defense Pattern 2: 플레이어에게 받은 데미지 / 2 만큼 플레이어에게 데미지 주기
            Debug.Log("Defense Pattern 2: Damaging player.");
            float damageToPlayer = damageReceived / 2;
            player.GetComponent<Health>().TakeDamage(damageToPlayer);
            yield return new WaitForSeconds(5f); 
        }
    }

    //피격 시스템 부재 -> 3초마다 데미지 입는 메서드
    private IEnumerator DecreaseHealthOverTime()
    {
        while (currentHealth > 0)
        {
            TakeDamage(1f); 
            yield return new WaitForSeconds(3f); 
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hitsDuringPattern++;
        damageReceived += damage; 
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died.");
        Destroy(gameObject);
    }
}
