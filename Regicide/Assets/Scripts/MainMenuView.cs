using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuView : View
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _settingsButton;

    public override void Initialize()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnPlayClicked()
    {
        AudioManager.Instance.PlayButtonClick(); // SFX
        SceneManager.LoadScene("GameScene");
    }

    private void OnExitClicked()
    {
        AudioManager.Instance.PlayButtonClick(); // SFX
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnSettingsClicked()
    {
        AudioManager.Instance.PlayButtonClick(); // SFX
        ViewManager.Show<SettingsMenuView>();
    }
}
