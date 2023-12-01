using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePuzzleResetState : ScalePuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Scale Puzzle Reset State");
    [SerializeField] private string currentState = "Scale Puzzle Reset State";

    public ScalePuzzleResetState(ScalePuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.LeftMilkFillLevel = 0;
        stateMachine.RightMilkFillLevel = 0;

        stateMachine.SwitchState(new ScalePuzzleListeningState(stateMachine));


    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}