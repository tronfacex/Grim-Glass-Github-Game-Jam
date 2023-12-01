using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIStateMachine : StateMachine
{
    //[field: SerializeField] public UITweener UITweener { get; private set; }

    [field: SerializeField] public PlayerStateMachine playerStateMachine;

    [field: SerializeField] public GameStateMachine GameStateMachine { get; private set; }

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;

    [field: SerializeField] public RectTransform FadeToBlackPanel { get; private set; }
    [field: SerializeField] public GameObject MenuPanelObject { get; private set; }
    [field: SerializeField] public RectTransform MenuPanel { get; private set; }
    [field: SerializeField] public RectTransform MenuPanelHidingPos { get; private set; }
    [field: SerializeField] public RectTransform MenuPanelShowingPos { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TitlePanelText { get; private set; }
    [field: SerializeField] public RectTransform TitlePanel { get; private set; }
    [field: SerializeField] public RectTransform TitlePanelHidingPos { get; private set; }
    [field: SerializeField] public RectTransform TitlePanelShowingPos { get; private set; }
    [field: SerializeField] public GameObject FirstButtonSelected;
    [field: SerializeField] public Button ResumeButton { get; private set; }
    [field: SerializeField] public Button RestartFromLastCheckpointButton { get; private set; }
    [field: SerializeField] public Button ExitButton { get; private set; }
    [field: SerializeField] public Button SkipToNextCheckpointButton { get; private set; }
    [field: SerializeField] public Slider AmbienceSlider { get; private set; }
    [field: SerializeField] public Slider XAxisSensitivitySlider { get; private set; }
    [field: SerializeField] public Slider YAxisSensitivitySlider { get; private set; }
    [field: SerializeField] public Slider SFXSlider { get; private set; }
    [field: SerializeField] public Toggle YInvertToggle { get; private set; }

    [field: SerializeField] public RectTransform AreYouSurePanel { get; private set; }
    [field: SerializeField] public GameObject AreYouSureFirstSelection { get; private set; }

    [field: SerializeField] public bool PlatformFailureDetected;

    [field: SerializeField] public GameEventScriptableObject HideAllUIEvent;

    [field: SerializeField] public GameEventScriptableObject ShowHUDAfterPauseEvent;

    [field: SerializeField] public GameEventScriptableObject PauseMenuStartedEvent;

    [field: SerializeField] public GameEventScriptableObject PauseMenuOnEvent;

    [field: SerializeField] public GameEventScriptableObject PauseMenuOffEvent;
    [field: SerializeField] public GameEventScriptableObject GameOverMenuOffEvent;
    [field: SerializeField] public GameEventScriptableObject SkipToNextCheckpointEvent;

    [field: SerializeField] public bool IsGameOver;

    //[field: SerializeField] private int idleStateHash = Animator.StringToHash("Idle State");

    //[field: SerializeField] private int movingStateHash = Animator.StringToHash("Moving State");

    //public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();

        YInvertToggle.isOn = !SettingsReader.Instance.GameSettings.InvertYAxis;

        XAxisSensitivitySlider.value = SettingsReader.Instance.GameSettings.XAxisLookSensitivity;

        YAxisSensitivitySlider.value = SettingsReader.Instance.GameSettings.YAxisLookSensitivity;

        SwitchState(new UIHiddenState(this));

        
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    public void SkipToNextCheckpoint()
    {
        SwitchState(new UIHiddenState(this));
        SkipToNextCheckpointEvent?.Raise();
    }

    public void OnExitButton()
    {
        AreYouSurePanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(AreYouSureFirstSelection);
    }

    public void OnDeclineToExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FirstButtonSelected);
        AreYouSurePanel.gameObject.SetActive(false);
    }

    public void OnAcceptExit()
    {
        //Destroy(GameObject.FindGameObjectWithTag("Settings Reader"));
        //EndEventEnded?.Raise();
        OnResume();
        StartCoroutine(DelayReloadGame());
    }

    private IEnumerator DelayReloadGame()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Loading");
    }
    /*public void SwitchToActiveState()
    {
        if (CurrentState != "Game Active State")
        {
            SwitchState(new GameActiveState(this));
            return;
        }
    }

    public void ReturnToActiveState()
    {
        if (CurrentState != "Game Active State")
        {
            SwitchState(new GameActiveState(this));
            return;
        }
    }*/

    /*public void OnPlatformFailureDetected()
    {
        playerStateMachine.Health.DealPlatformFailDamage();
        float playerHealth = playerStateMachine.Health.health;
        if (playerHealth > 0)
        {
            SwitchState(new GameResetToCheckpointState(this));
        }
        if (playerHealth == 0)
        {
            //UIStateMachine.SwitchState(Gameovermenu);

            SwitchState(new GameOverState(this));
        }
    }

    public void OnRestartLevel()
    {
        SwitchState(new GameResetToLevelState(this));
    }*/

    public void OnPlatformFailureEvent()
    {
        PlatformFailureDetected = true;
    }

    public void OnPlayerResetToCheckpoint()
    {
        PlatformFailureDetected = false;
    }

    public void OnPlayerDied()
    {
        //playerStateMachine.InputReader.DisallowPausing();
        TitlePanelText.text = "You Died... Again";
        ResumeButton.gameObject.SetActive(false);
        RestartFromLastCheckpointButton.gameObject.SetActive(true);
        FirstButtonSelected = RestartFromLastCheckpointButton.gameObject;
        
        // Create a new navigation instance for exit button
        Navigation newExitButtonNav = new Navigation();
        newExitButtonNav.mode = Navigation.Mode.Explicit; // Set the navigation mode
        newExitButtonNav.selectOnLeft = SkipToNextCheckpointButton;
        newExitButtonNav.selectOnRight = RestartFromLastCheckpointButton;
        newExitButtonNav.selectOnDown = XAxisSensitivitySlider;
        newExitButtonNav.selectOnUp = AmbienceSlider;


        ExitButton.navigation = newExitButtonNav;

        // Create a new navigation instance for Skip button
        Navigation newSkipButtonNav = new Navigation();
        newSkipButtonNav.mode = Navigation.Mode.Explicit; // Set the navigation mode
        newSkipButtonNav.selectOnLeft = RestartFromLastCheckpointButton;
        newSkipButtonNav.selectOnRight = ExitButton;
        newSkipButtonNav.selectOnDown = XAxisSensitivitySlider;
        newSkipButtonNav.selectOnUp = AmbienceSlider;


        SkipToNextCheckpointButton.navigation = newSkipButtonNav;

        Navigation newAmbientSliderNav = new Navigation();
        newAmbientSliderNav.mode = Navigation.Mode.Explicit;
        newAmbientSliderNav.selectOnDown = RestartFromLastCheckpointButton;
        newAmbientSliderNav.selectOnUp = SFXSlider;
        newAmbientSliderNav.selectOnLeft = null;
        newAmbientSliderNav.selectOnRight = null;

        AmbienceSlider.navigation = newAmbientSliderNav;

        Navigation newXAxisSliderNav = new Navigation();
        newXAxisSliderNav.mode = Navigation.Mode.Explicit;
        newXAxisSliderNav.selectOnDown = YAxisSensitivitySlider;
        newXAxisSliderNav.selectOnUp = RestartFromLastCheckpointButton;
        newXAxisSliderNav.selectOnLeft = null;
        newXAxisSliderNav.selectOnRight = null;

        XAxisSensitivitySlider.navigation = newXAxisSliderNav;




        FadeToBlackPanel.DOScale(Vector3.one, .2f).OnComplete(() =>
        {
            IsGameOver = true;
            SwitchState(new UIPauseMenuState(this));
        });
    }

    public void OnRestartFromLastCheckpoint()
    {
        IsGameOver = false;
        SwitchState(new UIHiddenState(this));
        GameOverMenuOffEvent?.Raise();
        playerStateMachine.SwitchState(new PlayerGameOverResetState(playerStateMachine));
    }
    public void OnResume()
    {
        SwitchState(new UIHiddenState(this));
    }
    public void SwitchToUIHiddenState()
    {
        SwitchState(new UIHiddenState(this));
    }
}
