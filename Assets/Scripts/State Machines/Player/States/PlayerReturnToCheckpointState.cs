using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReturnToCheckpointState : PlayerBaseState
{
    private float countdown = 2f;

    [SerializeField] private Transform checkpointTransform;

    [SerializeField] private List<Transform> levelCheckpointList;

    public PlayerReturnToCheckpointState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
      
        stateMachine.FreeLookCamera.m_Follow = null;
        
        
        checkpointTransform = stateMachine.levelCheckpoints.LevelCheckpointList[GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex];

       
    }

    public override void Tick(float deltaTime)
    {
        countdown = Mathf.Max(countdown - deltaTime, 0f);
        if (countdown > 0)
        {
            Move(deltaTime);
        }
        else
        {
            
            stateMachine.SwitchState(new PlayerLandingState(stateMachine));
        }
    }

    public override void Exit()
    {
       
        stateMachine.transform.position = checkpointTransform.position;
        stateMachine.transform.rotation = checkpointTransform.rotation;


        stateMachine.FreeLookCamera.m_Follow = stateMachine.CameraFollowTransform;
        stateMachine.FreeLookCamera.m_XAxis.Value = 0f;
        stateMachine.FreeLookCamera.m_YAxis.Value = .6f;
        stateMachine.ResetToCheckPointCompleteEvent?.Raise();
    }
}