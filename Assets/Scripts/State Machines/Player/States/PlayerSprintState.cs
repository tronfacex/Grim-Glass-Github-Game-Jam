using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private readonly int SprintMovementSpeedHash = Animator.StringToHash("SprintMovementSpeed");
    private readonly int SprintMovementBlendTreeHash = Animator.StringToHash("SprintMovementBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private float stowWeaponDelay = 2;
    private float stepBackComboGraceTime = .75f;

    private Vector3 previousPosition;
    private float speedThresholdGraceTime = .33f;


    private float footstepSoundCooldown = 0.2f;
    private float lastFootstepTime;

    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Enter Move State");
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(SprintMovementBlendTreeHash, CrossFadeDuration);

        if (!stateMachine.HasMetBobby)
        {
            stateMachine.BottleBackpack.SetActive(false);
            stateMachine.BottleWeapon.SetActive(false);
            return;
        }
        if (stateMachine.StowWeaponCountdownStarted) { return; }
        ToggleBottleMeshes(stateMachine.BottleBackpack);
    }
    public override void Tick(float deltaTime)
    {
        // Calculate the velocity
        Vector3 currentPosition = stateMachine.transform.position;
        Vector3 velocity = (currentPosition - previousPosition) / deltaTime;
        previousPosition = currentPosition;

        // Check if the magnitude of the velocity is below the threshold
        if (velocity.magnitude < stateMachine.SprintSpeedThreshold && speedThresholdGraceTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
            return;
        }

        //Toggles bottle meshes after a timer expires so that the player isn't constantly putting their weapon away between swings
        StowWeaponDelayCheck(deltaTime);

        //Handles stepback dodge information
        if (stateMachine.StepBackDodgeCountdownStarted && stepBackComboGraceTime > 0)
        {
            stepBackComboGraceTime -= deltaTime;
        }
        if (stepBackComboGraceTime <= 0)
        {
            stateMachine.StepBackDodgeCountdownStarted = false;
            stateMachine.Targeter.TargetClosest = true;
        }

        Vector3 movement = CalculateMovement();

        //If player is not on moving platform make sure the default gravity is on and use standard move, else switch to platform gravity
        if (!stateMachine.OnMovingPlatform)
        {
            if (stateMachine.ForceReceiver.gravityModifier == stateMachine.PlatformGravityModifier)
            {
                stateMachine.ForceReceiver.ReturnGravityModifier();
            }
            Move(movement * stateMachine.SprintMovementSpeed, deltaTime);
        }
        else
        {
            if (stateMachine.ForceReceiver.gravityModifier != stateMachine.PlatformGravityModifier)
            {
                stateMachine.ForceReceiver.ChangeGravityModifier(stateMachine.PlatformGravityModifier);
            }
        }

        //Set animator according movement input
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(SprintMovementSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(SprintMovementSpeedHash, 1, AnimatorDampTime, deltaTime);
        UpdatePlayerFootstepSound();

        FaceMovementDirection(movement, deltaTime);

        //If player speed falls below speed threshol and the gracetime (time for the player to build speed up above the threshold) has expired
        if (speedThresholdGraceTime > 0)
        {
            speedThresholdGraceTime -= deltaTime;
        }

        //Check if the player fell of something and return gravity in case it was a platform
        if (!stateMachine.IsGrounded && stateMachine.CharController.velocity.y < 0)
        {
            stateMachine.ForceReceiver.ReturnGravityModifier();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine, false, true));
        }

        //Secondary check for player inputs because I was getting better player movement recalculating at the end of the update
        if (stateMachine.OnMovingPlatform)
        {
            Vector3 newMovementCalc = CalculateMovement();
            if (newMovementCalc == Vector3.zero)
            {
                MoveWithPlatform(deltaTime);
            }
            else
            {
                MoveWithPlatform(newMovementCalc * stateMachine.SprintMovementSpeed, deltaTime);
            }

        }
    }

    public override void Exit()
    {
        //Debug.Log("Exit Move State");
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void StowWeaponDelayCheck(float deltaTime)
    {
        if (stowWeaponDelay > 0 && stateMachine.StowWeaponCountdownStarted)
        {
            stowWeaponDelay -= deltaTime;
        }
        if (stowWeaponDelay <= 0 && stateMachine.HasMetBobby)
        {
            ToggleBottleMeshes(stateMachine.BottleBackpack);
            stateMachine.StowWeaponCountdownStarted = false;
        }
    }

    private void OnAttack()
    {
        if (stateMachine.StepBackDodgeCountdownStarted)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 2));
        }
        else
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        }
    }

    private void OnDodge()
    {
        if (stateMachine.ForceReceiver.gravityModifier == stateMachine.PlatformGravityModifier)
        {
            stateMachine.ForceReceiver.ReturnGravityModifier();
        }
        stateMachine.SwitchState(new PlayerDodgeState(stateMachine));
    }

    private void OnJump()
    {
        if (stateMachine.ForceReceiver.gravityModifier == stateMachine.PlatformGravityModifier)
        {
            stateMachine.ForceReceiver.ReturnGravityModifier();
        }
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine, true));
    }

    private void UpdatePlayerFootstepSound()
    {
        if (Time.time - lastFootstepTime < footstepSoundCooldown) { return; }

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootsteps, stateMachine.transform.position);
        lastFootstepTime = Time.time;
    }
}
