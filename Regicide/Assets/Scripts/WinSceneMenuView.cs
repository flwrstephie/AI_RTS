using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneMenuView : View
{
    [SerializeField] private Button _backToMainButton;

    public override void Initialize()
    {
        _backToMainButton.onClick.AddListener(OnBackToMainMenuClicked);
    }

    private void OnBackToMainMenuClicked()
    {
        // AudioManager.Instance.PlayButtonClick(); // SFX
        SceneManager.LoadScene("MainMenuScene");
    }
}
