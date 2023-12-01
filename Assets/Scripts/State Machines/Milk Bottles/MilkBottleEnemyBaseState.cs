using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MilkBottleEnemyBaseState : State
{
    protected MilkBottleEnemyStateMachine stateMachine;

    public MilkBottleEnemyBaseState(MilkBottleEnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharController.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;

        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
    protected bool IsInDetectionRange()
    {
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.PlayerDetectionDistance * stateMachine.PlayerDetectionDistance;
    }

    protected float DistanceToPlayer()
    {
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr;
    }

    protected float GetRandomProbability()
    {
        return Random.Range(0f, 1f);
    }
}
