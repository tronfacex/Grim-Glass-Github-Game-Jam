using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetToLevelState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Reset To Level State");
    [SerializeField] private string currentState = "Game Reset To Level State";

    public GameResetToLevelState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
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
