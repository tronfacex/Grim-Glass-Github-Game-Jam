using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottleEnemyDeadState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("EnemyLocomotionBlendTree");

    private const float CrossFadeDuration = 0.1f;
    private float timer = .5f;
    public MilkBottleEnemyDeadState(MilkBottleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //GameDataReader.Instance.GameData.NumberOfBossesDefeated++;
        GameObject.Destroy(stateMachine.Target);
        stateMachine.PoofEffect.transform.position = stateMachine.transform.position;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.glassShatter, stateMachine.transform.position);
        stateMachine.PoofEffect.SetActive(true);
        stateMachine.EnemyWeapon.gameObject.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.SetFloat(SpeedHash, 0);
    }

    public override void Tick(float deltaTime)
    {
        if (timer > 0)
        {
            timer -= deltaTime;
        }
        if (timer <= 0)
        {
            GameObject.Destroy(stateMachine.Health.gameObject);
        }
    }

    public override void Exit() { }
}
