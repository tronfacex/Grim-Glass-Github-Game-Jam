using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMovingState : LevelBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Staging State");

    public LevelMovingState(LevelStateMachine stateMachine) : base(stateMachine) { }

    //private float countdown = 5f;

    public override void Enter()
    {
        //Debug.Log("Moving platforms Idle");
        stateMachine.CurrentState = currentStateHash;

        stateMachine.LevelTweener.TweenPlatform();

        /*if (fromMovingState)
        {
            stateMachine.PlatformTweener.OnIdle();
        }*/
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InScene)
        {
            //stateMachine.SwitchState(new )
        }
    }

    public override void Exit()
    {

    }
}
