using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] private MusicArea area;

    [SerializeField] private MusicArea previousArea;

    [SerializeField] private bool isCombatArea;

    [SerializeField] private List<GameObject> enemies;

    private void Update()
    {
        if (!isCombatArea) { return; }

        if (enemies.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return;  }

        previousArea = GameDataReader.Instance.GameData.currentMusicArea;

        AudioManager.instance.SetMusicAreaParameter(area);

        GameDataReader.Instance.GameData.currentMusicArea = area;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isCombatArea) { return; }

        if (!other.CompareTag("Player")) { return; }

        AudioManager.instance.SetMusicAreaParameter(previousArea);

        GameDataReader.Instance.GameData.currentMusicArea = previousArea;
    }

    private void OnDestroy()
    {
        AudioManager.instance.SetMusicAreaParameter(previousArea);

        GameDataReader.Instance.GameData.currentMusicArea = previousArea;
    }
}
