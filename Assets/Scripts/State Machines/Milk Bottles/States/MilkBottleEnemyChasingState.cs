using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MilkBottleEnemyChasingState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int IdleBlendTreeHash = Animator.StringToHash("EnemyIdleBlendTree");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("EnemyLocomotionBlendTree");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimationDampening = 0.1f;

    private bool blended;
    private Path path;
    private Seeker seeker;
    private Vector3 prevPosition;

    public float nextWaypointDistance = .1f;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    private float attackCooldown;
    private float locomotionCooldown;
    private bool attackReset;
    private bool locomotionReset;

    private float retreatProbability = 0.2f;
    const float meleeProbability = 0.75f;  // 60% chance to kick
    const float spinProbability = 0.40f; // 40% chance to spin attack
    const float rangedProbability = 0.0025f; // 40% chance to range attack

    public MilkBottleEnemyChasingState(MilkBottleEnemyStateMachine stateMachine, bool canAttack, float LocomotionCooldown, float AttackCooldown) : base(stateMachine)
    {
        seeker = stateMachine.Seeker;
        attackReset = canAttack;
        locomotionReset = canAttack;
        locomotionCooldown = LocomotionCooldown;
        attackCooldown = AttackCooldown;
    }

    public override void Enter()
    {
        //attackCooldown = stateMachine.AttackCooldown;
        stateMachine.Knockdown.knockDownAmount = 0f;
        //stateMachine.Animator.CrossFadeInFixedTime(IdleBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.SetFloat(SpeedHash, 0);
        //stateMachine.AIDestinationSetter.enabled = true;
        seeker.StartPath(stateMachine.transform.position, stateMachine.AIDestinationSetter.target.position, OnPathComplete);
        prevPosition = stateMachine.Player.transform.position;
    }
    public override void Tick(float deltaTime)
    {
        UpdateAttackCooldown(deltaTime);
        if (!CheckDetectionRangeAndSwitchStateIfOutOfRange())
        {
            return;
        }
        RecalculatePathIfNeeded();
        UpdatePathAndMovement(deltaTime);
        AttemptRangedAttack();
        AttemptMidRangeAttack();
        AttemptMeleeAttack();
    }

    public override void Exit()
    {

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }
    private bool ShouldRecalculatePath()
    {
        Vector3 currentPosition = stateMachine.Player.transform.position;

        if ((prevPosition - currentPosition).sqrMagnitude > 0.001f)
        {
            //Debug.Log("Positions are Different");
            return true;
        }
        else
        {
            //Debug.Log("Positions are Same");
            return false;
        }
    }
    private void UpdateAttackCooldown(float deltaTime)
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= deltaTime;
        }
        if (attackCooldown <= 0)
        {
            attackReset = true;
        }
        if (locomotionCooldown > 0)
        {
            locomotionCooldown -= deltaTime;
        }
        if (locomotionCooldown <= 0)
        {
            locomotionReset = true;
        }
    }

    private bool CheckDetectionRangeAndSwitchStateIfOutOfRange()
    {
        if (!IsInDetectionRange())
        {
            //Debug.Log("Out of Range");
            stateMachine.SwitchState(new MilkBottleEnemyIdleState(stateMachine, 0f));
            return false;
        }
        return true;
    }

    private void RecalculatePathIfNeeded()
    {
        seeker.StartPath(stateMachine.transform.position, stateMachine.AIDestinationSetter.target.position, OnPathComplete);
        prevPosition = stateMachine.Player.transform.position;
    }

    private void UpdatePathAndMovement(float deltaTime)
    {
        if (!locomotionReset) { return; }
        reachedEndOfPath = false;

        float distanceToWaypoint = CheckWaypointDistance();

        if (path != null && currentWaypoint < path.vectorPath.Count)
        {
            Vector3 dir = (path.vectorPath[currentWaypoint] - stateMachine.transform.position).normalized;
            if (stateMachine.InKnockback)
            {
                Move(deltaTime);
            }
            else
            {
                Move(dir * stateMachine.MovementSpeed, deltaTime);
                stateMachine.Animator.SetFloat(SpeedHash, 1, AnimationDampening, deltaTime);
            }
        }
        else
        {
            Move(deltaTime);
        }

        FaceTarget();
    }

    private float CheckWaypointDistance()
    {
        float distanceToWaypoint = 0;
        while (true)
        {
            if (path == null || currentWaypoint >= path.vectorPath.Count)
            {
                return distanceToWaypoint;
            }
            distanceToWaypoint = Vector3.Distance(stateMachine.transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        return distanceToWaypoint;
    }

    private void AttemptRetreat()
    {
        if (stateMachine.Knockdown.knockDownAmount < stateMachine.Knockdown.knockDownThreshold * .5f)
        {
            retreatProbability = 0.2f;
        }
        if (stateMachine.Knockdown.knockDownAmount > stateMachine.Knockdown.knockDownThreshold * .5f)
        {
            retreatProbability = 0.66f;
        }
        float probability = GetRandomProbability();
        if (probability <= retreatProbability)
        {
            stateMachine.SwitchState(new MilkBottleEnemyRetreatState(stateMachine));
            return;
        }
    }

    private void AttemptRangedAttack()
    {
        if (DistanceToPlayer() > stateMachine.AttackRange3 && DistanceToPlayer() < stateMachine.AttackRange4 && attackReset)
        {
            float probability = GetRandomProbability();
            if (probability <= rangedProbability)
            {
                //stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                //Debug.Log("THROW ATTACK WOULD HAPPEN");
                stateMachine.SwitchState(new MilkBottleEnemyAttackingState(stateMachine, 1));
            }
        }
    }

    private void AttemptMidRangeAttack()
    {
        if (DistanceToPlayer() > stateMachine.AttackRange2 && DistanceToPlayer() < stateMachine.AttackRange3 && attackReset)
        {
            float probability = GetRandomProbability();
            if (probability <= meleeProbability)
            {
                //stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                //Debug.Log("Kick ATTACK WOULD HAPPEN");
                stateMachine.SwitchState(new MilkBottleEnemyAttackingState(stateMachine, 0));
            }
        }
    }

    private void AttemptMeleeAttack()
    {
        if (DistanceToPlayer() < stateMachine.AttackRange1 && attackReset)
        {
            //AttemptRetreat();
            if (stateMachine.Knockdown.knockDownAmount < stateMachine.Knockdown.knockDownThreshold * .33f)
            {
                retreatProbability = 0.5f;
                //retreatProbability = 1f;
            }
            if (stateMachine.Knockdown.knockDownAmount > stateMachine.Knockdown.knockDownThreshold * .33f)
            {
                retreatProbability = 0.75f;
            }
            float probability = GetRandomProbability();
            if (probability <= retreatProbability)
            {
                stateMachine.SwitchState(new MilkBottleEnemyRetreatState(stateMachine));
                return;
            }

            stateMachine.SwitchState(new MilkBottleEnemyAttackingState(stateMachine, 2));

            /*probability = GetRandomProbability();
            if (probability <= meleeProbability)
            {
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 0));
            }
            else if (probability <= meleeProbability + spinProbability)
            {
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 2));
            }*/
        }
    }
}
