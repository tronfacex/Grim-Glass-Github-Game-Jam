using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jumping Up");

    private const float CrossFadeDuration = 0.1f;

    private float dodgeVelocityDampening = 0.2f;

    private bool wasSprinting;

    private float jumpCutoffDelay = 0.3f;

    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine, bool IsSprinting) : base(stateMachine)
    {
        wasSprinting = IsSprinting;
    }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.FloatEvent += OnFloat;
        stateMachine.InputReader.JumpCutoffEvent += OnJumpCutoff;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

    }


    public override void Tick(float deltaTime)
    {
        //Move(momentum, deltaTime);
        Vector3 movement = CalculateMovement();

        //We only want the jump cut off delay to delay in the event of an immediate button release
        //To allow the jump force to be applied a few frames into the jump animation we apply a delay but otherwise we want no delay
        //This allows us to do that by reducing the delay to zero shortly after the jump starts
        if (jumpCutoffDelay > 0)
        {
            jumpCutoffDelay = Mathf.Max(jumpCutoffDelay - deltaTime, 0f);
        }

        if (wasSprinting)
        {
            Move(movement * (stateMachine.JumpMovementSpeed * 1.5f), deltaTime);
        }
        else
        {
            Move(movement * stateMachine.JumpMovementSpeed, deltaTime);
        }

        if (stateMachine.CharController.velocity.y <= 0 && !stateMachine.IsGrounded)
        {
            if (wasSprinting)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine, true, false));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine, false, false));
            }
            return;
        }

        FaceMovementDirection(movement, deltaTime);
        //FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.DodgeToJumpStarted = false;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.FloatEvent -= OnFloat;
        stateMachine.InputReader.JumpCutoffEvent -= OnJumpCutoff;
    }

    private void OnJump()
    {
        if (!stateMachine.CanDoubleJump) { return; }
        if (stateMachine.IsGrounded) { return; }
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
    private void OnJumpCutoff()
    {
        stateMachine.ForceReceiver.JumpCutoffGravity(jumpCutoffDelay, 6f);
        stateMachine.ForceReceiver.ChangeGravityModifier(4f);
    }
}
