using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUDHideAllState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Return From Black State");
    [SerializeField] private string currentState = "HUD Return From Black State";

    private float DelayTime;
    private bool HUDHidden;

    public HUDHideAllState(HUDStateMachine stateMachine, float delayTime) : base(stateMachine)
    {
        DelayTime = delayTime;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;


        //Probably just a custom method to tween alpha of black panel off

    }

    public override void Tick(float deltaTime)
    {
        DelayTime = Mathf.Max(DelayTime - deltaTime, 0f);
        if (DelayTime == 0 && !HUDHidden)
        {
            HUDHidden = true;
            stateMachine.HUDPanel.DOAnchorPos(stateMachine.HUDPanelHidingPos.anchoredPosition, .8f).OnComplete(() =>
            {
                stateMachine.SwitchState(new HUDHiddenState(stateMachine, 0));
                stateMachine.HUDPanelHideAllComplete?.Raise();
            });

        }


    }

    public override void Exit()
    {
        //stateMachine.HealthParentObj.localScale = new Vector3(0, 1, 1);
        stateMachine.HUDPanel.gameObject.SetActive(false);

    }
}
