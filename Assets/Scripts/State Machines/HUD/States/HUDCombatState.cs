using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCombatState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Collectable State");
    [SerializeField] private string currentState = "HUD Collectable State";

    public HUDCombatState(HUDStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;
        

        //HUDTweener ShowCombat 

    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {
        //HUDTweener HideCombat
    }
}