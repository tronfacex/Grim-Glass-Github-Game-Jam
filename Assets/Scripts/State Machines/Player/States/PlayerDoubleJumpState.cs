using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    private readonly int DoubleJumpHash = Animator.StringToHash("Double Jumping Up");

    private const float CrossFadeDuration = 0.15f;

    private float dodgeVelocityDampening = 0.2f;

    private float doubleJumpDuration = 0.15f;

    private Vector3 momentum;

    public PlayerDoubleJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Double Jump!");
        stateMachine.DoubleJumping = true;
        stateMachine.CanDoubleJump = false;
        stateMachine.ForceReceiver.ReturnGravityModifier();
        stateMachine.ForceReceiver.DoubleJump(stateMachine.DoubleJumpForce);
        stateMachine.Animator.CrossFadeInFixedTime(DoubleJumpHash, CrossFadeDuration);

        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.FloatEvent += OnFloat;

        momentum = stateMachine.CharController.velocity;
        momentum.y = 0f;

        if (stateMachine.DodgeToJumpStarted)
        {
            momentum = stateMachine.CharController.velocity * dodgeVelocityDampening;
            momentum.y = 0f;
        }
    }


    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        doubleJumpDuration = Mathf.Max(doubleJumpDuration - deltaTime, 0);

        if (doubleJumpDuration == 0)
        {
            stateMachine.DoubleJumping = false;
        }

        Move(movement * stateMachine.JumpMovementSpeed, deltaTime);

        if (stateMachine.CharController.velocity.y <= 0 && !stateMachine.IsGrounded && !stateMachine.DoubleJumping)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine, true, true));
            return;
        }

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.DodgeToJumpStarted = false;
        stateMachine.DoubleJumping = false;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.FloatEvent -= OnFloat;
    }

    private void OnAttack()
    {
        if (stateMachine.IsGrounded)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }
        else
        {
            stateMachine.ForceReceiver.SlamAttack();
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 5));
        }
    }
    private void OnFloat()
    {
        if (stateMachine.IsGrounded) { return; }
        if (stateMachine.FloatDuration == 0) { return; }
        stateMachine.SwitchState(new PlayerFloatingState(stateMachine, false));
    }

}
