using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingState : PlayerBaseState
{
    private readonly int FloatingHash = Animator.StringToHash("Floating");

    private const float CrossFadeDuration = 0.1f;

    private Vector3 momentum;

    private bool canSlamAttack;

    public PlayerFloatingState(PlayerStateMachine stateMachine, bool CanSlamAttack) : base(stateMachine)
    {
        canSlamAttack = CanSlamAttack;
    }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.FloatCancelledEvent += OnFloatCancelled;

        stateMachine.ForceReceiver.FloatGravity();

        stateMachine.Animator.CrossFadeInFixedTime(FloatingHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        //Move(momentum, deltaTime);

        if (stateMachine.FloatTimeRemaining > 0)
        {
            stateMachine.FloatTimeRemaining = Mathf.Max(stateMachine.FloatTimeRemaining - deltaTime, 0);
        }

        if (stateMachine.FloatTimeRemaining == 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine, false, canSlamAttack));
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.JumpMovementSpeed, deltaTime);

        FaceMovementDirection(movement, deltaTime);

        if (stateMachine.IsGrounded)
        {
            stateMachine.SwitchState(new PlayerLandingState(stateMachine));
        }

        //FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.FloatCancelledEvent -= OnFloatCancelled;
        stateMachine.ForceReceiver.ReturnGravityModifier();
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

    private void OnFloatCancelled()
    {
        stateMachine.SwitchState(new PlayerFallingState(stateMachine, false, canSlamAttack));
    }
}
