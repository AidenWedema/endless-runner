using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoadManager roadManager;
    public GameState gameState;
    public Vector2Int resolution;

    public enum GameState
    {
        MainMenu,
        Running,
        Paused
    }

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    // Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        roadManager = RoadManager.Instance;
        if (roadManager == null)
            roadManager = gameObject.AddComponent<RoadManager>();
<<<<<<< Updated upstream
=======

        guiManager = GuiManager.Instance;
        if (guiManager == null)
            guiManager = gameObject.AddComponent<GuiManager>();


        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
>>>>>>> Stashed changes
    }

    // Functions
    public Vector2 GetScreenResolution()
    {
        resolution = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
        return resolution;
    }

    public Vector2 GetScreenPosition(Vector2 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }

    public Vector2 GetScreenPosition(float xPercetage, float yPercetage)
    {
        if (resolution == Vector2Int.zero)
            GetScreenResolution();

        return new Vector2(resolution.x * xPercetage, resolution.y * yPercetage);
    }

    public Vector2 GetWorldFromScreenPosition(float xPercetage, float yPercetage)
    {
        return Camera.main.ScreenToWorldPoint(GetScreenPosition(xPercetage, yPercetage));
    }

    public SwipeDirection CheckSwipeDirection(Vector2 startPosition, Vector2 endPosition, float threshold = 0f)
    {
        float vSwipe = Mathf.Abs(startPosition.y - endPosition.y);
        float hSwipe = Mathf.Abs(startPosition.x - endPosition.x);

        if (vSwipe < threshold && hSwipe < threshold)
            return SwipeDirection.None;

        if (vSwipe > hSwipe)
        {
            if (startPosition.y > endPosition.y)
                return SwipeDirection.Down;
            return SwipeDirection.Up;
        }
        if (startPosition.x > endPosition.x)
            return SwipeDirection.Left;
        return SwipeDirection.Right;
    }
}
