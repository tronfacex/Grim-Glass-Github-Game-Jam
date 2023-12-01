using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScalePuzzleListeningState : ScalePuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Scale Puzzle Listening State");
    [SerializeField] private string currentState = "Scale Puzzle Listening State";

    public ScalePuzzleListeningState(ScalePuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        float fillLevelDifference = stateMachine.LeftMilkFillLevel - stateMachine.RightMilkFillLevel;
        float maxTiltAngle = 8.6f;
        float newZRotation = Mathf.Clamp(fillLevelDifference * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

        Vector3 armZRotation = new Vector3(0, stateMachine.ScaleArm.eulerAngles.y, newZRotation);
        stateMachine.ScaleArm.DORotate(armZRotation, 0.0001f);

        stateMachine.LeftMilkRenderer.sprite = stateMachine.GetSpriteForFillLevel(stateMachine.LeftMilkFillLevel);
        stateMachine.RightMilkRenderer.sprite = stateMachine.GetSpriteForFillLevel(stateMachine.RightMilkFillLevel);

    }

    public override void Tick(float deltaTime)
    {

        


    }

    public override void Exit()
    {

    }
}
