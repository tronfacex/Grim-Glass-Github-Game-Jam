using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class DialogueSystemExitState : DialogueSystemBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Dialogue Startup State");
    [SerializeField] private string currentState = "Dialogue Startup State";


    public DialogueSystemExitState(DialogueSystemStateMachine stateMachine) : base(stateMachine) {   }

    public override void Enter()
    {
        stateMachine.ExitDialogueStartedEvent?.Raise();
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        //stateMachine.DialogueFadeToBlackPanel.SetActive(true);
        //stateMachine.DialogueFadeToBlackPanel.transform.DOScale(1, .8f);

        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;

        stateMachine.PlayerStateMachine.PlayerInCutscene = false;

        ClearAllReferences();

        SceneManager.UnloadSceneAsync(stateMachine.SceneIndex);

        /*if (stateMachine.SceneIndex == 2)
        {
            //etc.
            //Unloadscene additively here using scene index
            SceneManager.UnloadSceneAsync(stateMachine.SceneIndex);
        }*/

    }

    public override void Tick(float deltaTime)
    {


    }

    public override void Exit()
    {
        //stateMachine.DialogueFadeToBlackPanel.transform.DOScale(0, .5f);
        //stateMachine.DialogueFadeToBlackPanel.SetActive(false);
        stateMachine.DialogueFadeToBlackPanel = null;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        stateMachine.ReturnFromDialogueStateCompleteEvent?.Raise();
        stateMachine.PlayerStateMachine.SwitchState(new PlayerFreeMoveState(stateMachine.PlayerStateMachine));
    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex == stateMachine.SceneIndex)
        {

            stateMachine.SwitchState(new DialogueSystemIdleState(stateMachine));
        }
    }

    public void ClearAllReferences()
    {
        //Find all necessary references in new scene
        stateMachine.DialogTextManager = null;
        stateMachine.DialoguesList.Clear();
        stateMachine.BobbyPanelBools.Clear();
        stateMachine.GrimPanelBools.Clear();

        stateMachine.DialoguesList = null;
        stateMachine.GrimPanelBools = null;
        stateMachine.BobbyPanelBools = null;

        stateMachine.BackgroundPanel = null;
        stateMachine.BackgroundPanelTweenPos = null;
        stateMachine.DialoguePanelTweenPos = null;
        stateMachine.GrimPanel = null;
        stateMachine.GrimPanel2 = null;
        stateMachine.BobbyPanel = null;
        stateMachine.BobbyPanel2 = null;
        stateMachine.DialogueTextObject = null;
        stateMachine.DialogueText = null;

        stateMachine.PressAToAdvance = null;
        stateMachine.PressXToAdvance = null;
        stateMachine.ClickToAdvance = null;
        stateMachine.ActivePressToContinuePrompt = null;
        stateMachine.TutorialPanel = null;
        stateMachine.TutorialPanel2 = null;
        stateMachine.TutorialPanelButtonPanel = null;
        stateMachine.ClickToAdvanceTut = null;
        stateMachine.PressAToAdvanceTut = null;
        stateMachine.PressXToAdvanceTut = null;
    }

}
