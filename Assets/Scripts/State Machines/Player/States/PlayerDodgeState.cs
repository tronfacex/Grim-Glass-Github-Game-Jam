using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private readonly int DodgingBlendTreeHash = Animator.StringToHash("DodgingBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

    private bool dodgeAgain = false;
    private bool attackAfterDodge = false;
    private bool jumpAfterDodge = false;

    private const float CrossFadeDuration = 0.1f;

    private Vector2 dodgeDirectionInput;
    private float remainingDashTime;
    public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Enter Dashing");
        stateMachine.StepBackDodgeCountdownStarted = false;
        stateMachine.DodgeToAttackStarted = false;

        dodgeDirectionInput = stateMachine.InputReader.MovementValue;

        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, CrossFadeDuration);


        if (dodgeDirectionInput.x == 0 && dodgeDirectionInput.y == 0)
        {
            stateMachine.StepBackDodgeCountdownStarted = true;
            stateMachine.Targeter.TargetClosest = false;
            remainingDashTime = stateMachine.StepBackDodgeTime;
        }
        else
        {
            remainingDashTime = stateMachine.DodgeTime;
            stateMachine.DodgeToAttackStarted = true;

        }
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.AttackEvent += OnAttack;
    }
    public override void Tick(float deltaTime)
    {
        remainingDashTime = Mathf.Max(remainingDashTime - deltaTime, 0f);

        AnimatorStateInfo animtorStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        if (dodgeDirectionInput != Vector2.zero && animtorStateInfo.normalizedTime > 0.6f)
        {
            Vector3 movementDirection = CalculateMovement();
            movementDirection.y = 0f;
            if (movementDirection != Vector3.zero)
            {
                FaceMovementDirection(movementDirection, Time.deltaTime * 10);
            }
        }

        //Debug.Log(remainingDashTime);
        if (remainingDashTime > 0)
        {
            UpdateAnimator(Time.deltaTime);
            if (animtorStateInfo.normalizedTime < .8f)
            {
                Move(CalculateMovement(deltaTime) * stateMachine.StandardMovementSpeed, deltaTime);
            }
        }
        else
        {
            Debug.Log("Dodge Ended");
            if (attackAfterDodge && stateMachine.StepBackDodgeCountdownStarted)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 2));
                return;
            }
            if (attackAfterDodge)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 4));
                return;
            }
            if (jumpAfterDodge)
            {
                stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
                return;
            }
            if (dodgeAgain)
            {
                FaceMovementDirection(stateMachine.InputReader.MovementValue, deltaTime);
                stateMachine.SwitchState(new PlayerDodgeState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
            }
        }
    }
    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //This is the part where I try to account for the step back dodge
        if (dodgeDirectionInput.x == 0 && dodgeDirectionInput.y == 0)
        {
            Vector3 playerForward = -stateMachine.transform.forward;
            Vector3 playerRight = stateMachine.transform.right;
            movement += playerForward * stateMachine.StepBackDodgeLength / stateMachine.StepBackDodgeTime;
            //movement += playerRight * stateMachine.DodgeLength / stateMachine.DodgeTime;
        }
        else
        {
            movement += right * dodgeDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeTime;
            movement += forward * dodgeDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeTime;
        }

        //remainingDashTime = Mathf.Max(remainingDashTime - deltaTime, 0f);

        movement.y = stateMachine.ForceReceiver.Movement.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        //stateMachine.Animator.SetFloat(DashForward, 0, animatorDampTime, deltaTime);
        if (dodgeDirectionInput.y == 0)
        {
            stateMachine.Animator.SetFloat(DodgeForwardHash, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = dodgeDirectionInput.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(DodgeForwardHash, value, 0.01f, deltaTime);
        }

        if (dodgeDirectionInput.x == 0)
        {
            stateMachine.Animator.SetFloat(DodgeRightHash, 0, 0.01f, deltaTime);
        }
        else
        {
            float value = dodgeDirectionInput.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(DodgeRightHash, value, 0.01f, deltaTime);
        }
    }

    //Both FaceMovementDirection & CalculateMovement need to move to player base state!!!
    /*private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }*/

    private void OnJump()
    {
        //stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        jumpAfterDodge = true;
        stateMachine.DodgeToJumpStarted = true;
    }
    private void OnDodge()
    {
        dodgeAgain = true;
    }
    private void OnAttack()
    {
        attackAfterDodge = true;
    }
}
