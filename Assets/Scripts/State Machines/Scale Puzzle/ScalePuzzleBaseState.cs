using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScalePuzzleBaseState : State
{
    protected ScalePuzzleStateMachine stateMachine;

    public ScalePuzzleBaseState(ScalePuzzleStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
