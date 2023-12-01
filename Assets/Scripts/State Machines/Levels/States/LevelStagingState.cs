using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStagingState : LevelBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Staging State");

    public LevelStagingState(LevelStateMachine stateMachine) : base(stateMachine) {  }

    //private float countdown = 5f;

    public override void Enter()
    {
        //Debug.Log("Moving platforms Idle");
        stateMachine.CurrentState = currentStateHash;

        /*if (fromMovingState)
        {
            stateMachine.PlatformTweener.OnIdle();
        }*/
    }

    public override void Tick(float deltaTime)
    {
        //countdown = Mathf.Max(countdown - deltaTime, 0f);

        /*if (countdown == 0)
        {
            stateMachine.SwitchState(new MovingPlatformMovingState(stateMachine));
        }*/


    }

    public override void Exit()
    {

    }
}

