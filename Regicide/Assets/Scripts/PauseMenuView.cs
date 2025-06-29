using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuView : View
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;

    public override void Initialize()
    {
        _mainMenuButton.onClick.AddListener(BackToMainMenu);
        _resumeButton.onClick.AddListener(BackToGame);
    }

    private void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    private void BackToGame()
    {
        Time.timeScale = 1f;
        ViewManager.ShowLast();
    }
}
