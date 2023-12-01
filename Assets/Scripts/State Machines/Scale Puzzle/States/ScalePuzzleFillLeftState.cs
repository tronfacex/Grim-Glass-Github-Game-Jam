using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FMODUnity;
using FMOD.Studio;

public class ScalePuzzleFillLeftState : ScalePuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Scale Puzzle Fill Left State");
    [SerializeField] private string currentState = "Scale Puzzle Fill Left State";

    public ScalePuzzleFillLeftState(ScalePuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        stateMachine.ScalePuzzleFillLeftStartEvent?.Raise();

        stateMachine.milkPour = AudioManager.instance.CreateEventInstance(FMODEvents.instance.milkPour);

        stateMachine.MilkPourLeft.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {
        float tolerance = 0.01f; 

        // Check if the values are close enough
        if (Mathf.Abs(stateMachine.RightMilkFillLevel - stateMachine.LeftMilkFillLevel) <= tolerance)
        {
            stateMachine.SwitchState(new ScalePuzzleCompletedState(stateMachine));
            return;
        }

        UpdateMilkPourSound();
        stateMachine.LeftMilkFillLevel = Mathf.Min(stateMachine.LeftMilkFillLevel + (deltaTime * stateMachine.FillSpeedModifier), 1f);


        float fillLevelDifference = stateMachine.LeftMilkFillLevel - stateMachine.RightMilkFillLevel;
        float maxTiltAngle = 8.6f;
        float newZRotation = Mathf.Clamp(fillLevelDifference * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

        Vector3 armZRotation = new Vector3(0, stateMachine.ScaleArm.eulerAngles.y, newZRotation);
        stateMachine.ScaleArm.DORotate(armZRotation, 0.01f);

        stateMachine.LeftMilkRenderer.sprite = stateMachine.GetSpriteForFillLevel(stateMachine.LeftMilkFillLevel);
    }

    public override void Exit()
    {
        stateMachine.ScalePuzzleFillLeftEndEvent?.Raise();
        stateMachine.milkPour.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        stateMachine.milkPour.release();
        stateMachine.MilkPourLeft.SetActive(false);
    }

    private void UpdateMilkPourSound()
    {
        if (stateMachine.LeftMilkFillLevel == 1) 
        {
            stateMachine.milkPour.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            return; 
        }
        PLAYBACK_STATE playbackState;
        stateMachine.milkPour.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(stateMachine.LeftMilk.transform.position);
            stateMachine.milkPour.set3DAttributes(attributes);
            stateMachine.milkPour.start();
        }
    }
}
