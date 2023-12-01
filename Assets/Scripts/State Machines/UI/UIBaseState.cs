using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseState : State
{
    protected UIStateMachine stateMachine;

    public UIBaseState(UIStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
