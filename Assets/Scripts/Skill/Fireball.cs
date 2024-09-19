using UnityEngine;

public class Fireball : MonoBehaviour
{
    // 속도
    [SerializeField] private float speed;
    // 맞았는지 확인하는 변수
    private bool hit;
    // 방향
    private float direction;
    // 파이어볼 시간
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    // 변수에 저장
    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0,0);

        lifetime += Time.deltaTime;
        if (lifetime > 3.0f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explosion");
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0.0f;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y,
            transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
