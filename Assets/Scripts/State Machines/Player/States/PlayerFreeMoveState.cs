using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerFreeMoveState : PlayerBaseState
{
    private readonly int FreeMovementSpeedHash = Animator.StringToHash("FreeMovementSpeed");
    private readonly int FreeMovementBlendTreeHash = Animator.StringToHash("FreeMovementBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private float stowWeaponDelay = 2;
    private float stepBackComboGraceTime = .75f;

    //private EventInstance playerFootsteps;
    //private bool footStepsAudioPlaying;
    private float footstepSoundCooldown = 0.33f; // Adjust this value as needed
    private float lastFootstepTime;


    public PlayerFreeMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //AudioManager.instance.SetAmbienceParameter("wind_intensity", 1);
        //stateMachine.playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);

        //Debug.Log("Enter Move State");
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.SprintEvent += OnSprint;

        stateMachine.Animator.CrossFadeInFixedTime(FreeMovementBlendTreeHash, CrossFadeDuration);

        stateMachine.ForceReceiver.ReturnGravityModifier();

        stateMachine.FloatTimeRemaining = stateMachine.FloatDuration;

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
        //Toggles bottle meshes after a timer expires so that the player isn't constantly putting their weapon away between swings
        StowWeaponDelayCheck(deltaTime, stateMachine.StowWeaponDelaySpeed);

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
            Move(movement * stateMachine.StandardMovementSpeed, deltaTime);
        }
        else
        {
            if (stateMachine.ForceReceiver.gravityModifier != stateMachine.PlatformGravityModifier)
            {
                stateMachine.ForceReceiver.ChangeGravityModifier(stateMachine.PlatformGravityModifier);
            }
        }

        //Set animator based on movement
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeMovementSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeMovementSpeedHash, 1, AnimatorDampTime, deltaTime);
        UpdatePlayerFootstepSound();
        // Debug.Log("Footsteps played?");



        FaceMovementDirection(movement, deltaTime);

        //Check grounding to determine if player fell off something, check gravity in case player fell from a platform
        if (!stateMachine.IsGrounded && stateMachine.CharController.velocity.y < -0.1f)
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
                MoveWithPlatform(newMovementCalc * stateMachine.StandardMovementSpeed, deltaTime);
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exit Move State");
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.SprintEvent -= OnSprint;
        stateMachine.StowWeaponDelaySpeed = 1;
    }

    private void StowWeaponDelayCheck(float deltaTime, float delayCountdownSpeed)
    {
        if (stowWeaponDelay > 0 && stateMachine.StowWeaponCountdownStarted)
        {
            stowWeaponDelay -= deltaTime * delayCountdownSpeed;
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
        stateMachine.SwitchState(new PlayerDodgeState(stateMachine));
    }

    private void OnJump()
    {
        if (stateMachine.OnMovingPlatform)
        {
            stateMachine.ForceReceiver.ReturnGravityModifier();
        }
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
    }

    private void OnSprint()
    {
        stateMachine.SwitchState(new PlayerSprintState(stateMachine));
    }

    private void UpdatePlayerFootstepSound()
    {
        if (Time.time - lastFootstepTime < footstepSoundCooldown) { return; }

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootsteps, stateMachine.transform.position);
        lastFootstepTime = Time.time;
    }
}
