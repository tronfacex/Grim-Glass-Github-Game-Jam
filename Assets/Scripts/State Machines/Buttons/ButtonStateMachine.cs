using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonStateMachine : StateMachine
{
    [field: SerializeField] public ButtonTrigger ButtonTrigger { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Transform ButtonTransform { get; private set; }

    [field: SerializeField] public int CurrentState;

    [field: SerializeField] private int idleStateHash = Animator.StringToHash("Idle State");

    [field: SerializeField] private int pressedStateHash = Animator.StringToHash("Pressed State");

    [field: SerializeField] public bool SuccessfullyPressed;

    [field: SerializeField] public bool ResetOnExit { get; private set; }

    [SerializeField] public GameEventScriptableObject ButtonTiggerEvent;

    [SerializeField] public GameEventScriptableObject ButtonTiggerExitEvent;

    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;

        //SwitchState(new MovingPlatformIdleState(this, false));
    }
    private void OnEnable()
    {
        if (Health != null)
        {
            Health.OnDie += HandleDie;
        }
    }
    private void OnDisable()
    {
        if (Health != null)
        {
            Health.OnDie -= HandleDie;
        }
    }

    public void HandleDie()
    {
        //ButtonTransform.DOScaleY(.45f, 0.2f).OnComplete(ReturnButtonSize);
        ButtonTransform.DOScaleY(.45f, 0.2f);
        SwitchState(new ButtonPressedState(this));
    }
    public void ReturnButtonSize()
    {
        StartCoroutine(DelayReturnButtonSize());
    }
    private IEnumerator DelayReturnButtonSize()
    {
        yield return new WaitForSeconds(0.1f);
        ButtonTransform.DOScaleY(1, 0.2f);
    }

    public void SetSuccessfullyPressed(bool setPressed)
    {
        if (setPressed)
        {
            SuccessfullyPressed = true;
        }
        else
        {
            SuccessfullyPressed = false;
        }
    }
}
