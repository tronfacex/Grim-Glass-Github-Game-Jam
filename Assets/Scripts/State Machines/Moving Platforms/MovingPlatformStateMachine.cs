using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPlatformStateMachine : StateMachine
{
    //[field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public PlatformTweener PlatformTweener { get; private set; }

    [field: SerializeField] public PlatformPlayerMover PlatformPlayerMover { get; private set; }

    [field: SerializeField] public BoxCollider PlayerDetectorCollider { get; private set; }

    //[field: SerializeField] private bool moveOnStart;

    //[field: SerializeField] public Transform TweenPosition0 { get; private set; }
    //[field: SerializeField] public Transform TweenPosition1 { get; private set; }
    //[field: SerializeField] public Transform TweenPosition2 { get; private set; }
    [field: SerializeField] public List<Transform> TweenTransforms { get; private set; }

    [field: SerializeField] public int CurrentState;

    [field: SerializeField] private int idleStateHash = Animator.StringToHash("Idle State");

    [field: SerializeField] private int movingStateHash = Animator.StringToHash("Moving State");

    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new MovingPlatformIdleState(this, false));
    }
    public void TogglePlatformMovement()
    {
        if (CurrentState == idleStateHash)
        {
            SwitchState(new MovingPlatformMovingState(this));
            return;
        }
        if (CurrentState == movingStateHash)
        {
            SwitchState(new MovingPlatformIdleState(this, true));
            return;
        }
    }

    public void DelayedTogglePlatformMovement(float delay)
    {
        StartCoroutine(TogglePlatformDelay(delay));
    }

    private IEnumerator TogglePlatformDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (CurrentState != movingStateHash)
        {
            SwitchState(new MovingPlatformMovingState(this));
            //return;
        }
    }
}
