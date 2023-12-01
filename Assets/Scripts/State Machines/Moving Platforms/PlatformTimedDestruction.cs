using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTimedDestruction : MonoBehaviour
{
    [SerializeField] private bool beginTimer;
    [SerializeField] private MeshRenderer platformMeshRenderer;
    [SerializeField] private float flashPlatformDelay = .15f;
    [field: SerializeField] public bool PlatformDestroyed { get; private set; } 
    [SerializeField] private float time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PlatformDestroyed)
            {
                beginTimer = true;
            }
        }
    }

    private void Update()
    {
        if (beginTimer)
        {
            time = Mathf.Max(time - Time.deltaTime, 0);
        }
        if (time == 0)
        {
            StartCoroutine(beginPlatformDestruction());
        }
    }

    private IEnumerator beginPlatformDestruction()
    {
        platformMeshRenderer.enabled = false;
        yield return new WaitForSeconds(flashPlatformDelay);
        platformMeshRenderer.enabled = true;
        yield return new WaitForSeconds(flashPlatformDelay);
        platformMeshRenderer.enabled = false;
        flashPlatformDelay = flashPlatformDelay * .75f;
        yield return flashPlatformDelay;
        platformMeshRenderer.enabled = true;
        yield return flashPlatformDelay;
        platformMeshRenderer.enabled = false;
        flashPlatformDelay = flashPlatformDelay * .5f;
        yield return flashPlatformDelay;
        platformMeshRenderer.enabled = true;
        yield return flashPlatformDelay;
        platformMeshRenderer.enabled = false;
        flashPlatformDelay = flashPlatformDelay * .5f;
        yield return flashPlatformDelay;
        platformMeshRenderer.enabled = true;

        //Destroy platform method here
    }
}
