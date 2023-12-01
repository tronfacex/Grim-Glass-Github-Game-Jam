using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingPlatformBaseState : State
{
    protected MovingPlatformStateMachine stateMachine;

    public MovingPlatformBaseState(MovingPlatformStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        //Move(Vector3.zero, deltaTime);
    }
}
