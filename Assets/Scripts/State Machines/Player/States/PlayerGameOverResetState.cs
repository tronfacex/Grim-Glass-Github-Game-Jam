using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameOverResetState : PlayerBaseState
{
    private float countdown = 2f;

    [SerializeField] private Transform checkpointTransform;

    [SerializeField] private List<Transform> levelCheckpointList;

    public PlayerGameOverResetState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.FreeLookCamera.m_Follow = null;

        stateMachine.Health.ResetHealth();

        checkpointTransform = GameDataReader.Instance.GameData.CurrentLevelCheckpointList[GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex];

        stateMachine.SwitchState(new PlayerLandingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        stateMachine.transform.position = checkpointTransform.position;
        stateMachine.transform.rotation = checkpointTransform.rotation;

        stateMachine.FreeLookCamera.m_Follow = stateMachine.CameraFollowTransform;
        stateMachine.FreeLookCamera.m_XAxis.Value = 0f;
        stateMachine.FreeLookCamera.m_YAxis.Value = .6f;
        stateMachine.ResetToCheckPointCompleteEvent?.Raise();

        GameDataReader.Instance.GameData.InGameOver = false;
    }
}
