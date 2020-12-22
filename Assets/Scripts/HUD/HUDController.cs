using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;

    private void LateUpdate()
    {
        UpdateTimerTextElement(GameManager.Instance.TimeLeft);
        UpdateTextScoreElement(GameManager.Instance.PlayerTotalPoints);
    }
    private void UpdateTimerTextElement(int timeLeft)
    {
        timerText.text = "TIMER : " + timeLeft;
    }

    private void UpdateTextScoreElement(int score)
    {
        scoreText.text = score.ToString();
    }

    private void OnEnable()
    {
        UpdateTextScoreElement(0);
    }
}
