using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreEntryText : MonoBehaviour
{
    public int rank;
    TMPro.TextMeshProUGUI scoreTMP;
    string scoreText;
    HighScoreEntry highScoreEntry;

    private void Start()
    {
        scoreTMP = GetComponent<TextMeshProUGUI>();
        highScoreEntry = HighScoreManager.Instance.currentHighScores[rank - 1];

        BuildScoreText();
        scoreTMP.text = scoreText;
    }

    void BuildScoreText()
    {
        scoreText = rank + ". " + highScoreEntry.Score + "  ";
        for (int i = highScoreEntry.Name.Length; i < HighScoreEntry.scoreDigitLimit; i++)
        {
            scoreText += " ";
        }
        for (int i = highScoreEntry.Name.Length; i < HighScoreEntry.nameLengthLimit; i++)
        {
            scoreText += " ";
        }
        scoreText += highScoreEntry.Name;

        
    }
}
