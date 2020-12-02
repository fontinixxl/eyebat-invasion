using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject gameOverText;
    public Text totalPointsText;
    public Text CurrentPointsText;
    public Text timerText;
    public Button restartButton;

    private PlayerController playerController;

    public float spawnRangeYMin = 8.0f;
    public float spawnRangeYMax = 14.0f;
    public float spawnposX = 14.0f;
    public int maxTime = 30;
    private int playerPoints;

    [SerializeField]
    private float minSpawnRate;
    [SerializeField]
    private float maxSpawnRate;
    private readonly int[] spawnDirections = new int[2] { -1, 1 };
    private IEnumerator spawnCoroutine;
    private bool hasTimerCountdownStarted;

    // Start is called before the first frame update
    void Start()
    {
        //Find the player Object on the Hierarchy and get its controller script
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerPoints = 0;
        hasTimerCountdownStarted = false;
        spawnCoroutine = SpawnTarget();
        StartCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnTarget()
    {
        while (true)
        {
            int direction = spawnDirections[Random.Range(0, spawnDirections.Length)];
            float spawnPointX = spawnposX * direction;
            float spawnPointY = Random.Range(spawnRangeYMin, spawnRangeYMax);
            Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, 0);

            Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation);
            float spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
            
            // TODO: figure out how to call the timerCountdown outside the SpawnTarget
            // So it's not checking every iteration if the coroutine has started.
            
            // Start Timer Countdown once the first target has been spawned
            if (!hasTimerCountdownStarted)
            {
                StartCoroutine(TimerCountDown());
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator TimerCountDown()
    {
        // Set to true so the coroutine will be started once
        hasTimerCountdownStarted = true;

        int timeLeft = maxTime;
        while (timeLeft > 0)
        {
            DisplayTimer(timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
        // Just to show the zero
        DisplayTimer(timeLeft);

        // Call Game Over when the timer is over!
        GameOver();
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        StopCoroutine(spawnCoroutine);
        RemoveRemainingTargets();
        // Disable playerController script so player can't move.
        playerController.enabled = false;

        //Show Game Over menu
        DisplayGameOverMenu();
    }

    private void DisplayGameOverMenu()
    {
        gameOverText.gameObject.SetActive(true);
        DisplayTotalPoints();
        restartButton.gameObject.SetActive(true);
    }

    private void DisplayTotalPoints()
    {
        totalPointsText.text = playerPoints + " Points";
        totalPointsText.gameObject.SetActive(true);
    }

    private void RemoveRemainingTargets()
    {
        GameObject[] enemiesOnScene;
        enemiesOnScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemiesOnScene)
        {
            Destroy(enemy.gameObject);
        }

    }

    private void DisplayTimer(int timeLeft)
    {
        timerText.text = "Timer: " + timeLeft;
    }

    public void AddPointToPlayer()
    {
        playerPoints++;
        UpdatePointsCounter();
    }

    private void UpdatePointsCounter()
    {
        CurrentPointsText.text = "Points : " + playerPoints;
    }

     // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
