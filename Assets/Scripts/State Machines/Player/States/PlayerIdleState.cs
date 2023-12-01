using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private readonly int FreeMovementSpeedHash = Animator.StringToHash("FreeMovementSpeed");
    private readonly int FreeMovementBlendTreeHash = Animator.StringToHash("FreeMovementBlendTree");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeMovementBlendTreeHash, CrossFadeDuration);
        
    }

    public override void Tick(float deltaTime) 
    {
        stateMachine.Animator.SetFloat(FreeMovementSpeedHash, 0, AnimatorDampTime, deltaTime);

        Move(deltaTime);
    }
    public override void Exit() 
    {
        stateMachine.CanDoubleJump = true;
        //stateMachine.HasMetBobby = true;
    }
}