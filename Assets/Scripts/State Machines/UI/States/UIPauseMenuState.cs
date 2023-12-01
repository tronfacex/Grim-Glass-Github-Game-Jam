using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class UIPauseMenuState : UIBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("UI Pause State");
    [SerializeField] private string currentState = "UI Pause State";

    private bool isGameOver;

    public UIPauseMenuState(UIStateMachine stateMachine) : base(stateMachine) 
    {

    }

    public override void Enter()
    {
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        
        stateMachine.PauseMenuStartedEvent?.Raise();
        stateMachine.FadeToBlackPanel.DOScale(Vector3.one, 0.2f);

        stateMachine.playerStateMachine.InputReader.PauseEvent += OnPause;

        stateMachine.MenuPanelObject.SetActive(true);
        stateMachine.TitlePanel.DOAnchorPos(stateMachine.TitlePanelShowingPos.anchoredPosition, .2f).OnComplete(() => { stateMachine.PauseMenuOnEvent?.Raise();});
        stateMachine.MenuPanel.DOAnchorPos(stateMachine.MenuPanelShowingPos.anchoredPosition, .1f);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(stateMachine.FirstButtonSelected);
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {
        stateMachine.playerStateMachine.InputReader.PauseEvent -= OnPause;
        stateMachine.FadeToBlackPanel.localScale = Vector3.zero;

        if (stateMachine.playerStateMachine.HasMetBobby)
        {
            stateMachine.ShowHUDAfterPauseEvent?.Raise();
        }
        stateMachine.PauseMenuOffEvent?.Raise();
        stateMachine.TitlePanel.DOAnchorPos(stateMachine.TitlePanelHidingPos.anchoredPosition, .2f).OnComplete(() => { stateMachine.MenuPanelObject.SetActive(false);});
        stateMachine.MenuPanel.DOAnchorPos(stateMachine.MenuPanelHidingPos.anchoredPosition, .1f);
    }

    private void OnPause()
    {
        stateMachine.SwitchState(new UIHiddenState(stateMachine));
    }
}

