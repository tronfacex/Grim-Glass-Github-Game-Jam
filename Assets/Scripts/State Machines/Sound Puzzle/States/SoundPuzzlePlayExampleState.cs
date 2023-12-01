using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPuzzlePlayExampleState : SoundPuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Sound Puzzle Play Example State");
    [SerializeField] private string currentState = "Sound Puzzle Play Example State";

    private float delayTimer = 1.5f;

    private int answerIndex = 1;

    public SoundPuzzlePlayExampleState(SoundPuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {

        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.PlayExampleCam.Priority = 2;

        stateMachine.Answer1Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer2Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer3Sign.material = stateMachine.NeutralLight;

        stateMachine.BlueBottleButtonStateMachine.SuccessfullyPressed = false;
        stateMachine.GreenBottleButtonStateMachine.SuccessfullyPressed = false;
        stateMachine.RedBottleButtonStateMachine.SuccessfullyPressed = false;

    }

    public override void Tick(float deltaTime)
    {
        delayTimer = Mathf.Max(delayTimer - deltaTime, 0f);

        if (delayTimer == 0 && answerIndex == 1)
        {

            stateMachine.Answer1Sign.material = stateMachine.BlueLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise1, stateMachine.ExamplePlayButtonStateMachine.transform.position);
            delayTimer = 1.5f;
            answerIndex++;
            return;
        }

        if (delayTimer == 0 && answerIndex == 2)
        {

            stateMachine.Answer2Sign.material = stateMachine.RedLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise3, stateMachine.ExamplePlayButtonStateMachine.transform.position);
            delayTimer = 1.5f;
            answerIndex++;
            return;
        }

        if (delayTimer == 0 && answerIndex == 3)
        {
  
            stateMachine.Answer3Sign.material = stateMachine.GreenLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise2, stateMachine.ExamplePlayButtonStateMachine.transform.position);
            delayTimer = 2.5f;
            answerIndex++;
            return;
        }

        if (delayTimer == 0 && answerIndex == 4)
        {
            stateMachine.SwitchState(new SoundPuzzleListeningState(stateMachine));
            
        }

    }

    public override void Exit()
    {
        stateMachine.Answer1Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer2Sign.material = stateMachine.NeutralLight;
        stateMachine.Answer3Sign.material = stateMachine.NeutralLight;

        stateMachine.playerStateMachine.SwitchState(new PlayerFreeMoveState(stateMachine.playerStateMachine));

        stateMachine.PlayExampleCam.Priority = 0;

        stateMachine.ExamplePlayButtonStateMachine.SuccessfullyPressed = false;
    }


}
