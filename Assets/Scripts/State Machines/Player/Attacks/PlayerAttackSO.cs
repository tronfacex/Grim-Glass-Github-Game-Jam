using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack", menuName = "Scriptable Objects/Player Attack")]
public class PlayerAttackSO : ScriptableObject
{
    // How to use
    // Note: To create a whole new settings mode right click in Unity Scriptable Objects -> Player Attack.
    // Then name it appropriately and fill in the pertinent values.

    [field: SerializeField] public string AnimationString { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    //[field: SerializeField] public float ComboAttackTime { get; private set; }
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public bool AirAttack { get; private set; }
    //[field: SerializeField] public float AttackMoveDistance { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float Knockback { get; private set; }
    [field: SerializeField] public float Knockdown { get; private set; }
    [field: SerializeField] public int TrailEffect { get; private set; }
    [field: SerializeField] public float TrailEffectTime { get; private set; }
}
