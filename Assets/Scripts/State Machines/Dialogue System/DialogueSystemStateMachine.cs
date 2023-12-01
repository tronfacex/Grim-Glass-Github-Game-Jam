using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystemStateMachine : StateMachine
{
    //[field: SerializeField] public HUDTweener HUDTweener { get; private set; }

    [field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }
    [field: SerializeField] public GameStateMachine GameStateMachine { get; private set; }

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;

    [field: SerializeField] public int SceneIndex;

    [field: SerializeField] public GameEventScriptableObject FadeToDialogueStateCompleteEvent;

    [field: SerializeField] public GameEventScriptableObject ExitDialogueStartedEvent;

    [field: SerializeField] public GameEventScriptableObject ReturnFromDialogueStateCompleteEvent;

    [field: SerializeField] public DialogueTextManager DialogTextManager;

    [SerializeField] public GameObject DialogueFadeToBlackPanel;

    [field: SerializeField] public GameObject BackgroundPanel;

    [field: SerializeField] public RectTransform BackgroundPanelTweenPos;

    [field: SerializeField] public RectTransform DialoguePanelTweenPos;

    [field: SerializeField] public GameObject DialoguePanel;

    [field: SerializeField] public GameObject DialogueTextObject;

    [field: SerializeField] public Image PressAToAdvance;

    [field: SerializeField] public Image PressXToAdvance;

    [field: SerializeField] public Image ClickToAdvance;

    [field: SerializeField] public Image ActivePressToContinuePrompt;

    [field: SerializeField] public TextMeshProUGUI DialogueText;

    [field: SerializeField] public List<string> DialoguesList;

    [field: SerializeField] public List<bool> GrimPanelBools;

    [field: SerializeField] public List<bool> BobbyPanelBools;

    [field: SerializeField] public GameObject GrimPanel;

    [field: SerializeField] public GameObject GrimPanel2;

    [field: SerializeField] public GameObject BobbyPanel;

    [field: SerializeField] public GameObject BobbyPanel2;

    [field: SerializeField] public RectTransform TutorialPanel;

    [field: SerializeField] public RectTransform TutorialPanel2;

    [field: SerializeField] public RectTransform TutorialPanelButtonPanel;

    [field: SerializeField] public Image PressAToAdvanceTut;

    [field: SerializeField] public Image PressXToAdvanceTut;

    [field: SerializeField] public Image ClickToAdvanceTut;
    //[field: SerializeField] public bool tutorialShown;
    //[field: SerializeField] public bool containsTutorialSlide;

    //[SerializeField] public bool GrimPanel1;
    //[SerializeField] public bool BobbyPanel1;


    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;

        SwitchState(new DialogueSystemIdleState(this));
    }
    private void OnEnable()
    {
        //PlayerStateMachine.Health.OnPlatformFailTakeDamage += HandleResetToCheckpoint;
    }
    private void OnDisable()
    {
        //PlayerStateMachine.Health.OnPlatformFailTakeDamage += HandleResetToCheckpoint;
    }

    public void SwitchToDialogueStartupState(int sceneIndex)
    {
        SwitchState(new DialogueSystemStartupState(this, sceneIndex));
    }
}
