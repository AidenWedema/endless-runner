using TMPro;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    public TMP_Text scoreText;

    // Singleton
    public static GuiManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        scoreText = Instantiate(Resources.Load<GameObject>("Prefabs/Text"), GameObject.Find("Canvas").transform).GetComponent<TMP_Text>();
        Vector2 position = GameManager.Instance.GetWorldFromScreenPosition(0.1f, 0.9f);
        scoreText.rectTransform.sizeDelta = GameManager.Instance.GetScreenResolution() / 2;
        scoreText.rectTransform.pivot = new Vector2(0f, 1f);
        scoreText.rectTransform.position = position;
        scoreText.fontSize = 64;
        scoreText.fontStyle = FontStyles.Bold;
    }

    private void LateUpdate()
    {
        scoreText.text = $"Score: {GameManager.Instance.score}";
    }
}
