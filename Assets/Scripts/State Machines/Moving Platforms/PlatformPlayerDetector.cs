using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerDetector : MonoBehaviour
{
    [SerializeField] PlatformTweener platformTweener;

    [SerializeField] MovingPlatformStateMachine stateMachine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        stateMachine.SwitchState(new MovingPlatformMovingState(stateMachine));

    }
}
