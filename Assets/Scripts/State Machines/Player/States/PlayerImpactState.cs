using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactAnimationHash = Animator.StringToHash("Impact");
    private readonly int ImpactRecoveryAnimationHash = Animator.StringToHash("Impact Recovery");

    private const float CrossFadeDuration = 0.1f;

    private float impactDuration = .5f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Enter Impact");

        stateMachine.Health.IsInvincible = true;

        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash, CrossFadeDuration);

        stateMachine.playerImpact = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerImpacts);
        PLAYBACK_STATE playbackState;
        stateMachine.playerImpact.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(stateMachine.transform.position);
            stateMachine.playerImpact.set3DAttributes(attributes);
            stateMachine.playerImpact.start();
        }
        stateMachine.playerImpactFeedback.PlayFeedbacks();
    }
    public override void Tick(float deltaTime)
    {
        FaceTarget();
        Move(deltaTime);

        float normalizedTime = GetNormalizedTimeImpact(stateMachine.Animator, "Impact");

        if (normalizedTime >= 1f)
        {
            if (impactDuration > 0)
            {
                impactDuration -= deltaTime;
                //stateMachine.Animator.CrossFadeInFixedTime(ImpactRecoveryAnimationHash, CrossFadeDuration);
            }
            if (impactDuration <= 0)
            {
                stateMachine.Animator.CrossFadeInFixedTime(ImpactRecoveryAnimationHash, CrossFadeDuration);
                float newNormalizedTime = GetNormalizedTimeImpact(stateMachine.Animator, "Impact");
                if (newNormalizedTime >= .8f)
                {
                    stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
                }
            }
        }
    }
    public override void Exit()
    {
        stateMachine.Health.IsInvincible = false;
        stateMachine.playerImpact.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        stateMachine.playerImpact.release();
    }
}
