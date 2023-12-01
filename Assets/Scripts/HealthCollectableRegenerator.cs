using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectableRegenerator : MonoBehaviour
{
    [SerializeField] private GameObject CollectableFront;
    [SerializeField] private GameObject CollectableBack;
    [SerializeField] private GameObject Collider;

    public void OnPlayerDiedReset()
    {
        CollectableFront.SetActive(true);
        CollectableBack.SetActive(true);
        Collider.SetActive(true);
    }
}
