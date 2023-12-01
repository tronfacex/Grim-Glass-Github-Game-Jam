using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    //Controller settings data
    public bool InvertYAxis;
    public float XAxisLookSensitivity;
    public float YAxisLookSensitivity;

    //Gameplay difficulty
    public CharPropertiesSO PlayerCharacterProperties;
}
