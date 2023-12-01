using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIHiddenState : UIBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("UI Hidden State");
    [SerializeField] private string currentState = "UI Hidden State";

    public UIHiddenState(UIStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;

        if (stateMachine.playerStateMachine == null)
        {
            stateMachine.playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        }

        stateMachine.playerStateMachine.InputReader.PauseEvent += OnPause;

        stateMachine.MenuPanelObject.SetActive(false);
        
    }

    public override void Tick(float deltaTime)
    {



    }

    public override void Exit()
    {
        stateMachine.MenuPanelObject.SetActive(true);
        stateMachine.playerStateMachine.InputReader.PauseEvent -= OnPause;
    }

    private void OnPause()
    {
        if (stateMachine.playerStateMachine.PlayerInCutscene) { return; }
        if (stateMachine.PlatformFailureDetected) { return;  }
        stateMachine.TitlePanelText.text = "Pause Menu";
        stateMachine.ResumeButton.gameObject.SetActive(true);
        stateMachine.RestartFromLastCheckpointButton.gameObject.SetActive(false);
        stateMachine.FirstButtonSelected = stateMachine.ResumeButton.gameObject;

        // Create a new navigation instance for exit button
        Navigation newExitButtonNav = new Navigation();
        newExitButtonNav.mode = Navigation.Mode.Explicit; // Set the navigation mode
        newExitButtonNav.selectOnLeft = stateMachine.SkipToNextCheckpointButton;
        newExitButtonNav.selectOnRight = stateMachine.ResumeButton;
        newExitButtonNav.selectOnDown = stateMachine.XAxisSensitivitySlider;
        newExitButtonNav.selectOnUp = stateMachine.AmbienceSlider;


        stateMachine.ExitButton.navigation = newExitButtonNav;

        // Create a new navigation instance for Skip button
        Navigation newSkipButtonNav = new Navigation();
        newSkipButtonNav.mode = Navigation.Mode.Explicit; // Set the navigation mode
        newSkipButtonNav.selectOnLeft = stateMachine.ResumeButton;
        newSkipButtonNav.selectOnRight = stateMachine.ExitButton;
        newSkipButtonNav.selectOnDown = stateMachine.XAxisSensitivitySlider;
        newSkipButtonNav.selectOnUp = stateMachine.AmbienceSlider;

        Navigation newAmbientSliderNav = new Navigation();
        newAmbientSliderNav.mode = Navigation.Mode.Explicit;
        newAmbientSliderNav.selectOnDown = stateMachine.ResumeButton;
        newAmbientSliderNav.selectOnUp = stateMachine.SFXSlider;
        newAmbientSliderNav.selectOnLeft = null;
        newAmbientSliderNav.selectOnRight = null;

        stateMachine.AmbienceSlider.navigation = newAmbientSliderNav;

        Navigation newXAxisSliderNav = new Navigation();
        newXAxisSliderNav.mode = Navigation.Mode.Explicit;
        newXAxisSliderNav.selectOnDown = stateMachine.YAxisSensitivitySlider;
        newXAxisSliderNav.selectOnUp = stateMachine.ResumeButton;
        newXAxisSliderNav.selectOnLeft = null;
        newXAxisSliderNav.selectOnRight = null;

        stateMachine.XAxisSensitivitySlider.navigation = newXAxisSliderNav;

        stateMachine.SwitchState(new UIPauseMenuState(stateMachine));
    }
}
