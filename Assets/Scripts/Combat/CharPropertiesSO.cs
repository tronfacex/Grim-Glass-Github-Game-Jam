using UnityEngine;

[CreateAssetMenu(fileName = "Character Properties", menuName = "Scriptable Objects/Character Properties")]
public class CharPropertiesSO : ScriptableObject
{
    // How to use
    // Note: To create a whole new settings mode right click in Unity Scriptable Objects -> Player Attack.
    // Then name it appropriately and fill in the pertinent values.

    [field: SerializeField] public int MaxHealth;
    [field: SerializeField] public float FloatDuration;
    [field: SerializeField] public float KnockDownThreshold { get; private set; }
    [field: SerializeField] public float KnockDownAmountDecayRate { get; private set; }
}
