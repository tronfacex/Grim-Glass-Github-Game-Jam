using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour
{
    [SerializeField] private int CheckpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex = CheckpointIndex;
    }
}
