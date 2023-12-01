using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCutsceneLauncher : MonoBehaviour
{
    [SerializeField] private GameEventScriptableObject OpeningCutsceneTriggered;
    [SerializeField] private GameEventScriptableObject OpeningCutsceneEnded;

    [SerializeField] private MusicArea DialogueMusic;
    [SerializeField] private MusicArea MusicChange;

    [SerializeField] private bool keepMusicTheSameOnEntry;
    [SerializeField] private bool keepMusicTheSameOnExit;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return;  }

        if (!keepMusicTheSameOnEntry)
        {
            AudioManager.instance.SetMusicAreaParameter(DialogueMusic);
        }

        OpeningCutsceneTriggered?.Raise();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        OpeningCutsceneEnded?.Raise();

        if (!keepMusicTheSameOnExit)
        {
            AudioManager.instance.SetMusicAreaParameter(MusicChange);
        }

        gameObject.transform.SetParent(gameObject.transform);
        Destroy(gameObject);
    }
}
