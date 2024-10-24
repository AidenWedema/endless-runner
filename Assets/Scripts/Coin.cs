using UnityEngine;

public class Coin : Obstacle
{
    void Start()
    {
        hitbox = gameObject.GetComponent<Collider2D>();
        if (hitbox == null)
            hitbox = gameObject.AddComponent<BoxCollider2D>();
        roadPosition = GameManager.Instance.roadManager.GetRoadPosition(currentRoad);
        transform.position = new Vector2(roadPosition, transform.position.y);

        particles = GameObject.Find("Die").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        Move();

        if (TouchingPlayer())
        {
            GameManager.Instance.score += 100;
            Die();
        }
    }
}
