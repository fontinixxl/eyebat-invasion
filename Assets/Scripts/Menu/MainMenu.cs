using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _startButton.onClick.AddListener(GameManager.Instance.HandleStartButtonClicked);
        _exitButton.onClick.AddListener(GameManager.Instance.ExitGame);
    }
}