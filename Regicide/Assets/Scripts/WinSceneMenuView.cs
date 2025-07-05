using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneMenuView : View
{
    [SerializeField] private Button backToMainButton;

    private void Start()
    {
        if (backToMainButton != null)
            backToMainButton.onClick.AddListener(OnBackToMainMenu);
    }

    private void OnBackToMainMenu()
    {
        AudioManager.Instance.PlayButtonClick(); 
        SceneManager.LoadScene("MainMenu");
    }
}
