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
    public Health health;
    public float maxHealth = 100f;
    public float currentHealth;
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
    public BossState currentState;

    public GameObject object1Prefab; // object1 프리팹
    public GameObject object2Prefab; // object2 프리팹
    public GameObject object3Prefab; // object1 프리팹
    public GameObject object4Prefab; // object2 프리팹
    public GameObject pattern3RoomPrefab; // Pattern3Room 프리팹

    private GameObject object1;
    private GameObject object2;
    private GameObject object4;
    private GameObject object3;
    private GameObject Pattern3Room;

    private Queue<Vector2> lastSpawnedPlatforms = new Queue<Vector2>(); // 마지막 10개의 발판 위치 저장


    private void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
        StartCoroutine(StateMachine());
        StartCoroutine(DecreaseHealthOverTime());

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.isKinematic = true;
        // 오브젝트 생성
        object1 = Instantiate(object1Prefab);
        object2 = Instantiate(object2Prefab);
        object3 = Instantiate(object1Prefab);
        object4 = Instantiate(object2Prefab);

        Pattern3Room = Instantiate(pattern3RoomPrefab);

        // 오브젝트 비활성화
        if (object1 != null) object1.SetActive(false);
        if (object2 != null) object2.SetActive(false);
        if (object3 != null) object1.SetActive(false);
        if (object4 != null) object2.SetActive(false);
        if (Pattern3Room != null) Pattern3Room.SetActive(false);
    }

    private void Update()
    {
        
        if (Vector3.Distance(transform.position, player.position) < 2.1f) 
        {
            player.GetComponent<Health>().TakeDamage(5f);
        }

        // 시간 경과 체크
        timeInCurrentState += Time.deltaTime;

        if (player.position.x < transform.position.x) // 플레이어가 보스의 왼쪽에 있는 경우
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 180도 회전
        }
        else // 플레이어가 보스의 오른쪽에 있는 경우
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // 원래 방향
        }

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
                    hitsDuringPattern = 0;
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

            Vector3 directionToPlayer1 = (player.position - transform.position).normalized;
            transform.position += directionToPlayer1 * moveSpeed * Time.deltaTime;

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

                    transform.position += directionToPlayer1 * moveSpeed * Time.deltaTime;

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
        float timeElapsed = 0f;
        float increaseInterval = 0.8f; // 증가 간격

        // 보스를 중앙으로 이동
        transform.position = center;
        yield return new WaitForSeconds(1f);
   
        // 플레이어에게 지속적으로 데미지 주기 시작
        StartCoroutine(DamagePlayerInZone());

        while (hitsDuringPattern < 5)
        {
            player.position = Vector3.MoveTowards(player.position, center, pullSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;

            // 3초가 지났으면 pullSpeed 증가
            if (timeElapsed >= increaseInterval)
            {
                pullSpeed += 1f; // 당기는 힘 증가
                timeElapsed = 0f; // 타이머 초기화
            }

            yield return null;
        }

        
        transform.position = center;

        yield return new WaitForSeconds(3f);
    }


    private IEnumerator DamagePlayerInZone()
    {
        while (hitsDuringPattern < 5) // 패턴이 진행되는 동안
        {
            if (Vector3.Distance(player.position, Vector3.zero) < 5f) // 중심 주변 5의 범위
            {
                player.GetComponent<Health>().TakeDamage(5f);
            }
            yield return new WaitForSeconds(1f); // 1초 대기
        }
    }



    private IEnumerator Pattern2()
    {
        Debug.Log("Pattern 2: Creating platforms.");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 0; // 중력 비활성화

        Vector2 initialPlayerPosition = player.position;
        Vector2 initialBossPosition = Boss.position;

        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(false);
        }

        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        // 처음 생성할 발판 위치
        for (int j = 0; j < 3; j++)
        {
            Vector2 spawnPosition = new Vector2(player.position.x, player.position.y - 5);
            occupiedPositions.Add(spawnPosition);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            Destroy(platform, 5f);
        }

        // hitsDuringPattern이 5가 될 때까지 발판 생성
        while (hitsDuringPattern < 5)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += (Vector3)(directionToPlayer * moveSpeed * Time.deltaTime);

            // 플레이어의 Y 위치 체크 및 처리
            if (player.position.y < -40)
            {
                TakeDamage(5); // 데미지 입히기
                MoveToPlatform(); // 현재 발판 위로 이동
            }

            Vector2 randomOffset = Random.insideUnitCircle * Random.Range(5f, 8f);
            transform.position = (Vector3)player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            for (int j = 0; j < 5; j++)
            {
                Vector2 spawnPosition;
                int attempt = 0;
                bool positionFound = false;

                while (attempt < 10) // 최대 10번 시도
                {
                    float offsetX = Random.Range(-10f, 10f);
                    float offsetY = Random.Range(-10f, 10f);
                    spawnPosition = (Vector2)player.position + new Vector2(offsetX, offsetY);

                    // 발판 생성 위치 계산
                    spawnPosition = (Vector2)player.position + new Vector2(offsetX, offsetY);

                    // 주변 범위에 발판이 없는지 확인
                    if (!IsPositionOccupied(spawnPosition, occupiedPositions) && !IsOverlappingWithLastPlatforms(spawnPosition))
                    {
                        occupiedPositions.Add(spawnPosition);
                        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                        Destroy(platform, 4f);

                        // 마지막 발판 위치 업데이트
                        lastSpawnedPlatforms.Enqueue(spawnPosition);
                        if (lastSpawnedPlatforms.Count > 10) // 최대 10개 저장
                        {
                            lastSpawnedPlatforms.Dequeue(); // 가장 오래된 발판 위치 제거
                        }

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

            yield return new WaitForSeconds(2f);

            
        }


        // 비활성화했던 ground 레이어의 오브젝트 다시 활성화
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }

        // 플레이어를 초기 위치로 이동 (Y 방향으로 3 위로)
        player.position = new Vector2(initialPlayerPosition.x, initialPlayerPosition.y + 3);
        Boss.position = new Vector2(initialBossPosition.x, initialBossPosition.y + 3);

        yield return new WaitForSeconds(3f);
    }

    // 주어진 위치 주변에 발판이 있는지 확인하는 메서드
    private bool IsPositionOccupied(Vector2 position, HashSet<Vector2> occupiedPositions)
    {
        // 기본 위치와 주변 범위 2를 고려
        for (int xOffset = -10; xOffset <= 10; xOffset++)
        {
            for (int yOffset = -10; yOffset <= 10; yOffset++)
            {
                Vector2 checkPosition = position + new Vector2(xOffset, yOffset);
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
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        Debug.Log($"Found {platforms.Length} platforms.");
        if (platforms.Length > 0)
        {
            Vector2 lastPlatformPosition = platforms[platforms.Length - 1].transform.position;
            Debug.Log($"Moving player to platform at position: {lastPlatformPosition}");
            player.position = new Vector2(lastPlatformPosition.x, lastPlatformPosition.y + 1f);
        }
        else
        {
            Debug.LogWarning("No platforms found to move the player to.");
        }
    }

    private bool IsOverlappingWithLastPlatforms(Vector2 newPosition)
    {
        float platformSize = 2f; // 발판의 반지름
        foreach (var lastPosition in lastSpawnedPlatforms)
        {
            if (Vector2.Distance(newPosition, lastPosition) < platformSize * 2)
            {
                return true; // 겹침 발견
            }
        }
        return false; // 겹침 없음
    }




    private IEnumerator Pattern3()
    {
        Debug.Log("Pattern 3");

        transform.position = new Vector3(0, 0, 0);

        Vector3 position3 = transform.position + new Vector3(-7, 0, 0);
        Vector3 position1 = transform.position + new Vector3(-2, 0, 0);
        Vector3 position2 = transform.position + new Vector3(2, 0, 0);
        Vector3 position4 = transform.position + new Vector3(-16, -9, 0);


        player.transform.position = position3; // 플레이어 위치 설정
        transform.position = new Vector2(position3.x + 7, position3.y + 2); //보스 위치 설정


        object1.transform.position = position1 + new Vector3(0,-2,0);
        object2.transform.position = position2 + new Vector3(0,-2,0);
        object3.transform.position = position1;
        object4.transform.position = position2;
        Pattern3Room.transform.position = position4;

                                                                  
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(false);
        }

        object1.SetActive(true);
        object2.SetActive(true);
        object3.SetActive(true);
        object4.SetActive(true);
        Pattern3Room.SetActive(true);
       
        while (hitsDuringPattern < 5) // 보스에게 5번 타격할 때까지 대기
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
            yield return null;
        }


        object1.SetActive(false);
        object2.SetActive(false);
        object3.SetActive(false);
        object4.SetActive(false);
        Pattern3Room.SetActive(false);

        player.transform.position = new Vector2(0, 2);
        transform.position = new Vector2(player.position.x + 5, player.position.y + 3);

        // 비활성화했던 ground 레이어의 오브젝트 다시 활성화
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }


        yield return new WaitForSeconds(3f);

        

    }
    private int pattern4Count = 0; // 패턴 실행 횟수 카운터

    private IEnumerator Pattern4()
    {
        // 패턴이 2번 실행되었는지 확인
        if (pattern4Count >= 2)
        {
            pattern4Count = 0;
            yield break; // 더 이상 실행하지 않음
        }

        Debug.Log($"Pattern 4: Execution count {pattern4Count + 1}");

        yield return new WaitForSeconds(1f);

        float initialY = player.position.y;
        float moveDirection = Random.Range(0f, 1f) < 0.5f ? 1f : -1f; // 1/2 확률로 방향 결정
        float minX = player.position.x - 20f; // 플레이어의 X 위치 기준으로 이동 범위 설정
        float maxX = player.position.x + 20f;


        // 시작 위치로 이동
        float startX = (moveDirection > 0 ? minX : maxX);
        transform.position = new Vector3(startX, initialY, transform.position.z);

        // 0.5초 대기
        yield return new WaitForSeconds(0.7f);

        // 이동 시작
        for (float x = startX;
             (moveDirection > 0 ? x <= maxX : x >= minX);
             x += moveSpeed * 11 * Time.deltaTime * moveDirection)
        {
            transform.position = new Vector3(x, initialY, transform.position.z);

            if (Vector3.Distance(transform.position, player.position) <= 2.1f)
            {
                player.GetComponent<Health>().TakeDamage(5f);
            }
            yield return null;
        }
        yield return new WaitForSeconds(3f);

        // 패턴 실행 횟수 증가
        pattern4Count++;

        // 1/2 확률로 다시 실행
        if (Random.Range(0f, 1f) < 0.5f)
        {
            yield return StartCoroutine(Pattern4()); // 재귀 호출로 다시 실행
        }
        else
        {
            pattern4Count = 0;
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
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // 패턴이 끝나면 sphere 생성 멈춤
        StopCoroutine(sphereCoroutine);
        yield return new WaitForSeconds(3f);

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

            Destroy(sphere, 5f); // 5초 후에 구형 오브젝트 제거
            yield return new WaitForSeconds(1f); // 3초마다 생성
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

                if (contactTime >= 1f)
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
