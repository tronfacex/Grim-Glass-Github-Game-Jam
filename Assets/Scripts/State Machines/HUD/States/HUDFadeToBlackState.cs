using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUDFadeToBlackState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Black State");
    [SerializeField] private string currentState = "HUD Black State";

    private float DelayTime;
    private bool FadedToBlack;

    public HUDFadeToBlackState(HUDStateMachine stateMachine, float delayTime) : base(stateMachine) 
    {
        DelayTime = delayTime;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.FadeToBlackPanel.gameObject.SetActive(true);
        //Probably just a custom method to tween alpha of black panel on

    }

    public override void Tick(float deltaTime)
    {
        DelayTime = Mathf.Max(DelayTime - deltaTime, 0f);
        if (DelayTime == 0 && !FadedToBlack)
        {
            FadedToBlack = true;
            stateMachine.FadeToBlackPanel.DOScale(1, .8f).OnComplete(stateMachine.FadeToBlackCompleteEvent.Raise);
        }

    }

    public override void Exit()
    {
        //stateMachine.FadeToBlackPanel.DOScale(0, 1.25f);
    }
}
