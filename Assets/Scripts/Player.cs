using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private BoxCollider2D hitbox;
    [SerializeField]private int currentRoad;
    [SerializeField]private float roadPosition;
    private Vector2 homePosition;
    private Vector2 startTouch;
    private Vector2 endTouch;
    private float speed = 5f;
    private float bashCooldown = 3;
    private float bashTimer = 0;

    void Start()
    {
        hitbox = transform.GetOrAddComponent<BoxCollider2D>();
        currentRoad = GameManager.Instance.roadAmount / 2;
        roadPosition = GameManager.Instance.GetRoadPosition(currentRoad);
        homePosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        if (transform.position.x != roadPosition)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(roadPosition, transform.position.y), speed * Time.deltaTime);

            if (transform.position.y != homePosition.y)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, homePosition.y), speed * Time.deltaTime);

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
                if (swipe != GameManager.SwipeDirection.None)
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
        currentRoad = Mathf.Clamp(currentRoad + dir, 0, GameManager.Instance.roadAmount - 1);
        roadPosition = GameManager.Instance.GetRoadPosition(currentRoad);
    }

    private void Bash()
    {
        if (bashTimer > 0)
            return;

        bashTimer = bashCooldown;
    }
}
