using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPuzzleListeningState : SoundPuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Sound Puzzle Listening State");
    [SerializeField] private string currentState = "Sound Puzzle Listening State";

    public SoundPuzzleListeningState(SoundPuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;


        
    }

    public override void Tick(float deltaTime)
    {
        


    }

    public override void Exit()
    {

    }
}
