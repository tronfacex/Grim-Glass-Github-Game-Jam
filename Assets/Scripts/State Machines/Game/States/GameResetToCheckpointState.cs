using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetToCheckpointState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Reset To Checkpoint State");
    [SerializeField] private string currentState = "Game Reset To Checkpoint State";

    public GameResetToCheckpointState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Reset To Checkpoint State");
        stateMachine.CurrentStateHash = currentStateHash;
        GameDataReader.Instance.GameData.CurrentGameStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;
        GameDataReader.Instance.GameData.CurrentGameState = currentState;
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}
