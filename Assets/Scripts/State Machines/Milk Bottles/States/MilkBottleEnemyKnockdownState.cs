using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottleEnemyKnockdownState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int IdleBlendTreeHash = Animator.StringToHash("EnemyIdleBlendTree");
    private readonly int KnockdownBlendTreeHash = Animator.StringToHash("EnemyKnockdownBlendTree");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimationDampening = 0.1f;

    private bool blended;

    public MilkBottleEnemyKnockdownState(MilkBottleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.Health.health == 0)
        {
            stateMachine.SwitchState(new MilkBottleEnemyDeadState(stateMachine));
            return;
        }
        //stateMachine.Billboard.enabled = false;
        //stateMachine.SpriteDirectionalController.enabled = false;
        stateMachine.BarkSpew.SetActive(false);
        stateMachine.MeleeWeaponGameObject.SetActive(false);

        stateMachine.Animator.CrossFadeInFixedTime(KnockdownBlendTreeHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalizedTime = GetNormalizedTimeImpact(stateMachine.Animator, "Impact");

        if (normalizedTime >= 1)
        {
            stateMachine.SwitchState(new MilkBottleEnemyChasingState(stateMachine, true, 0, 0));
        }
        stateMachine.Knockdown.knockDownAmount = 0;
    }

    public override void Exit()
    {
        stateMachine.Knockdown.knockDownAmount = 0;
        stateMachine.Billboard.enabled = true;
        stateMachine.SpriteDirectionalController.enabled = true;
    }
}
