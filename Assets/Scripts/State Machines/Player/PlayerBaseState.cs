using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
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

    protected void MoveWithPlatform(float deltaTime)
    {
        
        if (stateMachine.OnMovingPlatform && stateMachine.ActivePlatform != null)
        {
            stateMachine.ForceReceiver.ApplyPlatformVerticalVelocity(stateMachine.ActivePlatformVelocity.y * 2);
            Vector3 totalMotion = stateMachine.ActivePlatformVelocity + stateMachine.ForceReceiver.Movement;
            stateMachine.CharController.Move(totalMotion * deltaTime);
        }
    }

    protected void MoveWithPlatform(Vector3 motion, float deltaTime)
    {
        Vector3 totalMotion = motion + stateMachine.ForceReceiver.Movement;

        if (stateMachine.OnMovingPlatform && stateMachine.ActivePlatform != null)
        {
            totalMotion += stateMachine.ActivePlatformVelocity;
        }

        stateMachine.CharController.Move(totalMotion * deltaTime);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RoationDamping);
    }

    /*protected void Move(Vector3 motion, float deltaTime, float attackMoveDistance)
    {
        Vector3 forwardStep = stateMachine.transform.forward * attackMoveDistance * deltaTime;
        stateMachine.CharController.Move((forwardStep + stateMachine.ForceReceiver.Movement) * deltaTime);
    }*/
    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;

        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ToggleBottleMeshes(GameObject itemToToggleOn)
    {
        if (!itemToToggleOn.activeSelf)
        {
            itemToToggleOn.SetActive(true);
        }

        GameObject itemToToggleOff = itemToToggleOn.CompareTag("Backpack")
                                ? stateMachine.BottleWeapon
                                : stateMachine.BottleBackpack;

        itemToToggleOff.SetActive(false);
    }
    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new PlayerFreeMoveState(stateMachine));
    }
}
