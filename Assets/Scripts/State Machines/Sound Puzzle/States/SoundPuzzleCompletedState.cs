using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPuzzleCompletedState : SoundPuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Sound Puzzle Completed State");
    [SerializeField] private string currentState = "Sound Puzzle Completed State";

    private float delayTime = 4f;
    private float explosionDelayTime;

    private bool explosionTriggered;
    private bool puzzleCompleted;

    public SoundPuzzleCompletedState(SoundPuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {

        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.PlayExampleCam.Priority = 2;

        explosionDelayTime = delayTime * .7f;

    }

    public override void Tick(float deltaTime)
    {
        delayTime = Mathf.Max(delayTime - deltaTime, 0f);
        explosionDelayTime = Mathf.Max(explosionDelayTime - deltaTime, 0f);

        if (explosionDelayTime == 0 && !explosionTriggered)
        {
            explosionTriggered = true;

            AudioManager.instance.PlayOneShot(FMODEvents.instance.explosionLong, stateMachine.PuzzleWall.transform.position);

            stateMachine.PuzzleWall.GetComponent<MeshRenderer>().enabled = false;
            stateMachine.PuzzleWall.GetComponent<MeshCollider>().enabled = false;

            stateMachine.Answer1Sign.gameObject.SetActive(false);
            stateMachine.Answer2Sign.gameObject.SetActive(false);
            stateMachine.Answer3Sign.gameObject.SetActive(false);

            stateMachine.Explosion.SetActive(true);
            stateMachine.BoomObject.SetActive(true);
        }

        if (delayTime == 0 && !puzzleCompleted)
        {
            puzzleCompleted = true;
            stateMachine.PlayExampleCam.Priority = 0;
            stateMachine.Explosion.SetActive(false);
            stateMachine.BoomObject.SetActive(false);
            stateMachine.ExamplePlayButtonStateMachine.SuccessfullyPressed = true;
            stateMachine.SoundPuzzleCompletedEvent?.Raise();
        }


    }

    public override void Exit()
    {
      
    }
}
