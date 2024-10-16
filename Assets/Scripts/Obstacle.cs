using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]private BoxCollider2D hitbox;
    public bool breakable { get; private set; }
    [SerializeField] private int currentRoad;
    [SerializeField] private float roadPosition;
    private float speed = 5f;

    void Start()
    {
        hitbox = transform.GetOrAddComponent<BoxCollider2D>();
        breakable = false;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
    }
}
