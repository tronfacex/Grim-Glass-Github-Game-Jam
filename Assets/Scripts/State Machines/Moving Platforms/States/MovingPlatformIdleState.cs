using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformIdleState : MovingPlatformBaseState
{
    [SerializeField] private bool fromMovingState;

    [SerializeField] private int currentStateHash = Animator.StringToHash("Idle State");

    public MovingPlatformIdleState(MovingPlatformStateMachine stateMachine, bool FromMovingState) : base(stateMachine) 
    {
        fromMovingState = FromMovingState;
    }

    //private float countdown = 5f;

    public override void Enter()
    {
        //Debug.Log("Moving platforms Idle");
        stateMachine.CurrentState = currentStateHash;

        /*if (stateMachine.PlatformTweener.OnDropVFX != null && stateMachine.PlatformTweener.OnDropVFX.activeSelf)
        {
            stateMachine.PlatformTweener.OnDropVFX.SetActive(false);
        }*/

        if (fromMovingState)
        {
            stateMachine.PlatformTweener.OnIdle();
        }
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
