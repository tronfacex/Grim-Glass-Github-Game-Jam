using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenuController : MonoBehaviour
{
    [field: Header("Main Menu Panel")]
    [field: SerializeField] public RectTransform MainMenuPanel { get; private set; }
    [field: SerializeField] public Button NewGameButton { get; private set; }
    [field: SerializeField] public Button TutorialButton { get; private set; }
    [field: SerializeField] public Button SettingsButton { get; private set; }
    [field: SerializeField] public GameObject MainMenuFirstSelection { get; private set; }

    [field: Header("New Game Panel")]
    [field: SerializeField] public RectTransform NewGamePanel { get; private set; }
    [field: SerializeField] public Slider GameDifficultySlider { get; private set; }
    [field: SerializeField] public Button StartGameButton { get; private set; }
    [field: SerializeField] public Button BackToMainButtonNG { get; private set; }
    [field: SerializeField] public GameObject NewGameFirstSelection { get; private set; }
    [field: Header("Tutorial Panel")]
    [field: SerializeField] public RectTransform TutorialPanel { get; private set; }
    [field: SerializeField] public Button BackToMainButtonTut { get; private set; }
    [field: SerializeField] public GameObject TutorialFirstSelection { get; private set; }
    [field: Header("Settings Panel")]
    [field: SerializeField] public RectTransform SettingsPanel { get; private set; }
    [field: SerializeField] public Button BackToMainButtonSettings { get; private set; }
    [field: SerializeField] public Slider XAxisSensitivitySlider { get; private set; }
    [field: SerializeField] public Slider YAxisSensitivitySlider { get; private set; }
    [field: SerializeField] public Toggle YInvertToggle { get; private set; }
    [field: SerializeField] public GameObject SettingsFirstSelection { get; private set; }

    [field: SerializeField] public RectTransform CenterScreenPos { get; private set; }

    [field: SerializeField] public RectTransform OffScreenRightPos { get; private set; }

    [field: SerializeField] public List<RectTransform> PanelsList { get; private set; }

    private void Start()
    {
        SettingsReader.Instance.GameSettings.XAxisLookSensitivity = 300;
        XAxisSensitivitySlider.value = SettingsReader.Instance.GameSettings.XAxisLookSensitivity;
        SettingsReader.Instance.GameSettings.YAxisLookSensitivity = 2;
        YAxisSensitivitySlider.value = SettingsReader.Instance.GameSettings.YAxisLookSensitivity;
        SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth = 5;
        GameDifficultySlider.value = SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth;

        ShowPanel(MainMenuPanel, CenterScreenPos.anchoredPosition, 0f);
    }

    public void ShowPanel(RectTransform Panel, Vector3 tweenPosition, float duration)
    {
        foreach (RectTransform panel in PanelsList)
        {
            HidePanel(panel, OffScreenRightPos.anchoredPosition, .33f);
        }
        Panel.gameObject.SetActive(true);
        Panel.DOAnchorPos(tweenPosition, duration);
        if (Panel.name == "Main Menu")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(MainMenuFirstSelection);
        }
        if (Panel.name == "New Game Menu")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(NewGameFirstSelection);
        }
        if (Panel.name == "Tutorial")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(TutorialFirstSelection);
        }
        if (Panel.name == "Settings Menu")
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(SettingsFirstSelection);
        }
    }

    public void HidePanel(RectTransform Panel, Vector3 tweenPosition, float duration)
    {
        Panel.DOAnchorPos(tweenPosition, duration);
        Panel.gameObject.SetActive(false);
    }

    public void OnNewGameButton()
    {
        ShowPanel(NewGamePanel, CenterScreenPos.anchoredPosition, .5f);
    }

    public void OnDifficultySliderChanged()
    {
        SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth = (int)GameDifficultySlider.value;
    }

    public void OnXAxisSliderChanged()
    {
        SettingsReader.Instance.GameSettings.XAxisLookSensitivity = XAxisSensitivitySlider.value;
    }
    public void OnYAxisSliderChanged()
    {
        SettingsReader.Instance.GameSettings.YAxisLookSensitivity = YAxisSensitivitySlider.value;
    }
    public void OnInvertYAxisToggle()
    {
        SettingsReader.Instance.GameSettings.InvertYAxis = !YInvertToggle.isOn;
    }

    public void OnBackToMainMenu()
    {
        ShowPanel(MainMenuPanel, CenterScreenPos.anchoredPosition, 0f);
    }

    public void OnStartGameButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnTutorialButton()
    {
        ShowPanel(TutorialPanel, CenterScreenPos.anchoredPosition, .5f);
    }
    public void OnSettingsButton()
    {
        ShowPanel(SettingsPanel, CenterScreenPos.anchoredPosition, .5f);
    }
}
