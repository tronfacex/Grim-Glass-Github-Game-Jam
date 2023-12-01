using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDRemoveHealthState : HUDBaseState
{
    private bool PlatformFailureDetected;
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Remove Health State");
    [SerializeField] private string currentState = "HUD Remove Health State";

    public HUDRemoveHealthState(HUDStateMachine stateMachine, bool platformFailureDetected) : base(stateMachine) 
    {
        PlatformFailureDetected = platformFailureDetected;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        if (PlatformFailureDetected)
        {
            //HUDTweener ShowHealth On Complete RemoveHealth HUD Object On Complete call SwitchToHUDHIDDEN with a delay appropriate for seeing the health remove
            stateMachine.SwitchState(new HUDFadeToBlackState(stateMachine, 1.5f));
            //stateMachine.
        }

        //HUDTweener ShowHealth On Complete RemoveHealth HUD Object On Complete call SwitchToHUDHIDDEN with a delay appropriate for seeing the collectable total on complete 
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {

    }
}
