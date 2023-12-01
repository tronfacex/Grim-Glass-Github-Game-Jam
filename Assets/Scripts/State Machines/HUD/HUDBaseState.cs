using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HUDBaseState : State
{
    protected HUDStateMachine stateMachine;

    public HUDBaseState(HUDStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
}
