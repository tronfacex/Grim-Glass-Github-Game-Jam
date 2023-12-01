using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogueSystemPlayerReadingState : DialogueSystemBaseState
{
    private float HideDelay;
    [SerializeField] private int currentStateHash = Animator.StringToHash("Dialogue Reading State");
    [SerializeField] private string currentState = "Dialogue Reading State";

    private int DialogueIndex;
    private bool tutorialStarted = false;
    private float tutorialDelay = .8f;
    private bool tutorialDelayElapsed = false;
    private bool allowPlayerToPassTutorial = true;

    public DialogueSystemPlayerReadingState(DialogueSystemStateMachine stateMachine, int dialogueIndex) : base(stateMachine) 
    {
        DialogueIndex = dialogueIndex;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;

        //Subscribe to player advance dialogue input here
        stateMachine.PlayerStateMachine.InputReader.JumpEvent += OnAdvance;
        stateMachine.PlayerStateMachine.InputReader.AttackEvent += OnAdvance;

        //tutorialDelay = .8f;
        //tutorialDelayElapsed = false;
    }

    public override void Tick(float deltaTime)
    {
        if (tutorialStarted)
        {
            tutorialDelay = Mathf.Max(tutorialDelay - deltaTime, 0f);
            if (tutorialDelay == 0 && !tutorialDelayElapsed)
            {
                tutorialDelayElapsed = true;
                allowPlayerToPassTutorial = true;
                /*stateMachine.TutorialPanelButtonPanel.gameObject.SetActive(true);
                if (GameDataReader.Instance.GameData.ControllerType == "Playstation")
                {
                    stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(false);
                    stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(true);
                    stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(false);
                }
                if (GameDataReader.Instance.GameData.ControllerType == "Xbox")
                {
                    stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(false);
                    stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(false);
                    stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(true);
                }
                if (GameDataReader.Instance.GameData.ControllerType == "KBM")
                {
                    stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(true);
                    stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(false);
                    stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(false);
                }*/
                SetTutorialButtonPanel();
            }
        }


    }

    public override void Exit()
    {
        stateMachine.PlayerStateMachine.InputReader.JumpEvent -= OnAdvance;
        stateMachine.PlayerStateMachine.InputReader.AttackEvent -= OnAdvance;
    }

    private void OnAdvance()
    {
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.PlayerStateMachine.transform.position);
        if (DialogueIndex < stateMachine.DialoguesList.Count - 1)
        {
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.DialogTextManager.transform.position);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.PlayerStateMachine.transform.position);

            DialogueIndex++;

            stateMachine.ActivePressToContinuePrompt.transform.localScale = Vector3.zero;
            stateMachine.ActivePressToContinuePrompt.gameObject.SetActive(false);

            stateMachine.SwitchState(new DialogueSystemAdvanceToNextState(stateMachine, DialogueIndex, stateMachine.GrimPanelBools[DialogueIndex], stateMachine.BobbyPanelBools[DialogueIndex]));
        }
        else
        {
            //if tutorial panel bool false show tutorial panel
            if (stateMachine.DialogTextManager.ContainsTutorialPanel && !stateMachine.DialogTextManager.TutorialPanelShown)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.PlayerStateMachine.transform.position);
                tutorialStarted = true;
                allowPlayerToPassTutorial = false;
                stateMachine.DialogTextManager.TutorialPanelShown = true;
                stateMachine.TutorialPanel.DOAnchorPos(stateMachine.BackgroundPanelTweenPos.anchoredPosition, .4f);
            }
            else
            {
                if (!allowPlayerToPassTutorial)
                {
                    return;
                }
                if (stateMachine.DialogTextManager.ContainsTutorialPanel2 && !stateMachine.DialogTextManager.TutorialPanel2Shown)
                {
                    //tutorialDelay = .8f;
                    //tutorialDelayElapsed = false;
                    stateMachine.TutorialPanelButtonPanel.DOAnchorPos(stateMachine.DialogTextManager.TutorialPanelButtonPanel2Pos.anchoredPosition, 0f, true).OnComplete(() => { stateMachine.TutorialPanelButtonPanel.gameObject.SetActive(false); });
                    //stateMachine.TutorialPanelButtonPanel.gameObject.SetActive(false);
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.PlayerStateMachine.transform.position);
                    tutorialStarted = true;
                    allowPlayerToPassTutorial = false;
                    stateMachine.DialogTextManager.TutorialPanel2Shown = true;
                    stateMachine.TutorialPanel2.DOAnchorPos(stateMachine.BackgroundPanelTweenPos.anchoredPosition, .4f).OnComplete(() => { stateMachine.DialogTextManager.TutorialPanel.gameObject.SetActive(false); allowPlayerToPassTutorial = true; SetTutorialButtonPanel(); });
                }
                else
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.uIClick, stateMachine.PlayerStateMachine.transform.position);
                    stateMachine.SwitchState(new DialogueSystemExitState(stateMachine));
                }
            }
        }
    }

    private void SetTutorialButtonPanel()
    {
        stateMachine.TutorialPanelButtonPanel.gameObject.SetActive(true);
        if (GameDataReader.Instance.GameData.ControllerType == "Playstation")
        {
            stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(false);
            stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(true);
            stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(false);
        }
        if (GameDataReader.Instance.GameData.ControllerType == "Xbox")
        {
            stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(false);
            stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(false);
            stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(true);
        }
        if (GameDataReader.Instance.GameData.ControllerType == "KBM")
        {
            stateMachine.DialogTextManager.ClickToAdvanceTut.gameObject.SetActive(true);
            stateMachine.DialogTextManager.PressXToAdvanceTut.gameObject.SetActive(false);
            stateMachine.DialogTextManager.PressAToAdvanceTut.gameObject.SetActive(false);
        }
    }
}

