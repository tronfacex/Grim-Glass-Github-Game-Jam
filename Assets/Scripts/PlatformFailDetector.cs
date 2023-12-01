using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFailDetector : MonoBehaviour
{
    [SerializeField] private GameEventScriptableObject PlatformFailureDetectedEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        PlatformFailureDetectedEvent?.Raise();
        //Debug.Log("Platform Failure Detected");
    }
}
