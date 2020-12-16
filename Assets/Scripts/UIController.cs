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
    [SerializeField]
    private int maxTime = 30;
    public AudioClip gameOverSound;

    private int totalScore;

    public static event Action TimesUpEvent;
    public static event Action StartGameEvent;
    public static event Action RestartGameEvent;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGameLoop);
        exitButton.onClick.AddListener(ExitGame);

        PlayerController.EnemyCaughtEvent += UpdateScore;

        //restartButton = gameOverScreen.GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(OnClickRestartButton);
    }

    // Called by the Start Button Event
    public void StartGameLoop()
    {
        titleScreen.SetActive(false);
        StartGameEvent?.Invoke();
        StartCoroutine(TimerCountDown());
    }
    private void OnClickRestartButton()
    {
        gameOverScreen.SetActive(false);
        UpdateScore(0);
        UpdateTimer(maxTime);
        RestartGameEvent?.Invoke();
        StartCoroutine(TimerCountDown());
    }

    // TODO: Consider moving Coroutine to GameManager
    IEnumerator TimerCountDown()
    {
        int timeLeft = maxTime;

        while (timeLeft > 0)
        {
            UpdateTimer(timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
        // Just to show the zero
        UpdateTimer(timeLeft);

        //Trigger the timer is over event
        TimesUpEvent?.Invoke();

        DisplayGameOverMenu();
    }

    private void DisplayGameOverMenu()
    {
        GameManager.Instance.AudioSource.PlayOneShot(gameOverSound);
        totalScoreText.text += " " + totalScore.ToString();
        gameOverScreen.SetActive(true);
    }

    private void UpdateTimer(int timeLeft)
    {
        timerText.text = "TIMER : " + timeLeft;
    }

    private void UpdateScore(int score)
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
