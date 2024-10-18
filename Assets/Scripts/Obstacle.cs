using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]protected Collider2D hitbox;
    public bool breakable;
    public int currentRoad;
    [SerializeField]protected float roadPosition;
    [SerializeField]protected float speed = 3f;
    [SerializeField]protected Color32 color;
    protected ParticleSystem particles;

    void Start()
    {
        hitbox = gameObject.GetComponent<Collider2D>();
        if (hitbox == null)
            hitbox = gameObject.AddComponent<BoxCollider2D>();
        roadPosition = GameManager.Instance.roadManager.GetRoadPosition(currentRoad);
        transform.position = new Vector2(roadPosition, transform.position.y);

        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        particles = GameObject.Find("Die").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        Move();
    }

    public void Move()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);

        if (transform.position.y < GameManager.Instance.GetWorldFromScreenPosition(0, -0.5f).y)
            Destroy(gameObject);
    }

    public void Die()
    {
        particles.Stop();
        particles.transform.position = transform.position;
        particles.startColor = color;
        particles.Play();
        Destroy(gameObject);
    }

    protected bool TouchingPlayer()
    {
        Collider2D[] colliders = new Collider2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.GetMask("Player");
        filter.useLayerMask = true;
        bool result = Physics2D.OverlapCollider(hitbox, filter, colliders) != 0;
        return result;
    }
}
