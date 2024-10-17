using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]private Collider2D hitbox;
    public bool breakable;
    public int currentRoad;
    [SerializeField]private float roadPosition;
    [SerializeField]private float speed = 5f;
    [SerializeField]private Color32 color;
    private ParticleSystem particles;

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
}
