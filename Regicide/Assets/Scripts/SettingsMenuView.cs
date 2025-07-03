using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : View
{
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _creditsButton;

    public override void Initialize()
    {
        _mainVolumeSlider.onValueChanged.AddListener(SetMainVolume);
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        _backButton.onClick.AddListener(() => ViewManager.Show<MainMenuView>());
        _creditsButton.onClick.AddListener(() => ViewManager.Show<CreditsMenuView>());
    }

    private void SetMainVolume(float value)
    {
        AudioListener.volume = value;
    }

    private void SetBGMVolume(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
