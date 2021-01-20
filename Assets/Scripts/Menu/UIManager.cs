using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Field Declarations
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private GameOverMenu _gameOverMenu;
    [SerializeField] private HUDController _hudController;
    [SerializeField] private GameObject _instructions;
    [SerializeField] private GameObject _credits;

    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        SetGameObjectActive(_mainMenu.gameObject, true);
        SetGameObjectActive(_hudController.gameObject, false);
        SetGameObjectActive(_gameOverMenu.gameObject, false);
        SetGameObjectActive(_instructions, false);
        SetGameObjectActive(_credits, false);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.PRERUNNING)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartGame();
        }
    }

    private void HandleGameStateChanged(GameState currentState, GameState previousState)
    {

        // ENTERING "PRERUNNING" FROM "PREGAME" STATE
        if (previousState == GameState.PREGAME &&
            currentState == GameState.PRERUNNING)
        {
            SetGameObjectActive(_mainMenu.gameObject, false);
            _instructions.SetActive(true);
        }

        // ENTERING "RUNNING" FROM "PRERUNNING" STATE
        if (previousState == GameState.PRERUNNING &&
            currentState == GameState.RUNNING)
        {
            _instructions.SetActive(false);
            SetGameObjectActive(_hudController.gameObject, true);
        }

        // ENTERING "GAME OVER" FROM ANY STATE
        if (currentState == GameState.GAMEOVER)
        {
            SetGameObjectActive(_gameOverMenu.gameObject, true);
            SetGameObjectActive(_hudController.gameObject, false);
        }
        // LEAVING "GAME OVER" FROM ANY STATE
        if (previousState == GameState.GAMEOVER)
        {
            SetGameObjectActive(_gameOverMenu.gameObject, false);
            SetGameObjectActive(_mainMenu.gameObject, true);
        }
    }

    public void SetGameObjectActive(GameObject gameObject, bool active)
    {
        gameObject.SetActive(active);
    }

    public void HandleCreditsButtonClicked()
    {
        SetGameObjectActive(_mainMenu.gameObject, false);
        SetGameObjectActive(_credits, true);
    }

    public void HandleMainButtonClicked()
    {
        SetGameObjectActive(_credits, false);
        SetGameObjectActive(_mainMenu.gameObject, true);
    }
}
