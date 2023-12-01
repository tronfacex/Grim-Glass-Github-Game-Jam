using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCollectableTotal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCollectablesInScene;

    [SerializeField] private TextMeshProUGUI playerTotal;

    // Start is called before the first frame update
    void Start()
    {
        
        totalCollectablesInScene.text = GameDataReader.Instance.GameData.AllCollectablesInSceneCount.ToString();

        playerTotal.text = GameDataReader.Instance.GameData.CollectablesCount.ToString();
    }
}
