using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueSystemAdvanceToNextState : DialogueSystemBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Dialogue Advance To Next State");
    [SerializeField] private string currentState = "Dialogue Advance To Next State";

    private int DialogueIndex;
    private bool GrimPanel1Visible;
    private bool BobbyPanel1Visible;

    public DialogueSystemAdvanceToNextState(DialogueSystemStateMachine stateMachine, int dialogueIndex, bool grimPanel1, bool bobbyPanel1) : base(stateMachine)
    {
        DialogueIndex = dialogueIndex;
        GrimPanel1Visible = grimPanel1;
        BobbyPanel1Visible = bobbyPanel1;
    }

    public override void Enter()
    {
        stateMachine.CurrentStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;

        if (DialogueIndex != 0)
        {
            stateMachine.DialogueTextObject.transform.DOScale(0, .5f).SetEase(Ease.InBack).OnComplete(SetDialogueText);
        }
        else
        {
            SetDialogueText();
        }
        

        SetCharacterImages();

    }

    public override void Tick(float deltaTime)
    {


    }

    public override void Exit()
    {
        /*if (DialogueIndex < stateMachine.DialoguesList.Count - 1)
        {
            DialogueIndex++;

            stateMachine.ActivePressToContinuePrompt.transform.localScale = Vector3.zero;
            stateMachine.ActivePressToContinuePrompt.SetActive(false);


        }*/
        //stateMachine.ActivePressToContinuePrompt.transform.localScale = Vector3.zero;
        //stateMachine.ActivePressToContinuePrompt.gameObject.SetActive(false);
    }

    private void SetDialogueText()
    {
        stateMachine.DialogueText.text = stateMachine.DialoguesList[DialogueIndex];

        stateMachine.DialogueTextObject.transform.DOScale(1, .33f).SetEase(Ease.OutBack).OnComplete(SetAdvanceToNextDialogPrompt);
    }

    private void SetCharacterImages()
    {
        stateMachine.BobbyPanel.SetActive(stateMachine.BobbyPanelBools[DialogueIndex]);
        stateMachine.BobbyPanel2.SetActive(!stateMachine.BobbyPanelBools[DialogueIndex]);

        stateMachine.GrimPanel.SetActive(stateMachine.GrimPanelBools[DialogueIndex]);
        stateMachine.GrimPanel2.SetActive(!stateMachine.GrimPanelBools[DialogueIndex]);
    }

    public void OnAdvanceText()
    {
        if (DialogueIndex < stateMachine.DialoguesList.Count - 1)
        {
            DialogueIndex++;
            //stateMachine.SwitchState(new Dial)
        }
        else
        {
            //EXIT DIALOGUE HERE
           // stateMachine.SwitchState(new DialogueSystemAdvanceToNextState(DialogueIndex, ))
        }
    }
    
    private void SwitchToPlayerReadingState()
    {
        stateMachine.SwitchState(new DialogueSystemPlayerReadingState(stateMachine, DialogueIndex));
    }

    private void SetAdvanceToNextDialogPrompt()
    {
        if (GameDataReader.Instance.GameData.ControllerType == "Playstation")
        {
            stateMachine.PressXToAdvance.gameObject.SetActive(true);
            stateMachine.PressXToAdvance.rectTransform.DOScale(1, .2f).OnComplete(SwitchToPlayerReadingState);
            stateMachine.ActivePressToContinuePrompt = stateMachine.PressXToAdvance;
            return;
        }
        if (GameDataReader.Instance.GameData.ControllerType == "Xbox")
        {
            stateMachine.PressAToAdvance.gameObject.SetActive(true);
            stateMachine.ActivePressToContinuePrompt = stateMachine.PressAToAdvance;
            stateMachine.PressAToAdvance.rectTransform.DOScale(1, .2f).OnComplete(SwitchToPlayerReadingState);
            return;
        }
        if (GameDataReader.Instance.GameData.ControllerType == "KBM")
        {
            stateMachine.ClickToAdvance.gameObject.SetActive(true);
            stateMachine.ActivePressToContinuePrompt = stateMachine.ClickToAdvance;
            stateMachine.ClickToAdvance.rectTransform.DOScale(1, .2f).OnComplete(SwitchToPlayerReadingState);
            return;
        }
    }
}
