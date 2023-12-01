using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActiveState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Active State");
    [SerializeField] private string currentState = "Game Active State";

    public GameActiveState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        GameDataReader.Instance.GameData.CurrentGameStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;
        GameDataReader.Instance.GameData.CurrentGameState = currentState;

        if (GameDataReader.Instance.GameData.CurrentTimeScale != 1)
        {
            GameDataReader.Instance.GameData.CurrentTimeScale = 1;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Tick(float deltaTime)
    {
        //Debug.Log(GameDataReader.Instance.GameData.CurrentGameState);


    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
