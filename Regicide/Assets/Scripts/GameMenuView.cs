using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : View
{
    [SerializeField] private Button _pauseButton;

    public override void Initialize()
    {
        _pauseButton.onClick.AddListener(OpenPauseMenu);
    }

    private void OpenPauseMenu()
    {
        AudioManager.Instance.PlayButtonClick(); 
        ViewManager.Show<PauseMenuView>();
        Time.timeScale = 0f; // Pause game
    }
}
