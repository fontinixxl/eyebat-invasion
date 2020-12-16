using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    #region Field Declarations

    [Header("UI Components")]
    [Space]
    public Text scoreText;
    public Text totalScoreText;
    public GameObject gameOverScreen;
    public GameObject titleScreen;
    public Button exitButton;
    public Button startButton;
    public Button restartButton;

    [Header("Timer Components")]
    [Space]
    public Text timerText;

    private int totalScore;

    public static event Action StartGameEvent;
    public static event Action RestartGameEvent;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGameLoop);
        exitButton.onClick.AddListener(ExitGame);

        PlayerController.EnemyCaughtEvent += UpdateTextScoreElement;
        GameManager.GameOverEvent += DisplayGameOverMenu;

        //restartButton = gameOverScreen.GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(OnClickRestartButton);
    }

    // Called by the Start Button Event
    public void StartGameLoop()
    {
        StartGameEvent?.Invoke();
        titleScreen.SetActive(false);
    }
    private void OnClickRestartButton()
    {
        gameOverScreen.SetActive(false);
        UpdateTextScoreElement(0);
        RestartGameEvent?.Invoke();
    }

    private void DisplayGameOverMenu()
    {
        totalScoreText.text += " " + totalScore.ToString();
        gameOverScreen.SetActive(true);
    }

    public void UpdateTimerTextElement(int timeLeft)
    {
        timerText.text = "TIMER : " + timeLeft;
    }

    private void UpdateTextScoreElement(int score)
    {
        totalScore = score;
        scoreText.text = score.ToString();
    }

    private void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
