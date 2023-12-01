using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInSceneState : LevelBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("In-Scene State");

    public LevelInSceneState(LevelStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.CurrentState = currentStateHash;

    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {

    }
}
