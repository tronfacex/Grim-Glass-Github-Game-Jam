using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MilkBottleEnemyRetreatState : MilkBottleEnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("EnemyLocomotionBlendTree");

    private Path path;
    private Seeker seeker;

    public float nextWaypointDistance = .1f;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;

    private const float CrossFadeDuration = 0.1f;
    private const float AnimationDampening = 0.1f;
    private const float RetreatSpeedMultiplier = 5;

    [SerializeField] private GameObject retreatDetector;

    public MilkBottleEnemyRetreatState(MilkBottleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        seeker = stateMachine.Seeker;
        retreatDetector = stateMachine.RetreatDetector;
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.SetFloat(SpeedHash, 1);
        stateMachine.AIDestinationSetter.target = retreatDetector.transform;
        seeker.StartPath(stateMachine.transform.position, stateMachine.AIDestinationSetter.target.position, OnPathComplete);
        stateMachine.GetComponent<SimpleSmoothModifier>().enabled = true;
    }
    public override void Tick(float deltaTime)
    {
        if (path == null)
        {
            stateMachine.SwitchState(new MilkBottleEnemyChasingState(stateMachine, true, 0, 0));
            return;
        }

        UpdatePathAndMovement(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.AIDestinationSetter.target = stateMachine.Player.transform;
        stateMachine.GetComponent<SimpleSmoothModifier>().enabled = false;
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
    }
    private void UpdatePathAndMovement(float deltaTime)
    {
        //if (!locomotionReset) { return; }
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
                Move(dir * stateMachine.MovementSpeed * RetreatSpeedMultiplier, deltaTime);
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
                    stateMachine.SwitchState(new MilkBottleEnemyChasingState(stateMachine, true, 1.25f, 0));
                    //stateMachine.SwitchState(new EnemyAttackingState(stateMachine, 1));
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
}
