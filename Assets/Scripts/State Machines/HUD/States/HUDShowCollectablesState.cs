using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDShowCollectablesState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Collectable State");
    [SerializeField] private string currentState = "HUD Collectable State";

    public HUDShowCollectablesState(HUDStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;
        

        //HUDTweener ShowCollectables call SwitchToHudhidden with a delay appropriate for seeing the collectable total on complete 

    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}
