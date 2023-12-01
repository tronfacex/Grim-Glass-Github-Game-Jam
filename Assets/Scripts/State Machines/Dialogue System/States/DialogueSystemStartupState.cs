using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class DialogueSystemStartupState : DialogueSystemBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Dialogue Startup State");
    [SerializeField] private string currentState = "Dialogue Startup State";

    //private int SceneIndex;

    public DialogueSystemStartupState(DialogueSystemStateMachine stateMachine, int sceneIndex) : base(stateMachine) 
    {
        //SceneIndex = sceneIndex;
        stateMachine.SceneIndex = sceneIndex;
    }

    public override void Enter()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;

        //stateMachine.tutorialShown = false;

        stateMachine.PlayerStateMachine.PlayerInCutscene = true;

        if (stateMachine.SceneIndex == 4)
        {
            //etc.
            //Loadscene additively here using scene index
            SceneManager.LoadSceneAsync(stateMachine.SceneIndex, LoadSceneMode.Additive);
        }
        if (stateMachine.SceneIndex == 5)
        {
            //etc.
            //Loadscene additively here using scene index
            SceneManager.LoadSceneAsync(stateMachine.SceneIndex, LoadSceneMode.Additive);
        }
        if (stateMachine.SceneIndex == 6)
        {
            //etc.
            //Loadscene additively here using scene index
            SceneManager.LoadSceneAsync(stateMachine.SceneIndex, LoadSceneMode.Additive);
        }

        stateMachine.PlayerStateMachine.SwitchState(new PlayerIdleState(stateMachine.PlayerStateMachine));
    }

    public override void Tick(float deltaTime)
    {
        

    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        stateMachine.FadeToDialogueStateCompleteEvent?.Raise();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == stateMachine.SceneIndex)
        {
            //Find all necessary references in new scene
            //stateMachine.DialoguesList.Clear();
            //stateMachine.BobbyPanelBools.Clear();
            //stateMachine.GrimPanelBools.Clear();

            //DialogueTextManager dialogTextManager;

            stateMachine.DialogTextManager = GameObject.FindGameObjectWithTag("Dialogue Text Manager").GetComponent<DialogueTextManager>();

            stateMachine.DialoguesList = stateMachine.DialogTextManager.DialogueList;
            stateMachine.GrimPanelBools = stateMachine.DialogTextManager.GrimPanelBoolList;
            stateMachine.BobbyPanelBools = stateMachine.DialogTextManager.BobbyPanelBoolList;

            if (stateMachine.DialoguesList.Count != stateMachine.GrimPanelBools.Count || stateMachine.DialoguesList.Count != stateMachine.BobbyPanelBools.Count)
            {
                Debug.Log("One or more characters does not have the correct number of charcter panel bools in their dialogue text manager");
            }

            //Change to references on Dialogue Text Manager
            stateMachine.BackgroundPanel = stateMachine.DialogTextManager.BackgroundPanel;

            stateMachine.GrimPanel = stateMachine.DialogTextManager.GrimPanel;
            
            stateMachine.GrimPanel2 = stateMachine.DialogTextManager.GrimPanel2;

            stateMachine.BobbyPanel = stateMachine.DialogTextManager.BobbyPanel;

            stateMachine.BobbyPanel2 = stateMachine.DialogTextManager.BobbyPanel2;

            stateMachine.DialogueTextObject = stateMachine.DialogTextManager.DialogueTextPanel;

            stateMachine.DialogueText = stateMachine.DialogTextManager.DialogueText;

            stateMachine.PressAToAdvance = stateMachine.DialogTextManager.PressAToAdvance;

            stateMachine.PressXToAdvance = stateMachine.DialogTextManager.PressXToAdvance;

            stateMachine.ClickToAdvance = stateMachine.DialogTextManager.ClickToAdvance;

            stateMachine.DialogueFadeToBlackPanel = stateMachine.DialogTextManager.FadeToBlackPanel;

            stateMachine.DialoguePanelTweenPos = stateMachine.DialogTextManager.DialogTextPanelTweenPos;

            stateMachine.BackgroundPanelTweenPos = stateMachine.DialogTextManager.BackgroundPanelTweenPos;

            //stateMachine.containsTutorialSlide = stateMachine.DialogTextManager.ContainsTutorialPanel;

            if (stateMachine.DialogTextManager.ContainsTutorialPanel)
            {
                stateMachine.DialogTextManager.TutorialPanelShown = false;
                stateMachine.TutorialPanel = stateMachine.DialogTextManager.TutorialPanel;
                stateMachine.TutorialPanelButtonPanel = stateMachine.DialogTextManager.TutorialPanelButtonPanel;
            }
            if (stateMachine.DialogTextManager.ContainsTutorialPanel2)
            {
                stateMachine.DialogTextManager.TutorialPanel2Shown = false;
                stateMachine.TutorialPanel2 = stateMachine.DialogTextManager.TutorialPanel2;
            }

            stateMachine.BackgroundPanel.GetComponent<RectTransform>().DOAnchorPos(stateMachine.BackgroundPanelTweenPos.anchoredPosition, 1);

            stateMachine.DialogueTextObject.GetComponent<RectTransform>().DOAnchorPos(stateMachine.DialoguePanelTweenPos.anchoredPosition, .75f).SetEase(Ease.OutBack);

            stateMachine.SwitchState(new DialogueSystemAdvanceToNextState(stateMachine, 0, stateMachine.GrimPanelBools[0], stateMachine.BobbyPanelBools[0]));
        }
    }

}
