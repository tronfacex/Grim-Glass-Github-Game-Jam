using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Pause State");
    [SerializeField] private string currentState = "Game Pause State";

    public GamePauseState(GameStateMachine stateMachine) : base(stateMachine) { }

    //UI SHOULD PUSH GAME STATE HERE AFTER THE MENU TWEEN IS COMPLETE
    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        GameDataReader.Instance.GameData.CurrentGameStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;
        GameDataReader.Instance.GameData.CurrentGameState = currentState;

        Time.timeScale = 0;

        GameDataReader.Instance.GameData.CurrentTimeScale = 0;

    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
            GameDataReader.Instance.GameData.CurrentTimeScale = 1;
        }
    }

    private void OnUnpause()
    {
        Time.timeScale = 1;
        stateMachine.SwitchState(new GameActiveState(stateMachine));
    }

    //This was going to be triggered by an event in the UI Statemachine but I think it's unnecessary
    private void OnPauseMenuTweened()
    {
        Time.timeScale = 0;
    }
}
