using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TMPro.TextMeshProUGUI scoreTMP;
    [SerializeField] TMPro.TextMeshProUGUI gameOverScoreTMP;
    string scoreText;
    int numDigits = 3;

    private void OnEnable()
    {
        EventManager.OnGameOver += HandleOnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= HandleOnGameOver;
    }

    void Start()
    {
        scoreTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameManager.IsGameStarted)
        {
            BuildScoreText();
            DisplayScore();
        }
    }

    void BuildScoreText()
    {
        scoreText = ((int)GameManager.GameTime).ToString();
        for (int i = scoreText.Length; i < numDigits; i++)
        {
            scoreText = 0 + scoreText;
        }
    }

    void DisplayScore()
    {
        scoreTMP.text = scoreText;
    }

    void HandleOnGameOver()
    {
        gameOverScoreTMP.text = "Score: " + scoreText;
    }
}
