using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUDHiddenState : HUDBaseState
{
    private float HideDelay;
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Hidden State");
    [SerializeField] private string currentState = "HUD Hidden State";

    public HUDHiddenState(HUDStateMachine stateMachine, float hideDelay) : base(stateMachine) 
    {
        HideDelay = hideDelay;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        
        stateMachine.CurrentState = currentState;

        //stateMachine.HUDPanel.DOAnchorPos(stateMachine.HUDPanelHidingPos.anchoredPosition, .15f);


        //stateMachine.HideAllHUDElements(HideDelay);

        //stateMachine.SwitchState(new GameActiveState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}

