using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPuzzleFailState : SoundPuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Sound Puzzle Failed State");
    [SerializeField] private string currentState = "Sound Puzzle Failed State";

    private float delayTimer;
    private float failDelayTimer;
    private bool failTriggered;
    private bool failCompleted;

    public SoundPuzzleFailState(SoundPuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {

        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.PlayExampleCam.Priority = 2;

        delayTimer = stateMachine.CameraHoldDuration;

        failDelayTimer = delayTimer * .7f;

        stateMachine.playerStateMachine.SwitchState(new PlayerIdleState(stateMachine.playerStateMachine));

        

    }

    public override void Tick(float deltaTime)
    {
        delayTimer = Mathf.Max(delayTimer - deltaTime, 0f);
        failDelayTimer = Mathf.Max(failDelayTimer - deltaTime, 0f);

        if (failDelayTimer == 0 && !failTriggered)
        {
            failTriggered = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.puzzleFailTone, stateMachine.PuzzleWall.transform.position);
        }

        if (delayTimer == 0 && !failCompleted)
        {
            failCompleted = true;
            stateMachine.SwitchState(new SoundPuzzleListeningState(stateMachine));
        }


    }

    public override void Exit()
    {
        stateMachine.PlayExampleCam.Priority = 0;
        stateMachine.Answer1Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer2Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer3Sign.material = stateMachine.NeutralLight;

        stateMachine.BlueBottleButtonStateMachine.SuccessfullyPressed = false;
        stateMachine.GreenBottleButtonStateMachine.SuccessfullyPressed = false;
        stateMachine.RedBottleButtonStateMachine.SuccessfullyPressed = false;

        stateMachine.playerStateMachine.SwitchState(new PlayerFreeMoveState(stateMachine.playerStateMachine));
    }
}
