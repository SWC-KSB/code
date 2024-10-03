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

    public GameObject object1Prefab; // object1 ������
    public GameObject object2Prefab; // object2 ������
    public GameObject object3Prefab; // object1 ������
    public GameObject object4Prefab; // object2 ������
    public GameObject pattern3RoomPrefab; // Pattern3Room ������
    public GameObject pattern1effectPrefab; // pattern1 ������

    private GameObject object1;
    private GameObject object2;
    private GameObject object4;
    private GameObject object3;
    private GameObject Pattern3Room;

    private Queue<Vector2> lastSpawnedPlatforms = new Queue<Vector2>(); // ������ 10���� ���� ��ġ ����

    [Header("ȿ����")]
    public AudioClip FireSound;
    public AudioClip BackSound;
    public AudioClip BreakSound;
    public AudioClip DashSound;
    public AudioClip MakePlatformSound;
    private AudioSource audioSource;    // ����� �ҽ� ������Ʈ

    [Header("Hit Effect")]
    [SerializeField] private int numberOfFlashes = 3;  // �����̴� Ƚ��
    [SerializeField] private float flashDuration = 0.1f;  // �����̴� �ӵ�
    private SpriteRenderer spriteRender;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
private void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
        StartCoroutine(StateMachine());


        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.isKinematic = true;
        // ������Ʈ ����
        object1 = Instantiate(object1Prefab);
        object2 = Instantiate(object2Prefab);
        object3 = Instantiate(object3Prefab);
        object4 = Instantiate(object4Prefab);

        Pattern3Room = Instantiate(pattern3RoomPrefab);

        // ������Ʈ ��Ȱ��ȭ
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

        spriteRender = GetComponent<SpriteRenderer>();

        // �ð� ��� üũ
        timeInCurrentState += Time.deltaTime;

        if (player.position.x < transform.position.x) // �÷��̾ ������ ���ʿ� �ִ� ���
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 180�� ȸ��
        }
        else // �÷��̾ ������ �����ʿ� �ִ� ���
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // ���� ����
        }

    }

    private void SwitchState()
    {
        timeInCurrentState = 0f; // ���� ��ȯ �� �ð� �ʱ�ȭ
        switch (currentState)
        {
            case BossState.Idle:
                currentState = BossState.Attack;
                break;

            case BossState.Attack:
                if (hitsDuringPattern >= 3) // �������� 5�� ������ ���� ��ȯ
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
                    if (patternsPerformed >= 3) // ������ 3�� �������� ��
                    {
                        currentState = BossState.Idle;
                    }
                }
                break;

            case BossState.Defense:
                int randomValue = Random.Range(0, 2); // 0�� Attack, 1�� Pattern

                if (randomValue >= 1)
                {
                    currentState = BossState.Attack; // 50% Ȯ���� Attack���� ��ȯ
                }
                else
                {
                    currentState = BossState.Pattern; // 50% Ȯ���� Pattern���� ��ȯ
                }
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
    public GameObject bossAttackPrefab; // BossAttack �������� ������ ����



    private void PlayFireSound(float startTime, float duration)
    {
        audioSource.clip = FireSound;
        audioSource.time = startTime; // ���� �ð� ����
        audioSource.Play();
        Invoke("StopSound", duration); // ������ �ð� �Ŀ� ����
    }

    private void StopSound()
    {
        audioSource.Stop();
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
                // ���� ���� ����
                Vector3 attackPosition = player.position;
                GameObject bossAttack = Instantiate(bossAttackPrefab, attackPosition, Quaternion.identity);
                bossAttack.transform.localScale = new Vector3(1, 1, 1);
                Renderer bossAttackRenderer = bossAttack.GetComponent<Renderer>();
                bossAttackRenderer.material.color = Color.gray;

                PlayFireSound(0.5f, 1f); // 0.5�ʺ��� 1�ʰ� ���


                yield return new WaitForSeconds(1f);
                bossAttackRenderer.material.color = Color.white;

                float timer = 0f;
                while (timer < 0.5f)
                {
                    transform.position += directionToPlayer1 * moveSpeed * Time.deltaTime;

                    if (Vector3.Distance(bossAttack.transform.position, player.position) < 1f)
                    {
                        player.GetComponent<Health>().TakeDamage(5f);
                        PlayFireSound(5f, 1f); // 5�ʺ��� 1�ʰ� ���

                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                Destroy(bossAttack);
            }
            else
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
            }

            if (hitsDuringPattern >= 5) // �������� 5�� ������ ���� ��ȯ
            {
                currentState = BossState.Pattern; // ���� ���·� ��ȯ
                hitsDuringPattern = 0; // ���� ���� �� �ʱ�ȭ
                yield break; // AttackBehavior ����
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }



    private HashSet<int> executedPatterns = new HashSet<int>(); // ����� ������ �����ϴ� ����

    private IEnumerator PatternBehavior()
    {
        Debug.Log("Boss is performing a pattern.");
        isPerformingPattern = true; 

        int randomPattern;

        // ��� ������ ����� ������ �ݺ�
        while (executedPatterns.Count < 5)
        {
            randomPattern = Random.Range(1, 6); // 1���� 5������ ���� ���� ����

            // �̹� ����� �������� Ȯ��
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
                hitsDuringPattern = 0; // ���� ���� �� ����
                break; 
            }
        }

        // ��� ������ ����� ��� �ʱ�ȭ
        if (executedPatterns.Count >= 5)
        {
            executedPatterns.Clear(); // ����� ���� �ʱ�ȭ
        }

        isPerformingPattern = false; 
    }


    private IEnumerator DefenseBehavior()
    {
        Debug.Log("Boss is in defense mode.");
        yield return StartCoroutine(DefensePattern()); // ��� ���� ����
    }

    private IEnumerator Pattern1()
    {
        Debug.Log("Pattern 1: Pulling player towards center.");
        Vector3 center = Vector3.zero; // �� �߾�
        float pullSpeed = 5f;
        float timeElapsed = 0f;
        float increaseInterval = 1.1f; // ���� ����

        // ������ �߾����� �̵�
        transform.position = center;
        yield return new WaitForSeconds(1f);

        // ������ ����
        GameObject spawnedPrefab = Instantiate(pattern1effectPrefab, center, Quaternion.identity);

        // �÷��̾�� ���������� ������ �ֱ� ����
        StartCoroutine(DamagePlayerInZone());

        // BackSound ���
        PlayBackgroundSound(); // ���� ���

        while (hitsDuringPattern < 5)
        {
            player.position = Vector3.MoveTowards(player.position, center, pullSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;

            // 3�ʰ� �������� pullSpeed ����
            if (timeElapsed >= increaseInterval)
            {
                pullSpeed += 1f; // ���� �� ����
                timeElapsed = 0f; // Ÿ�̸� �ʱ�ȭ
            }

            yield return null;
        }

        // ������ ���� ��
        transform.position = center;
        yield return new WaitForSeconds(3f);

        // ������ ���� �� ������ ����
        Destroy(spawnedPrefab);

        // ���� ����
        StopBackgroundSound(); // ���� ����
    }

    private void PlayBackgroundSound()
    {
        audioSource.clip = BackSound; // BackSound Ŭ�� ����
        audioSource.loop = true; // �ݺ� ��� ����
        audioSource.Play(); // ���� ���
    }

    private void StopBackgroundSound()
    {
        audioSource.Stop(); // ���� ����
    }



    private IEnumerator DamagePlayerInZone()
    {
        while (hitsDuringPattern < 5) // ������ ����Ǵ� ����
        {
            if (Vector3.Distance(player.position, Vector3.zero) < 5f) // �߽� �ֺ� 5�� ����
            {
                player.GetComponent<Health>().TakeDamage(5f);
            }
            yield return new WaitForSeconds(1f); // 1�� ���
        }
    }



    private IEnumerator Pattern2()
    {
        Debug.Log("Pattern 2: Creating platforms.");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 0; // �߷� ��Ȱ��ȭ

        Vector2 initialPlayerPosition = player.position;
        Vector2 initialBossPosition = Boss.position;
        player.transform.position = new Vector2(0, 6);
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(false);
        }

        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        // ó�� ������ ���� ��ġ
        for (int j = 0; j < 3; j++)
        {
            Vector2 spawnPosition = new Vector2(player.position.x, player.position.y - 3);
            occupiedPositions.Add(spawnPosition);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            Destroy(platform, 5f);
        }

        // hitsDuringPattern�� 5�� �� ������ ���� ����
        while (hitsDuringPattern < 5)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += (Vector3)(directionToPlayer * moveSpeed * Time.deltaTime);

            // �÷��̾��� Y ��ġ üũ �� ó��
            if (player.position.y < -7)
            {
                player.GetComponent<Health>().TakeDamage(5f); MoveToPlatform(); // ���� ���� ���� �̵�
            }

            Vector2 randomOffset = Random.insideUnitCircle * Random.Range(5f, 8f);
            transform.position = (Vector3)player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            PlayMakePlatformSound(2, 3);

            for (int j = 0; j < 5; j++)
            {
                Vector2 spawnPosition;
                int attempt = 0;
                bool positionFound = false;

                while (attempt < 10) // �ִ� 10�� �õ�
                {
                    float offsetX = Random.Range(-10f, 10f);
                    float offsetY = Random.Range(-10f, 10f);
                    spawnPosition = (Vector2)player.position + new Vector2(offsetX, offsetY);

                    // ���� ���� ��ġ ���
                    spawnPosition = (Vector2)player.position + new Vector2(offsetX, offsetY);

                    // �ֺ� ������ ������ ������ Ȯ��
                    if (!IsPositionOccupied(spawnPosition, occupiedPositions) && !IsOverlappingWithLastPlatforms(spawnPosition))
                    {
                        occupiedPositions.Add(spawnPosition);
                        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                        Destroy(platform, 4f);

                        // ������ ���� ��ġ ������Ʈ
                        lastSpawnedPlatforms.Enqueue(spawnPosition);
                        if (lastSpawnedPlatforms.Count > 10) // �ִ� 10�� ����
                        {
                            lastSpawnedPlatforms.Dequeue(); // ���� ������ ���� ��ġ ����
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


        // ��Ȱ��ȭ�ߴ� ground ���̾��� ������Ʈ �ٽ� Ȱ��ȭ
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }

        // �÷��̾ �ʱ� ��ġ�� �̵� (Y �������� 3 ����)
        player.position = new Vector2(initialPlayerPosition.x, initialPlayerPosition.y + 3);
        Boss.position = new Vector2(initialBossPosition.x, initialBossPosition.y + 3);

        yield return new WaitForSeconds(3f);
    }

    // �־��� ��ġ �ֺ��� ������ �ִ��� Ȯ���ϴ� �޼���
    private bool IsPositionOccupied(Vector2 position, HashSet<Vector2> occupiedPositions)
    {
        // �⺻ ��ġ�� �ֺ� ���� 2�� ���
        for (int xOffset = -10; xOffset <= 10; xOffset++)
        {
            for (int yOffset = -10; yOffset <= 10; yOffset++)
            {
                Vector2 checkPosition = position + new Vector2(xOffset, yOffset);
                if (occupiedPositions.Contains(checkPosition))
                {
                    return true; // �ֺ� ������ ������ ����
                }
            }
        }
        return false; // �ֺ� ������ ������ ����
    }

    // �÷��̾ ���� ���� ���� �̵���Ű�� �޼���
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
        float platformSize = 2f; // ������ ������
        foreach (var lastPosition in lastSpawnedPlatforms)
        {
            if (Vector2.Distance(newPosition, lastPosition) < platformSize * 2)
            {
                return true; // ��ħ �߰�
            }
        }
        return false; // ��ħ ����
    }
    private void PlayMakePlatformSound(float startTime, float duration)
    {
        audioSource.clip = MakePlatformSound;
        audioSource.time = startTime; // ���� �ð� ����
        audioSource.Play();
        Invoke("StopSound", duration); // ������ �ð� �Ŀ� ����
    }



    private IEnumerator Pattern3()
    {
        Debug.Log("Pattern 3");

        transform.position = new Vector2(-4, 15);

        Vector3 position3 = transform.position;
        Vector3 position1 = transform.position;
        Vector3 position2 = transform.position;
        


        player.transform.position = position3; // �÷��̾� ��ġ ����
        

        object1.transform.position = position1 + new Vector3(0,-2,0);
        object2.transform.position = position2 + new Vector3(0,-2,0);
        object3.transform.position = position1;
        object4.transform.position = position2;
        

                                                                  
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
       
        while (hitsDuringPattern < 5) // �������� 5�� Ÿ���� ������ ���
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

        // ��Ȱ��ȭ�ߴ� ground ���̾��� ������Ʈ �ٽ� Ȱ��ȭ
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }


        yield return new WaitForSeconds(3f);

        

    }
    private int pattern4Count = 0; // ���� ���� Ƚ�� ī����

    private IEnumerator Pattern4()
    {
        // ������ 2�� ����Ǿ����� Ȯ��
        if (pattern4Count >= 2)
        {
            pattern4Count = 0;
            yield break; // �� �̻� �������� ����
        }

        Debug.Log($"Pattern 4: Execution count {pattern4Count + 1}");

        yield return new WaitForSeconds(1f);

        float initialY = player.position.y;
        float moveDirection = Random.Range(0f, 1f) < 0.5f ? 1f : -1f; // 1/2 Ȯ���� ���� ����
        float minX = player.position.x - 10f; // �÷��̾��� X ��ġ �������� �̵� ���� ����
        float maxX = player.position.x + 10f;


        // ���� ��ġ�� �̵�
        float startX = (moveDirection > 0 ? minX : maxX);
        transform.position = new Vector3(startX, initialY, transform.position.z);

        // 0.5�� ���
        yield return new WaitForSeconds(0.7f);
        PlayDashSound();
        // �̵� ����
        for (float x = startX;
             (moveDirection > 0 ? x <= maxX : x >= minX);
             x += moveSpeed * 9 * Time.deltaTime * moveDirection)
        {
            transform.position = new Vector3(x, initialY, transform.position.z);

            if (Vector3.Distance(transform.position, player.position) <= 2.1f)
            {
                player.GetComponent<Health>().TakeDamage(5f);
            }
            yield return null;
        }
        StopDashSound();
        yield return new WaitForSeconds(3f);

        // ���� ���� Ƚ�� ����
        pattern4Count++;

        // 1/2 Ȯ���� �ٽ� ����
        if (Random.Range(0f, 1f) < 0.5f)
        {
            yield return StartCoroutine(Pattern4()); // ��� ȣ��� �ٽ� ����
        }
        else
        {
            pattern4Count = 0;
            yield return null; 
        }
    }
    private void PlayDashSound()
    {
        audioSource.clip = DashSound; // BackSound Ŭ�� ����
        audioSource.Play(); // ���� ���
    }
    private void StopDashSound()
    {
        audioSource.Stop();
    }


    private IEnumerator Pattern5()
    {
        Debug.Log("Pattern 5: Player is immobilized.");
        

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();
        bossRB.gravityScale = 1; 


        // �÷��̾� Y ��ġ �̵�
        player.position += new Vector3(0, 5, 0);

        // �÷��̾��� Rigidbody2D�� ��� (�̵��� ����)
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        yield return new WaitForSeconds(3f); // ���� �Ұ� �ð�

        // �÷��̾��� Rigidbody2D ���� ����
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints2D.None;
        }

        Coroutine sphereCoroutine = StartCoroutine(SpawnSpheres()); // ���� ������Ʈ ���� �ڷ�ƾ ����

        while (hitsDuringPattern < 5) // �������� 5�� Ÿ���� ������ ���
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // ������ ������ sphere ���� ����
        StopCoroutine(sphereCoroutine);
        yield return new WaitForSeconds(3f);

    }


    private IEnumerator SpawnSpheres()
    {
        while (true)
        {
            GameObject sphere = Instantiate(spherePrefab, player.position, Quaternion.identity);
            float randomSize = Random.Range(1f, 4f);
            sphere.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            PlayBreakSound(0,2);
            
            // �÷��̾���� ���� ó��
            StartCoroutine(CheckPlayerCollision(sphere));

            Destroy(sphere, 3f); 
            yield return new WaitForSeconds(3f); // 3�ʸ��� ����
        }
    }


    private void PlayBreakSound(float startTime, float duration)
    {
        audioSource.clip = BreakSound;
        audioSource.time = startTime; // ���� �ð� ����
        audioSource.Play();
        Invoke("StopSound", duration); // ������ �ð� �Ŀ� ����
    }

    private IEnumerator CheckPlayerCollision(GameObject sphere)
    {
        float contactTime = 0f;
        bool playerInContact = false;

        while (sphere != null)
        {
            if (Vector3.Distance(sphere.transform.position, player.position) < 1f)
            {

                contactTime += Time.deltaTime;

                if (contactTime >= 0.3f)
                {
                    
                    player.GetComponent<Health>().TakeDamage(5f); // �÷��̾�� 5�� ������ �ֱ�
                }
            }
            else
            {
                if (playerInContact)
                {
                    playerInContact = false;
                    
                    contactTime = 0f; // ���� �ð� �ʱ�ȭ
                }
            }

            yield return null;
        }

        // ���� ������Ʈ�� �ı��� �� �̵� �ӵ� ����
        if (playerInContact)
        {
            playerInContact = false;
            moveSpeed /= 0.7f; // �̵� �ӵ� ����
        }
    }


    private IEnumerator DefensePattern()
    {
        int randomDefensePattern = Random.Range(1, 3); // 1 �Ǵ� 2 ����

        if (randomDefensePattern == 1)
        {
            // Defense Pattern 1: �÷��̾�� ���� ������ * 2 ��ŭ ȸ��
            Debug.Log("Defense Pattern 1: Healing.");
            currentHealth += damageReceived * 2;
            currentHealth = Mathf.Min(currentHealth, maxHealth); 
            yield return new WaitForSeconds(5f); 
        }
        else if (randomDefensePattern == 2)
        {
            // Defense Pattern 2: �÷��̾�� ���� ������ / 2 ��ŭ �÷��̾�� ������ �ֱ�
            Debug.Log("Defense Pattern 2: Damaging player.");
            float damageToPlayer = damageReceived / 2;
            player.GetComponent<Health>().TakeDamage(damageToPlayer);
            yield return new WaitForSeconds(5f); 
        }
    }



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hitsDuringPattern++;
        damageReceived += damage;
        if (currentHealth > 0)
        {
            StartCoroutine(FlashRed());  // �����Ÿ��� ȿ��
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator FlashRed()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 1);  // ���������� ����
            yield return new WaitForSeconds(flashDuration);  // �����̴� �ð�
            spriteRender.color = Color.white;  // ���� ������ ���ƿ�
            yield return new WaitForSeconds(flashDuration);  // �����̴� �ð�
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died.");
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        
        Renderer renderer = GetComponent<Renderer>();
        Color color = renderer.material.color;

        float fadeDuration = 1f; // Fadeout �ð� (��)
        float startAlpha = color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            color.a = alpha;
            renderer.material.color = color;
            yield return null; // ���� �����ӱ��� ���
        }

        // ���������� ���� ���� 0���� ����
        color.a = 0;
        renderer.material.color = color;

        // ���� ������Ʈ ����
        Destroy(gameObject);
    }
}
