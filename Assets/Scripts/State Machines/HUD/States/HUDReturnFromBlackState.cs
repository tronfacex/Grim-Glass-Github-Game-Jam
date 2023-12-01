using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUDReturnFromBlackState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Return From Black State");
    [SerializeField] private string currentState = "HUD Return From Black State";

    private float DelayTime;
    private bool ReturnedFromBlack;

    public HUDReturnFromBlackState(HUDStateMachine stateMachine, float delayTime) : base(stateMachine) 
    {
        DelayTime = delayTime;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;


        //Probably just a custom method to tween alpha of black panel off

    }

    public override void Tick(float deltaTime)
    {
        DelayTime = Mathf.Max(DelayTime - deltaTime, 0f);
        if (DelayTime == 0 && !ReturnedFromBlack)
        {
            ReturnedFromBlack = true;
            stateMachine.FadeToBlackPanel.DOScale(0, .8f).OnComplete(() => 
            {
                stateMachine.SwitchState(new HUDShowAllState(stateMachine, .5f));
                stateMachine.FadeReturnCompleteEvent.Raise();
            });

        }


    }

    public override void Exit()
    {
        //maybe an event if needed
        stateMachine.FadeToBlackPanel.gameObject.SetActive(false);
    }
}

