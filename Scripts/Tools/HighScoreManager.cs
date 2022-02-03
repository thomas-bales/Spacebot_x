using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    [HideInInspector] public HighScoreEntry[] currentHighScores = new HighScoreEntry[10];

    int newHighScore = 0;
    int newHighScoreRank = 0;

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
    }

    private void OnEnable()
    {
        EventManager.OnAddNewHighScore += HandleOnAddNewHighScore;
    }

    private void OnDisable()
    {
        EventManager.OnAddNewHighScore -= HandleOnAddNewHighScore;
    }

    private void Start()
    {
        LoadHighScores();
    }

    void LoadHighScores()
    {
        for (int i = 1; i <= currentHighScores.Length; i++)
        {
            currentHighScores[i - 1] = new HighScoreEntry();
            currentHighScores[i - 1].Rank = i;
            currentHighScores[i - 1].Score = PlayerPrefs.GetInt("score" + i);
            currentHighScores[i - 1].SetName(PlayerPrefs.GetString("name" + i));
        }
    }

    public bool CheckIfHighScore(int score)
    {
        for (int i = 1; i <= currentHighScores.Length; i++)
        {
            if (score >= currentHighScores[i - 1].Score)
            {
                newHighScore = score;
                newHighScoreRank = i;
                return true;
            }
        }

        return false;
    }

    void HandleOnAddNewHighScore(string _name)
    {
        AddLastCheckedScoreAsHighScore(_name);
    }

    void AddLastCheckedScoreAsHighScore(string _name)
    {
        string newHighScoreName = _name;
        string temp_name;
        int temp_score;

        for (int i = newHighScoreRank; i <= 10; i++)
        {
            temp_name = currentHighScores[i - 1].Name;
            currentHighScores[i - 1].SetName(newHighScoreName);

            temp_score = currentHighScores[i - 1].Score;
            currentHighScores[i - 1].Score = newHighScore;

            newHighScoreName = temp_name;
            newHighScore = temp_score;
        }
    }

    public void SaveHighScores()
    {
        for (int i = 1; i <= currentHighScores.Length; i++)
        {
            PlayerPrefs.SetInt("score" + i, currentHighScores[i - 1].Score);
            PlayerPrefs.SetString("name" + i, currentHighScores[i - 1].Name);
        }
    }

    public void PrintHighScores()
    {
        for (int i = 1; i <= currentHighScores.Length; i++)
        {
            Debug.Log("Rank: " + i + "; Name: " + currentHighScores[i - 1].Name + "; Score: " + currentHighScores[i - 1].Score);
        }
    }

    public void ResetHighScores()
    {
        for (int i = 1; i <= currentHighScores.Length; i++)
        {
            PlayerPrefs.DeleteKey("score" + i);
            PlayerPrefs.DeleteKey("name" + i);
        }
    }
}
