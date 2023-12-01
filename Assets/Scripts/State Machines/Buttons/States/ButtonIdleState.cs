using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIdleState : ButtonBaseState
{
    [SerializeField] private bool hasBeenPressed;

    [SerializeField] private int currentStateHash = Animator.StringToHash("Idle State");

    public ButtonIdleState(ButtonStateMachine stateMachine) : base(stateMachine) {  }

    //private float countdown = 5f;

    public override void Enter()
    {
        //Debug.Log("Moving platforms Idle");
        stateMachine.CurrentState = currentStateHash;
    }

    public override void Tick(float deltaTime)
    {
        //countdown = Mathf.Max(countdown - deltaTime, 0f);

        /*if (countdown == 0)
        {
            stateMachine.SwitchState(new MovingPlatformMovingState(stateMachine));
        }*/

        //if (stateMachine.SuccessfullyPressed) { return; }

    }

    public override void Exit()
    {

    }
}