using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MilkBottleEnemyAttackingState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("EnemyLocomotionBlendTree");

    private EnemyAttackSO attack;

    private float normalizedTime;
    private bool alreadyAppliedForce;
    private bool alreadyPlayedSound;

    private const float CrossFadeDuration = 0.1f;
    private const float AnimationDampening = 0.1f;

    public MilkBottleEnemyAttackingState(MilkBottleEnemyStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        //Debug.Log("ENEMY ATTACK STARTED");
        if (attack.RangedAttackPrefab == null)
        {
            stateMachine.EnemyWeapon.SetAttack(attack.AttackDamage, attack.Knockback, 0f);
        }
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationString, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (normalizedTime >= 1f)
        {
            stateMachine.SwitchState(new MilkBottleEnemyChasingState(stateMachine, false, attack.LocomotionCooldown, attack.AttackCooldown));
        }

        if (normalizedTime <= attack.ForceTime - .2f)
        {
            if (attack.TrackTarget)
            {
                FaceTarget();
            }
            if (!alreadyPlayedSound)
            {
                alreadyPlayedSound = true;
                if (attack.AttackSound == 1)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.meleeAttackSound, stateMachine.transform.position);
                }
                if (attack.AttackSound == 2)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.rangedAttackSound, stateMachine.transform.position);
                }
                if (attack.AttackSound == 3)
                {
                    
                    stateMachine.vomitSoundLoop = AudioManager.instance.CreateEventInstance(FMODEvents.instance.vomitLoopSound);
                    FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(stateMachine.transform.position);
                    stateMachine.vomitSoundLoop.set3DAttributes(attributes);
                    stateMachine.vomitSoundLoop.start();
                }
            }
        }

        if (normalizedTime >= attack.ForceTime)
        {
            TryApplyForce();
        }
    }

    public override void Exit()
    {
        stateMachine.vomitSoundLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        stateMachine.vomitSoundLoop.release();
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }
}
