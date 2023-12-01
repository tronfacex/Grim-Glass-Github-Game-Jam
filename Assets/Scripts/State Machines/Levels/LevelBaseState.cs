using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBaseState : State
{
    protected LevelStateMachine stateMachine;

    public LevelBaseState(LevelStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        //Move(Vector3.zero, deltaTime);
    }
}
