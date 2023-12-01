using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HUDShowAllState : HUDBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("HUD Show All State");
    [SerializeField] private string currentState = "HUD Show All State";

    private float showDuration = 5f;
    private float animationTime;

    public HUDShowAllState(HUDStateMachine stateMachine, float AnimationTime) : base(stateMachine) 
    {
        animationTime = AnimationTime;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        /*int difficultySettings = SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth;

        if (difficultySettings == 1)
        {
            stateMachine.HealthParentObj1.SetActive(true);
            stateMachine.HealthParentObj = stateMachine.HealthParentObj1.GetComponent<RectTransform>();

        }
        if (difficultySettings == 2)
        {
            stateMachine.HealthParentObj2.SetActive(true);
            stateMachine.HealthParentObj = stateMachine.HealthParentObj2.GetComponent<RectTransform>();
        }
        if (difficultySettings == 3)
        {
            stateMachine.HealthParentObj3.SetActive(true);
            stateMachine.HealthParentObj = stateMachine.HealthParentObj3.GetComponent<RectTransform>();
        }
        if (difficultySettings == 4)
        {
            stateMachine.HealthParentObj4.SetActive(true);
            stateMachine.HealthParentObj = stateMachine.HealthParentObj4.GetComponent<RectTransform>();
        }
        if (difficultySettings == 5)
        {
            stateMachine.HealthParentObj5.SetActive(true);
            stateMachine.HealthParentObj = stateMachine.HealthParentObj5.GetComponent<RectTransform>();
        }

        GameObject[] healthIconArray = GameObject.FindGameObjectsWithTag("Health Icon");

        foreach (GameObject icon in healthIconArray)
        {
            Image iconImage = icon.GetComponent<Image>();
            stateMachine.HealthIcons.Add(iconImage);
        }*/

        if (!stateMachine.HUDPanel.gameObject.activeSelf)
        {
            stateMachine.HUDPanel.gameObject.SetActive(true);
        }

        //Probably just a custom method to Show All instead of an events
        stateMachine.HUDPanel.DOAnchorPos(stateMachine.HUDPanelShowingPos.anchoredPosition, animationTime);
        
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {
        stateMachine.HUDPanel.DOAnchorPos(stateMachine.HUDPanelHidingPos.anchoredPosition, .15f);
    }

    private void DisplayHealthIcons()
    {
        stateMachine.HealthParentObj.DOScale(Vector3.one, 1.25f).SetEase(Ease.Linear);
    }
}

