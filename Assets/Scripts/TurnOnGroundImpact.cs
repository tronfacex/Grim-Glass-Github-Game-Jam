using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnGroundImpact : MonoBehaviour
{
    [SerializeField] GameObject impactGameObject;
    [SerializeField] private PlayerStateMachine playerStateMachine;

    public void TurnOnImpact()
    {
        impactGameObject.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bobbyAirImpact, gameObject.transform.position);
        playerStateMachine.playerGroundSlamFeedback.PlayFeedbacks();
    }
}
