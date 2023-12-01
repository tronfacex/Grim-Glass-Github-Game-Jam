using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBaseState : State
{
    protected GameStateMachine stateMachine;

    public GameBaseState(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
