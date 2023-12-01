using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatformMovingState : MovingPlatformBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Moving State");

    private float timer;

    [SerializeField] private bool TweenTriggered;

    public MovingPlatformMovingState(MovingPlatformStateMachine stateMachine/*, float delayTimer*/) : base(stateMachine) 
    {
        //timer = delayTimer;
    }

    //private float timerTest = 3f;

    public override void Enter()
    {
        stateMachine.CurrentState = currentStateHash;
        //Debug.Log("Moving platforms started");
        

        stateMachine.PlatformTweener.TweenPlatform();
    }

    public override void Tick(float deltaTime)
    {
        /*timer = Mathf.Max(timer - deltaTime, 0f);

        if (timer == 0f && !TweenTriggered)
        {
            OnTimerComplete();
        }*/

        /*timerTest = Mathf.Max(timerTest - deltaTime, 0f);

        if (timerTest == 0)
        {
            stateMachine.SwitchState(new MovingPlatformIdleState(stateMachine, true));
        }*/

    }

    public override void Exit()
    {
        
    }

    public void OnTimerComplete()
    {
        TweenTriggered = true;
        stateMachine.PlatformTweener.TweenPlatform();
    }
}
