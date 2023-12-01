using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonBaseState : State
{
    protected ButtonStateMachine stateMachine;

    public ButtonBaseState(ButtonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
