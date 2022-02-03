using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonMethods : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] AudioClipSO buttonClickSound;

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void OpenHighScore()
    {
        GameManager.Instance.OpenHighScore();
    }

    public void GoToTitleScreen()
    {
        GameManager.Instance.GoToTitleScreen();
    }

    public void InputName()
    {
        string playerName = inputField.text;
        EventManager.RaiseOnAddNewHighScore(playerName);
        GameManager.Instance.EndGame();
    }

    void PlaySound()
    {
        //AudioManager.instance.PlayAudioClip(buttonClickSound);
    }
}
