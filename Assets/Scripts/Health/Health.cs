using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리를 위해 추가

public class Health : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] public float InitialHealth;
    public float currentHealth { get; set; }
    private Animator anim;
    private bool dead;

    [Header("iFrame")]
    [SerializeField] private float iFrameDuration;  // 무적 시간
    [SerializeField] private int numberOfFlashes;   // 깜박이는 횟수
    private SpriteRenderer spriteRender;
    private bool isInvulnerable = false;            // 무적 상태 플래그

    [Header("효과음")]
    public AudioClip damageSound;                   // 데미지 입을 때 효과음
    private AudioSource audioSource;                // 오디오 소스 컴포넌트

    [Header("게임 오버 설정")]
    [SerializeField] private string gameOverSceneName = "GameOverScene";  // 게임 오버 씬 이름
    [SerializeField] private float sceneTransitionDelay = 2f;  // 씬 전환 전 딜레이

    private void Awake()
    {
        currentHealth = InitialHealth;
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        // AudioSource 컴포넌트 가져오기 또는 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 데미지 받는 함수
    public void TakeDamage(float damage)
    {
        // 무적 상태가 아닐 때만 데미지 받음
        if (!isInvulnerable)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, InitialHealth);

            if (currentHealth > 0)
            {
                // 플레이어가 다쳤음
                anim.SetTrigger("Hurt");

                // 데미지 입는 소리 재생
                if (damageSound != null)
                {
                    audioSource.PlayOneShot(damageSound);
                }

                StartCoroutine(Invulnerability());  // 무적 상태 시작
            }
            else
            {
                if (!dead)
                {
                    // 플레이어가 죽었을 때
                    anim.SetTrigger("Die");
                    GetComponent<PlayerMovement>().enabled = false;
                    dead = true;

                    // 일정 시간이 지나면 Die 함수 호출
                    StartCoroutine(DieWithDelay());
                }
            }
        }
    }

    // 이 함수가 호출되면 현재 씬이 다시 로드됩니다.
    private IEnumerator DieWithDelay()
    {
        // 씬을 다시 로드하기 전에 딜레이를 줍니다.
        yield return new WaitForSeconds(sceneTransitionDelay);

        // 현재 활성화된 씬을 불러오고 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // 무적 상태 유지 (데미지 제한, 충돌은 허용)
    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }

        isInvulnerable = false;  // 무적 상태 해제
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, InitialHealth);
    }
}
