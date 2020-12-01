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
    private float spawnRate = 3.0f;
    [SerializeField]
    private float minSpawnRate;
    [SerializeField]
    private float maxSpawnRate;
    private readonly int[] spawnDirections = new int[2] { -1, 1 };
    private IEnumerator spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //Find the player Object on the Hierarchy and get its controller script
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerPoints = 0;
        spawnRate = maxSpawnRate;
        spawnCoroutine = SpawnTarget();
        StartCoroutine(spawnCoroutine);
        StartCoroutine(TimerCountDown(maxTime));
    }

    IEnumerator SpawnTarget()
    {
        while (true)
        {
            int direction = spawnDirections[Random.Range(0, spawnDirections.Length)];
            float spawnPointX = spawnposX * direction;
            float spawnPointY = Random.Range(spawnRangeYMin, spawnRangeYMax);
            Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, 0);

            yield return new WaitForSeconds(spawnRate);
            Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation);
            spawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    IEnumerator TimerCountDown(int timeLeft)
    {
        while (timeLeft > 0)
        {
            UpdateTimer(timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
        // Just to show the zero
        UpdateTimer(timeLeft);

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

    private void UpdateTimer(int timeLeft)
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
