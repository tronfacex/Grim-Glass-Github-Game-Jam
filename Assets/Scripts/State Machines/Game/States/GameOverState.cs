using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Over State");
    [SerializeField] private string currentState = "Game Over State";

    private float timer = 1.5f;
    private bool timerExpired = false;
    public GameOverState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        GameDataReader.Instance.GameData.CurrentGameStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;
        GameDataReader.Instance.GameData.CurrentGameState = currentState;

        //stateMachine.RestoreAllEnemyHealthEvent?.Raise();
    }

    public override void Tick(float deltaTime)
    {
        timer = Mathf.Max(timer - deltaTime, 0f);
        if (timer == 0 && !timerExpired)
        {
            timerExpired = true;
            stateMachine.RestoreAllEnemyHealthEvent?.Raise();
        }


    }

    public override void Exit()
    {
        //stateMachine.Res
    }
}
