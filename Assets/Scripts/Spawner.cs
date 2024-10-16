using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float time;
    private Vector2 cooldownRange;
    private Vector2 spawnRange;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Paused)
            return;

        time -= Time.deltaTime;

        if (time > 0)
            return;

        time = Random.Range(cooldownRange.x, cooldownRange.y);

        Resources.LoadAll<GameObject>("Prefabs/Enemies");
    }
}
