using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]private Collider2D hitbox;
    [SerializeField]private int currentRoad;
    [SerializeField]private float roadPosition;
    private Vector2 homePosition;
    private Vector2 startTouch;
    private Vector2 endTouch;
    private float speed = 5f;
    private float attackSpeed = 10f;
    private bool attacking;
    private float iTimer;

    void Start()
    {
        hitbox = gameObject.GetComponent<Collider2D>();
        if (hitbox == null)
            hitbox = gameObject.AddComponent<BoxCollider2D>();
        currentRoad = RoadManager.Instance.roadAmount / 2;
        roadPosition = RoadManager.Instance.GetRoadPosition(currentRoad);
        homePosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        iTimer -= Time.deltaTime;

        if (Time.frameCount % (Application.targetFrameRate / 10) == 0)
            GameManager.Instance.score++;

        if (transform.position.x != roadPosition)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(roadPosition, transform.position.y), speed * Time.deltaTime);

        if (TouchingObstacle(out Collider2D collider))
        {
            if (attacking && collider.GetComponent<Obstacle>().breakable)
            {
                collider.gameObject.GetComponent<Obstacle>().Die();
                return;
            }
            SceneManager.LoadScene(0);
        }

        if (attacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, homePosition.y + 2), attackSpeed * Time.deltaTime);
            if (transform.position.y == homePosition.y + 2)
            {
                attacking = false;
            }
        }
        else if (transform.position.y != homePosition.y)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, homePosition.y), attackSpeed / 3 * Time.deltaTime);

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startTouch = touch.position;
                endTouch = touch.position;
                break;

            case TouchPhase.Moved:
                endTouch = touch.position;
                GameManager.SwipeDirection swipe = GameManager.Instance.CheckSwipeDirection(startTouch, endTouch, 100f);
                if (swipe != GameManager.SwipeDirection.None && transform.position.x == roadPosition)
                {
                    startTouch = touch.position;
                    HandleSwipe(swipe);
                }
                break;

            case TouchPhase.Ended:
                endTouch = touch.position;
                break;
        }
    }

    private void HandleSwipe(GameManager.SwipeDirection swipe)
    {
        switch (swipe)
        {
            case GameManager.SwipeDirection.Up:
                Bash();
                break;

            case GameManager.SwipeDirection.Down:
                break;

            case GameManager.SwipeDirection.Left:
                Move(-1);
                break;

            case GameManager.SwipeDirection.Right:
                Move(1);
                break;
        }
    }

    private void Move(int dir)
    {
        currentRoad = Mathf.Clamp(currentRoad + dir, 0, RoadManager.Instance.roadAmount - 1);
        roadPosition = RoadManager.Instance.GetRoadPosition(currentRoad);
    }

    private void Bash()
    {
        if (transform.position.y != homePosition.y)
            return;

        attacking = true;
    }

    private bool TouchingObstacle()
    {
        return TouchingObstacle(out _);
    }

    private bool TouchingObstacle(out Collider2D collider)
    {
        Collider2D[] colliders = new Collider2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.GetMask("Obstacle");
        filter.useLayerMask = true;
        bool result = Physics2D.OverlapCollider(hitbox, filter, colliders) != 0;
        collider = colliders[0];
        return result;
    }

    public float Speed { get { return speed; }}
}
