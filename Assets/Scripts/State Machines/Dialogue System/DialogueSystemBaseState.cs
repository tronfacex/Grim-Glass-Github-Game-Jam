using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueSystemBaseState : State
{
    protected DialogueSystemStateMachine stateMachine;

    public DialogueSystemBaseState(DialogueSystemStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
