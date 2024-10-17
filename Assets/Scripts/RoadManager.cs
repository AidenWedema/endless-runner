using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public int roadAmount = 3;
    public float roadWidth = 1.1f;
    
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
    }

    // Functions
    public float GetRoadPosition(int road)
    {
        return road * roadWidth - (roadAmount / 2 * roadWidth);
    }
}
