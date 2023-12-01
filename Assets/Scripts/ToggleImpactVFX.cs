using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleImpactVFX : MonoBehaviour
{
    [SerializeField] private float timerDuration;
    [SerializeField] private float timer;

    private void OnEnable()
    {
        timer = timerDuration;
    }

    private void Update()
    {
        timer = Mathf.Max(timer - Time.deltaTime, 0f);
        if (timer == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
