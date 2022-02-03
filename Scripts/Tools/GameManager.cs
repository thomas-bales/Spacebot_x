using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    public static GameManager Instance;

    [SerializeField] int enemySpawnPerMinute;
    public static float EnemySpawnRate
    {
        get
        {
            return 60f / Instance.enemySpawnPerMinute;
        }
    }

    [SerializeField] int maxEnemySpawnPerMinute;
    [SerializeField] int EnemySpawnPerMinuteIncreaseRate;
    [SerializeField] int EnemySpawnPerMinuteIncreaseAmount;
    [Space]
    [Space]

    [SerializeField] int eggSpawnPerMinute;
    public static float EggSpawnRate
    {
        get
        {
            return 60f / Instance.eggSpawnPerMinute;
        }
    }

    [SerializeField] int maxEggSpawnPerMinute;
    [SerializeField] int EggSpawnPerMinuteIncreaseRate;
    [SerializeField] int EggSpawnPerMinuteIncreaseAmount;
    [Space]
    [Space]

    [Header("For debug only!")]
    [SerializeField] bool isGamePaused;
    public static bool IsGamePaused
    {
        get
        {
            return Instance.isGamePaused;
        }
    }

    [SerializeField] bool isGameStarted;
    public static bool IsGameStarted
    {
        get
        {
            return Instance.isGameStarted;
        }
    }

    [SerializeField] bool isGameOver;
    public static bool IsGameOver
    {
        get
        {
            return Instance.isGameOver;
        }
    }

    [HideInInspector] public static bool IsGameEnded;

    [SerializeField] float gameTime;
    public static float GameTime 
    { 
        get 
        { 
            return Instance.gameTime; 
        } 
    }
    #endregion

    public Animator animator;

    float nextEggSpawnRateIncreaseTime;
    float nextEnemySpawnRateIncreaseTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        nextEggSpawnRateIncreaseTime = EggSpawnPerMinuteIncreaseRate;
        nextEnemySpawnRateIncreaseTime = EnemySpawnPerMinuteIncreaseRate;
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += HandleOnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= HandleOnGameOver;
    }

    private void Update()
    {
        if (IsGameStarted && !IsGameOver)
        {
            UpdateGameTime();
        }

        if (GameTime >= nextEggSpawnRateIncreaseTime)
        {
            IncreaseEggSpawnRate();
            nextEggSpawnRateIncreaseTime += EnemySpawnPerMinuteIncreaseRate;
        }

        if (GameTime >=nextEnemySpawnRateIncreaseTime)
        {
            IncreaseEnemySpawnRate();
            nextEnemySpawnRateIncreaseTime += EnemySpawnPerMinuteIncreaseRate;
        }
    }

    void IncreaseEggSpawnRate()
    {
        eggSpawnPerMinute += EggSpawnPerMinuteIncreaseAmount;
        eggSpawnPerMinute = (int)Mathf.Clamp(eggSpawnPerMinute, 0f, maxEggSpawnPerMinute);
    }

    void IncreaseEnemySpawnRate()
    {
        enemySpawnPerMinute += EnemySpawnPerMinuteIncreaseAmount;
        enemySpawnPerMinute = (int)Mathf.Clamp(enemySpawnPerMinute, 0f, maxEnemySpawnPerMinute);
    }

    void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
    }

    void ResetGameTime()
    {
        gameTime = 0f;
    }

    void HandleOnGameOver()
    {
        isGameOver = true;
        isGameStarted = false;

        PauseGame();

        if (HighScoreManager.Instance.CheckIfHighScore((int)gameTime))
        {
            EventManager.RaiseOnPlayerGotHighScore();
            animator.SetBool("isHighScore", true);
        }
        else
        {
            animator.SetBool("isHighScore", false);
            StartCoroutine(co_HandleOnGameOver());
        }

        animator.SetTrigger("isGameOver");
    }

    IEnumerator co_HandleOnGameOver()
    {
        yield return new WaitForSecondsRealtime(3.5f);
        EndGame();
    }

    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    void UnpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        IsGameEnded = false;
        ResetGameTime();
        UnpauseGame();
        LevelManager.Instance.LoadScene(1);
        isGameStarted = true;
        AudioManager.instance.ToggleMusic();
    }

    public void OpenHighScore()
    {
        LevelManager.Instance.LoadScene(2);
        EventManager.RaiseOnOpenHighScores();
    }

    public void GoToTitleScreen()
    {
        LevelManager.Instance.LoadScene(0);
    }

    public void EndGame()
    {
        IsGameEnded = true;
        HighScoreManager.Instance.SaveHighScores();
        OpenHighScore();
        Debug.Log("Toggled audio from endgame");
        AudioManager.instance.ToggleMusic();
    }
}
