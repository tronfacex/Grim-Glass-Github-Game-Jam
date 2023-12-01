using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedState : ButtonBaseState
{

    [SerializeField] private int currentStateHash = Animator.StringToHash("Pressed State");

    public ButtonPressedState(ButtonStateMachine stateMachine) : base(stateMachine){  }

    //private float countdown = 5f;

    public override void Enter()
    {
        //Debug.Log("Moving platforms Idle");
        stateMachine.CurrentState = currentStateHash;

        stateMachine.ButtonTiggerEvent?.Raise();

        stateMachine.SuccessfullyPressed = true;
    }

    public override void Tick(float deltaTime)
    {
        //countdown = Mathf.Max(countdown - deltaTime, 0f);

        /*if (countdown == 0)
        {
            stateMachine.SwitchState(new MovingPlatformMovingState(stateMachine));
        }*/

        if (!stateMachine.SuccessfullyPressed)
        {
            stateMachine.SwitchState(new ButtonIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.ReturnButtonSize();
        if (stateMachine.Health != null)
        {
            stateMachine.Health.ResetHealth();
        }
        if (stateMachine.ResetOnExit)
        {
            stateMachine.ButtonTiggerExitEvent?.Raise();
        }
    }
}
