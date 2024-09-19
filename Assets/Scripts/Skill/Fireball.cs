using UnityEngine;

public class Fireball : MonoBehaviour
{
    // �ӵ�
    [SerializeField] private float speed;
    // �¾Ҵ��� Ȯ���ϴ� ����
    private bool hit;
    // ����
    private float direction;
    // ���̾ �ð�
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    // ������ ����
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
