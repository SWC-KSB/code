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
         

        // ������Ʈ ��Ȱ��ȭ
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

        // �ð� ��� üũ
        timeInCurrentState += Time.deltaTime;

        
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
                if (hitsDuringPattern >= 5) // �������� 5�� ������ ���� ��ȯ
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
                    if (patternsPerformed >= 3) // ������ 3�� �������� ��
                    {
                        currentState = BossState.Idle;
                    }
                }
                break;

            case BossState.Defense:
                currentState = BossState.Attack; // ��� ���¿��� ���� ���·� ��ȯ
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
                // ���� ���� ����
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
        bossRB.gravityScale = 0; // �߷� ��Ȱ��ȭ

        Vector3 initialPlayerPosition = player.position;
        Vector3 initialBossPosition = Boss.position;

        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(false);
        }

        HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

        // ó�� ������ ���� ��ġ
        for (int j = 0; j < 3; j++)
        {
            Vector3 spawnPosition = new Vector3(player.position.x, player.position.y - 5, player.position.z);
            occupiedPositions.Add(spawnPosition);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            Destroy(platform, 5f);
        }

        // hitsDuringPattern�� 5�� �� ������ ���� ����
        while (hitsDuringPattern < 5)
        {
            // �÷��̾��� Y ��ġ üũ �� ó��
            if (player.position.y < -40)
            {
                TakeDamage(5); // ������ ������
                MoveToPlatform(); // ���� ���� ���� �̵�
            }

            for (int j = 0; j < 5; j++)
            {
                Vector3 spawnPosition;
                int attempt = 0;
                bool positionFound = false;

                while (attempt < 10) // �ִ� 10�� �õ�
                {
                    float offsetX = Random.Range(-3f, 3f);
                    float offsetY = Random.Range(-3f, 3f);

                    // ���� ���� ��ġ ���
                    spawnPosition = player.position + new Vector3(offsetX, offsetY, 0);

                    // �ֺ� ������ ������ ������ Ȯ��
                    if (!IsPositionOccupied(spawnPosition, occupiedPositions))
                    {
                        occupiedPositions.Add(spawnPosition); // ��ȿ�� ��ġ �߰�
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

        // �߷� �������� ���� ������ ����
        bossRB.gravityScale = 1;

        // ��Ȱ��ȭ�ߴ� ground ���̾��� ������Ʈ �ٽ� Ȱ��ȭ
        foreach (var groundObject in groundObjects)
        {
            groundObject.SetActive(true);
        }

        // �÷��̾ �ʱ� ��ġ�� �̵� (Y �������� 3 ����)
        player.position = new Vector3(initialPlayerPosition.x, initialPlayerPosition.y + 3, initialPlayerPosition.z);
        Boss.position = new Vector3(initialBossPosition.x, initialBossPosition.y + 3, initialBossPosition.z);
    }

    // �־��� ��ġ �ֺ��� ������ �ִ��� Ȯ���ϴ� �޼���
    private bool IsPositionOccupied(Vector3 position, HashSet<Vector3> occupiedPositions)
    {
        // �⺻ ��ġ�� �ֺ� ���� 2�� ���
        for (int xOffset = -5; xOffset <= 5; xOffset++)
        {
            for (int yOffset = -5; yOffset <= 5; yOffset++)
            {
                Vector3 checkPosition = position + new Vector3(xOffset, yOffset, 0);
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
        // ���� ã�� 
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform"); // ������ �±װ� "Platform"�̶�� ����
        if (platforms.Length > 0)
        {
            // ���������� ������ ���� ���� �÷��̾� �̵�
            Vector3 lastPlatformPosition = platforms[platforms.Length - 1].transform.position;
            player.position = new Vector3(lastPlatformPosition.x, lastPlatformPosition.y + 1f, lastPlatformPosition.z); // ���� ���� �̵�
        }
    }



    private IEnumerator Pattern3()
    {
        Debug.Log("Pattern 3");

        Rigidbody2D bossRB = GetComponent<Rigidbody2D>();

        bossRB.gravityScale = 1;

        // ������ ��ġ���� ���� x�������� -1, +1 ��ŭ ������ ���� ������Ʈ Ȱ��ȭ
        Vector3 position1 = transform.position + new Vector3(-2, 1, 0);
        Vector3 position2 = transform.position + new Vector3(2, 1, 0);
        Vector3 position3 = transform.position + new Vector3(-10, 0, 0);

        object1.transform.position = position1; // object1 ��ġ ����
        object2.transform.position = position2; // object2 ��ġ ����
        player.transform.position = position3; // �÷��̾� ��ġ ����

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
            yield return null;
        }

        // ������ ������ sphere ���� ����
        StopCoroutine(sphereCoroutine);


    }


    private IEnumerator SpawnSpheres()
    {
        while (true)
        {
            GameObject sphere = Instantiate(spherePrefab, player.position, Quaternion.identity);
            float randomSize = Random.Range(1f, 3f);
            sphere.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            sphere.GetComponent<Renderer>().material.color = Color.yellow; // �ʱ� ���� ����

            // �÷��̾���� ���� ó��
            StartCoroutine(CheckPlayerCollision(sphere));

            Destroy(sphere, 7f); // 5�� �Ŀ� ���� ������Ʈ ����
            yield return new WaitForSeconds(2f); // 3�ʸ��� ����
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
                    //playermovement.moveSpeed *= 0.7f; // �̵� �ӵ� 30% ����
                }

                contactTime += Time.deltaTime;

                if (contactTime >= 1.5f)
                {
                    sphere.GetComponent<Renderer>().material.color = Color.red; // 3�� �� ���� ����
                    player.GetComponent<Health>().TakeDamage(5f); // �÷��̾�� 5�� ������ �ֱ�
                }
            }
            else
            {
                if (playerInContact)
                {
                    playerInContact = false;
                    //playermovement.moveSpeed /= 0.7f; // �̵� �ӵ� ����
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

    //�ǰ� �ý��� ���� -> 3�ʸ��� ������ �Դ� �޼���
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
