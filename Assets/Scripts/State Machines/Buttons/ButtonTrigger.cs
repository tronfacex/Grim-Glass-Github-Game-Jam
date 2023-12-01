using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Transform button;
    [SerializeField] private ButtonStateMachine stateMachine;

    private void OnTriggerEnter(Collider other)
    {
        if (stateMachine.SuccessfullyPressed) { return; }
        if (!other.CompareTag("Player")) { return; }
        button.DOScaleY(.5f, 0.1f);
        stateMachine.SwitchState(new ButtonPressedState(stateMachine));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        if (!stateMachine.ResetOnExit) { return; }

        stateMachine.SuccessfullyPressed = false;

        //button.DOScaleY(1, 0.15f);
    }
}
