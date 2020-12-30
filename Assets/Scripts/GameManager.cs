using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState { PREGAME, RUNNING, GAMEOVER, PAUSED }

public class GameManager : Singleton<GameManager>
{
    #region Declarations

    public GameObject targetPrefab;
    public GameObject despawnSensor;

    private AudioSource audioSource;
    public AudioSource AudioSource { get { return audioSource; } }
    public AudioClip gameOverSound;

    private float spawnRangeYMin;
    private float spawnRangeYMax;
    // Offset distance off-screen on the X coordinate where the enemy will be spawning
    private readonly float offScreenXOffset = 1;
    private readonly float spawnYOffset = 2.5f;
    [SerializeField] private float minSpawnRate = 1;
    [SerializeField] private float maxSpawnRate = 3;
    private readonly int[] spawnDirections = new int[2] { -1, 1 };
    
    [SerializeField] private int maxTime = 30;
    // TOOD: Add description to the Editor
    [SerializeField] private int _timeIncrement = 3;
    private int _timeLeft;
    public int TimeLeft { get { return _timeLeft; } }
    private string _currentLevelName = String.Empty;

    private GameState _currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }
    [HideInInspector] public EventGameState OnGameStateChanged;

    [SerializeField] private int _playerTotalPoints;
    [HideInInspector] public int PlayerTotalPoints { get { return _playerTotalPoints; } }

    [Header("DEBUG--> Toggle Features")]
    public bool TimerBonusFeature;
    public bool GameOverOnEyeEscapeFeature;


    #endregion

    #region Initializations

    protected override void Awake()
    {
        base.Awake();
        _timeLeft = maxTime;

    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        PlayerController.ScorePointsEvent += HandlePlayerScorePointsEvent;

        // TODO: Fix target broken pivot that force me to make weird math to figure out corret spawnRange on Y
        spawnRangeYMin = (ScreenBounds.Height / 2);
        spawnRangeYMax = ScreenBounds.Height - spawnYOffset;

        SpawnLeftRightSensor();

        OnGameStateChanged.Invoke(GameState.PREGAME, _currentGameState);
    }

    private void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;

        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        ao.completed += OnUnloadOperationComplete;
    }

    #endregion

    #region Game Loop
    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (CurrentGameState)
        {
            case GameState.PREGAME:
                LoadLevel("Main");
                _playerTotalPoints = 0;
                _timeLeft = maxTime;
                break;
            case GameState.RUNNING:
                audioSource.Play();
                if (previousGameState == GameState.PREGAME)
                {
                    StartCoroutine("SpawnTarget");
                    StartCoroutine("TimerCountDown");
                }
                break;
            case GameState.PAUSED:
                break;
            case GameState.GAMEOVER:
                StopAllCoroutines();
                UnloadLevel(_currentLevelName);
                audioSource.Stop();
                audioSource.PlayOneShot(gameOverSound);
                RemoveRemainingTargets();
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(_currentGameState, previousGameState);

    }

    public void GameOver()
    {
        UpdateState(GameState.GAMEOVER);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    #endregion

    #region Coroutines and Spawning Methods

    private void SpawnLeftRightSensor()
    {
        float spawnRangeYPos = spawnRangeYMin + ((spawnRangeYMax - spawnRangeYMin) / 2);
        Vector3 spawnSensorPosition = new Vector3(ScreenBounds.Width + offScreenXOffset * 2, spawnRangeYPos,
            despawnSensor.transform.position.z);
        // Spawn right sensor
        Instantiate(despawnSensor, spawnSensorPosition, Quaternion.identity);
        // Spawn left sensor
        spawnSensorPosition.x *= -1;
        Instantiate(despawnSensor, spawnSensorPosition, Quaternion.identity);
    }

    // TOOD: Maybe move to MainScene So it won't be necessary to remove remaining Enemies as they will be destroy Unloading the level
    IEnumerator SpawnTarget()
    {
        while (_currentGameState == GameState.RUNNING)
        {
            int direction = spawnDirections[UnityEngine.Random.Range(0, spawnDirections.Length)];
            float spawnXComponent = (ScreenBounds.Width + offScreenXOffset) * direction;
            float spawnYComponent = UnityEngine.Random.Range(spawnRangeYMin, spawnRangeYMax);

            Vector3 spawnPosition = new Vector3(spawnXComponent, spawnYComponent, 0);

            Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation);
            float spawnRate = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator TimerCountDown()
    {
        // Wait one second before starting the timer, so the first target will
        // be on the screeen when the timer starts.
        yield return new WaitForSeconds(1);

        while (_timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            _timeLeft--;
        }

        UpdateState(GameState.GAMEOVER);
    }

    #endregion

    #region Event Handlers

    public void HandleStartButtonClicked()
    {
        LoadLevel("Main");
    }

    private void HandlePlayerScorePointsEvent(int pointsToScore)
    {
        _playerTotalPoints += pointsToScore;

        if (TimerBonusFeature == true)
        {
            _timeLeft += _timeIncrement;
        }
    }

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Level " + _currentLevelName + " Loaded");

        UpdateState(GameState.RUNNING);
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        // Clean up level is necessary, go back to main menu
    }

    #endregion

    #region Helpers
    private void RemoveRemainingTargets()
    {
        GameObject[] enemiesOnScene;
        enemiesOnScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesOnScene)
        {
            Destroy(enemy.gameObject);
        }
    }

    #endregion

    [System.Serializable] public class EventGameState : UnityEvent<GameState, GameState> { }
}
