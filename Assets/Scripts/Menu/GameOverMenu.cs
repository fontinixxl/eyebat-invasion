using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button PlayAgainButton;
    public Button ExitButton;

    public Text totalScoreText;

    private void Start()
    {
        PlayAgainButton.onClick.AddListener(HandleRestartClick);
        ExitButton.onClick.AddListener(HandleExitClick);
    }

    private void HandleRestartClick()
    {
        GameManager.Instance.RestartGame();
    }
    private void HandleExitClick()
    {
        GameManager.Instance.ExitGame();
    }

    public void UpdateTotalScoreText(string text)
    {
        totalScoreText.text = "SCORE: " + text;
    }

    private void OnEnable()
    {
        UpdateTotalScoreText(GameManager.Instance.PlayerTotalPoints.ToString());
    }
}
