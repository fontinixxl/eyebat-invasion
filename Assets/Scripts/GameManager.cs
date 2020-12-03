using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;

    private PlayerController playerController;

    [Range(7.0f, 8.0f)]
    public float spawnRangeYMin = 7.5f;
    [Range(10.0f, 12.0f)]
    public float spawnRangeYMax = 12.0f;
    private float spawnposX = 14.0f;

    [SerializeField]
    private float minSpawnRate = 1;
    [SerializeField]
    private float maxSpawnRate = 3;
    private readonly int[] spawnDirections = new int[2] { -1, 1 };
    private IEnumerator spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Subscrive to the TimesUpEvent so we can Stop the Game from the Manager.
        HUDController.TimesUpEvent += GameOver;

        //Find the player Object on the Hierarchy and get its controller script
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

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

            // TODO: Start Timer Countdown once the first target has been spawned

            yield return new WaitForSeconds(spawnRate);
        }
    }

    // Stop game logic
    public void GameOver()
    {
        StopCoroutine(spawnCoroutine);
        RemoveRemainingTargets();
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

     // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
