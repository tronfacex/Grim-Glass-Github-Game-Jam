using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystemIdleState : DialogueSystemBaseState
{
    private float HideDelay;
    [SerializeField] private int currentStateHash = Animator.StringToHash("Dialogue Idle State");
    [SerializeField] private string currentState = "Dialogue Idle State";

    public DialogueSystemIdleState(DialogueSystemStateMachine stateMachine) : base(stateMachine) {  }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;

        //Subscribe to player begin dialogue state input here and move to DialogueStartupState


    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}
