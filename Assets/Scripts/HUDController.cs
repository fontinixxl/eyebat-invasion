using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDController : MonoBehaviour
{
    #region Field Declarations

    [Header("UI Components")]
    [Space]
    //public Text totalScoreText;
    public Text scoreText;
    public Button restartButton;
    public GameObject gameOverText;


    [Header("Timer Components")]
    [Space]
    public Text timerText;
    [SerializeField]
    private int maxTime = 30;

    public static event Action TimesUpEvent;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.EnemyCaughtEvent += UpdateScore;

        StartCoroutine(TimerCountDown());
    }

    IEnumerator TimerCountDown()
    {
        int timeLeft = maxTime;

        yield return new WaitForSeconds(1);
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
        gameOverText.gameObject.SetActive(true);
        //DisplayTotalScore();
        //restartButton.gameObject.SetActive(true);
    }
    /*
    private void DisplayTotalScore(int score)
    {
        totalScoreText.text = score + " Points";
        totalScoreText.gameObject.SetActive(true);
    }
    */

    private void UpdateTimer(int timeLeft)
    {
        timerText.text = "TIMER : " + timeLeft;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
