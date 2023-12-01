using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private readonly int LandingHash = Animator.StringToHash("Landing");

    private const float CrossFadeDuration = 0.1f;

    private float normalizedTime;

    public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LandingHash, CrossFadeDuration);
        stateMachine.ForceReceiver.ReturnGravityModifier();

        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        normalizedTime = GetNormalizedTimeLanding();

        if (normalizedTime >= .8f)
        {
            stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
            return;
        }

    }

    public override void Exit()
    {
        stateMachine.CanDoubleJump = true;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private float GetNormalizedTimeLanding()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Landing"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Landing"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    private void OnJump()
    {
        if (stateMachine.OnMovingPlatform)
        {
            stateMachine.ForceReceiver.ReturnGravityModifier();
        }
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
    }
}
