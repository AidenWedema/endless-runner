using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int roadAmount = 3;
    public float roadWidth = 1.1f;
    private bool newRoad = false;
    private float newRoadPosition = 0f;
    
    // Singleton
    public static RoadManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        if (!TryGetComponent<LineRenderer>(out lineRenderer))
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = Resources.Load<Material>("Materials/Road");
            lineRenderer.textureMode = LineTextureMode.Tile;
            lineRenderer.alignment = LineAlignment.TransformZ;
            lineRenderer.useWorldSpace = true;
            lineRenderer.sortingOrder = -10;
        }

        DrawRoads();
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        /*if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
        { 
            if (Random.value <= 0.5f && roadAmount > 3)
                RemoveRoad();
            else
                AddRoad();
        }*/

        if (!newRoad)
            return;

        newRoadPosition += GameManager.Instance.player.Speed * Time.deltaTime;

        float x1 = GetRoadPosition(roadAmount - 1);
        float x2 = GetRoadPosition(roadAmount - 2);
        float left = GameManager.Instance.GetWorldFromScreenPosition(-0.5f, 0).x;
        float y1 = GameManager.Instance.GetWorldFromScreenPosition(0, 1.5f).y;
        float y2 = GameManager.Instance.GetWorldFromScreenPosition(0, -0.5f).y;

        lineRenderer.SetPosition(lineRenderer.positionCount - 6, new Vector3(x1, y1, 0));
        lineRenderer.SetPosition(lineRenderer.positionCount - 5, new Vector3(x1, newRoadPosition, 0));
        lineRenderer.SetPosition(lineRenderer.positionCount - 4, new Vector3(x2, newRoadPosition - 1, 0));
        lineRenderer.SetPosition(lineRenderer.positionCount - 3, new Vector3(x2, y2, 0));
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, new Vector3(left, y2, 0));
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(left, y1, 0));

        if (newRoadPosition <= y2)
        {
            newRoad = false;
            DrawRoads();
        }
    }

    // Functions
    public float GetRoadPosition(int road)
    {
        return road * roadWidth - (roadAmount / 2 * roadWidth);
    }

    public void AddRoad()
    {
        roadAmount++;
        lineRenderer.positionCount += 6;
        newRoad = true;
        newRoadPosition = GameManager.Instance.GetWorldFromScreenPosition(0, 1.5f).y;
        SetCamera();
    }

    public void RemoveRoad() 
    { 
        roadAmount--;
        lineRenderer.positionCount -= 4;
        SetCamera();
        DrawRoads();
    }

    public void DrawRoads()
    {
        lineRenderer.positionCount = 0;
        float left = GameManager.Instance.GetWorldFromScreenPosition(-0.5f, 0).x;
        float y1 = GameManager.Instance.GetWorldFromScreenPosition(0, 1.5f).y;
        float y2 = GameManager.Instance.GetWorldFromScreenPosition(0, -0.5f).y;
        for (int i = 0; i < roadAmount; i++)
        {
            float x = GetRoadPosition(i);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(x, y1, 0));
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(x, y2, 0));
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(left, y2, 0));
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(left, y1, 0));
        }
    }

    public void SetCamera()
    {
        Camera cam = Camera.main;
        cam.orthographicSize = Mathf.Max(5, roadAmount * roadWidth + 1);

        float l = GetRoadPosition(Mathf.FloorToInt(roadAmount / 2));
        float r = GetRoadPosition(Mathf.CeilToInt(roadAmount / 2));

        cam.transform.position = new Vector3(0, (l + r) / 2, -10f);
    }
}
