using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float normalizedTime;
    private float previousFrameTime;
    private int attackIndex;
    private float comboGraceTime;
    private bool comboSuccessful = false;
    private bool alreadyAppliedForce;
    private bool jumpAfterAttack = false;
    private PlayerAttackSO attack;
    private GameObject trailEffect;
    private bool whooshSoundPlayed = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.InputReader.AttackEvent += OnAttack;

        comboGraceTime = .1f;

        if (stateMachine.HasMetBobby)
        {
            ToggleBottleMeshes(stateMachine.BottleWeapon);
        }

        //Debug.Log("Attacking Entered");
        if (stateMachine.StepBackDodgeCountdownStarted == true)
        {
            stateMachine.Targeter.TargetClosest = false;
        }

        if (attack.AirAttack)
        {
            stateMachine.GroundSlamWeapon.SetAttack(attack.AttackDamage, attack.Knockback, attack.Knockdown);
        }
        else
        {
            stateMachine.Weapon.SetAttack(attack.AttackDamage, attack.Knockback, attack.Knockdown);
        }

        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationString, attack.TransitionDuration);

        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;
    }

    public override void Tick(float deltaTime)
    {
        if (attack.AirAttack)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * stateMachine.JumpMovementSpeed, deltaTime);
        }
        else
        {
            Move(deltaTime);
        }

        if (!attack.AirAttack)
        {
            FaceTarget();
        }

        normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (normalizedTime >= 1f)
        {
            comboGraceTime -= deltaTime;
        }

        if (comboSuccessful && normalizedTime >= 1f && !jumpAfterAttack)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
            return;
        }

        if (comboSuccessful && comboGraceTime <= 0 && !jumpAfterAttack)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
            return;
        }

        if (comboGraceTime <= 0 && !comboSuccessful && !jumpAfterAttack)
        {
            stateMachine.StowWeaponCountdownStarted = true;
            stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
            return;
        }
        if (comboGraceTime <= 0 && jumpAfterAttack)
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false));
            return;
        }

        if (normalizedTime >= attack.ForceTime)
        {
            TryApplyForce();
            
        }

        if (normalizedTime >= attack.TrailEffectTime * .5f && !whooshSoundPlayed)
        {
            whooshSoundPlayed = true;
            if (attack.TrailEffect == 3 || attack.TrailEffect == -1) { return; } 
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bobbyWhoosh, stateMachine.transform.position);
        }
        
        if (normalizedTime >= attack.TrailEffectTime)
        {
            if (attack.TrailEffect != -1)
            {
                stateMachine.TrailEffects[attack.TrailEffect].SetActive(true);
                
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.Targeter.TargetClosest = true;
        stateMachine.DodgeToAttackStarted = false;
        stateMachine.CanDoubleJump = true;
        if (attack.TrailEffect != -1)
        {
            stateMachine.TrailEffects[attack.TrailEffect].SetActive(false);
        }
    }

    private void OnAttack()
    {
        normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (attack.ComboStateIndex == -1) { return; }

        if (comboGraceTime <= 0f || comboSuccessful) { return; }

        comboSuccessful = true;

        previousFrameTime = normalizedTime;
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }

    private void OnJump()
    {
        jumpAfterAttack = true;
    }
    private void OnDodge()
    {
        stateMachine.SwitchState(new PlayerDodgeState(stateMachine));
    }

}
