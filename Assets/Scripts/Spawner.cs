using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]private GameObject[] enemies;
    [SerializeField]private float time;
    [SerializeField]private Vector2 cooldownRange;
    [SerializeField]private Vector2Int spawnRange;

    void Start()
    {
        enemies = Resources.LoadAll<GameObject>("Prefabs/Enemies");
    }

    void LateUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        time -= Time.deltaTime;

        if (time > 0)
            return;

        time = Random.Range(cooldownRange.x, cooldownRange.y);
        int spawnCount = Random.Range(spawnRange.x, spawnRange.y);
        spawnCount = Mathf.Clamp(spawnCount, 1, RoadManager.Instance.roadAmount - 1);
        bool[] openRoads = new bool[RoadManager.Instance.roadAmount];

        for (int i = spawnCount; i > 0; i--)
        {
            Transform enemy = Instantiate(enemies[Random.Range(0, enemies.Length)]).transform;
            enemy.position = GameManager.Instance.GetWorldFromScreenPosition(0, 2f);
            int road = Random.Range(0, RoadManager.Instance.roadAmount);
            while (openRoads[road])
                road = Random.Range(0, RoadManager.Instance.roadAmount);

            enemy.GetComponent<Obstacle>().currentRoad = road;
            openRoads[road] = true;
        }
    }
}
