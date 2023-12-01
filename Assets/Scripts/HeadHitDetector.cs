using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitDetector : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private LayerMask detectableLayers = (1 << 8) | (1 << 3);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && !stateMachine.IsGrounded)
        {
            stateMachine.ForceReceiver.JumpCutoffGravity(0f, 0f);
            stateMachine.ForceReceiver.ChangeGravityModifier(4f);
        }
    }
}
