using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottleEnemyIdleState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int IdleBlendTreeHash = Animator.StringToHash("EnemyIdleBlendTree");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("EnemyLocomotionBlendTree");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimationDampening = 0.1f;

    private float timer;
    private bool timerElapsed;

    private bool blended;

    public MilkBottleEnemyIdleState(MilkBottleEnemyStateMachine stateMachine, float timerAmount) : base(stateMachine) 
    {
        timer = timerAmount;
    }

    public override void Enter()
    {
        stateMachine.transform.position = stateMachine.StartingPos.position;

        stateMachine.PlayerDetectionDistance = 0;

        stateMachine.Animator.CrossFadeInFixedTime(IdleBlendTreeHash, CrossFadeDuration);
        //stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        //stateMachine.Animator.SetFloat(SpeedHash, 0);
        //stateMachine.AIDestinationSetter.target = stateMachine.StartingPosition;

        /*if (IsInDetectionRange())
        {
            Debug.Log("In Range");
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }*/
    }
    public override void Tick(float deltaTime)
    {
        timer = Mathf.Max(timer - deltaTime, 0f);
        if (timer == 0 && !timerElapsed)
        {
            timerElapsed = true;
            stateMachine.PlayerDetectionDistance = 20f;
        }
        /*if (timer > 0)
        {
            timer -= deltaTime;
            blended = true;
        }
        if (timer <= 0 && blended)
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
            blended = false;
        }*/
        //Move(deltaTime);

        if (IsInDetectionRange())
        {
            //Debug.Log("In Range");
            stateMachine.SwitchState(new MilkBottleEnemyChasingState(stateMachine, true, 0f, 0f));
            return;
        }
        //stateMachine.Animator.SetFloat(SpeedHash, 0, AnimationDampening, deltaTime);
    }

    public override void Exit()
    {

    }
}
