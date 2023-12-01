using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallingHash = Animator.StringToHash("Falling Idle");

    private const float CrossFadeDuration = 0.75f;

    private Vector3 momentum;

    private bool wasSprinting;
    private bool canSlamAttack;

    public PlayerFallingState(PlayerStateMachine stateMachine, bool IsSprinting, bool CanSlamAttack) : base(stateMachine)
    {
        wasSprinting = IsSprinting;
        canSlamAttack = CanSlamAttack;
    }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.FloatEvent += OnFloat;

        stateMachine.Animator.CrossFadeInFixedTime(FallingHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {

        Vector3 movement = CalculateMovement();

        if (wasSprinting)
        {
            Move(movement * (stateMachine.JumpMovementSpeed * 1.5f), deltaTime);
        }
        else
        {
            Move(movement * stateMachine.JumpMovementSpeed, deltaTime);
        }


        FaceMovementDirection(movement, deltaTime);

        if (stateMachine.IsGrounded)
        {
            stateMachine.SwitchState(new PlayerLandingState(stateMachine));
        }

    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.FloatEvent -= OnFloat;
    }

    private void OnJump()
    {
        if (!stateMachine.CanDoubleJump) { return; }
        stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
    }

    private void OnAttack()
    {
        if (stateMachine.IsGrounded)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }
        else
        {
            if (canSlamAttack)
            {
                stateMachine.ForceReceiver.SlamAttack();
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 5));
            }
        }
    }
    private void OnFloat()
    {
        if (stateMachine.IsGrounded) { return; }
        if (stateMachine.FloatDuration == 0) { return; }
        stateMachine.SwitchState(new PlayerFloatingState(stateMachine, false));
    }
    private void OnJumpCutoff()
    {
        stateMachine.ForceReceiver.ChangeGravityModifier(7f);
    }
}
