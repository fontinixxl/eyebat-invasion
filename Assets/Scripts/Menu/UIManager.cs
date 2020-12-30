using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    #region Field Declarations
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private GameOverMenu _gameOverMenu;
    [SerializeField] private HUDController _hudController;

    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameState currentState, GameState previousState)
    {
        // ENTERING "RUNNING" FROM "PREGAME" STATE
        if (previousState == GameState.PREGAME &&
            currentState == GameState.RUNNING)
        {
            SetGameObjectActive(_hudController.gameObject, true);
            SetGameObjectActive(_mainMenu.gameObject, false);
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
}
