using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDAddHealthState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Remove Health State");
    [SerializeField] private string currentState = "HUD Remove Health State";

    public HUDAddHealthState(HUDStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        //HUDTweener ShowHealth On Complete AddHealth HUD Object On Complete call SwitchToHUDHidden with a delay appropriate for seeing the collectable total on complete 

    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}
