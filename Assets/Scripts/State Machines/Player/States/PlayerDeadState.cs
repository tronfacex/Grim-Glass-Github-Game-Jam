using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private float fadeOutDelay = 3f;
    private bool fadeStarted; 
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.gameObject.SetActive(false);

        
    }

    public override void Tick(float deltaTime) 
    {
        fadeOutDelay = Mathf.Max(fadeOutDelay - deltaTime, 0f);
        if (fadeOutDelay == 0 && !fadeStarted)
        {
            fadeStarted = true;
            stateMachine.PlayerDiedFadeOutEvent?.Raise();
        }
    }
    public override void Exit() { }
}
