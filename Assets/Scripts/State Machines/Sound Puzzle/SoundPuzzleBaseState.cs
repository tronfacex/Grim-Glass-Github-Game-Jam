using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundPuzzleBaseState : State
{
    protected SoundPuzzleStateMachine stateMachine;

    public SoundPuzzleBaseState(SoundPuzzleStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
