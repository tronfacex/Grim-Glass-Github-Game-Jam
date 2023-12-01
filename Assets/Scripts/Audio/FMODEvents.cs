using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference platformingMusic { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference windAmbience { get; private set; }
    [field: SerializeField] public EventReference vinylAmbience { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference bobbyImpact { get; private set; }
    [field: SerializeField] public EventReference bobbyAirImpact { get; private set; }
    [field: SerializeField] public EventReference bobbyWhoosh { get; private set; }
    [field: SerializeField] public EventReference playerImpacts { get; private set; }

    [field: Header("Collectable SFX")]
    [field: SerializeField] public EventReference objectCollected { get; private set; }
    [field: SerializeField] public EventReference healthCollected { get; private set; }

    [field: Header("General SFX")]
    [field: SerializeField] public EventReference explosionLong { get; private set; }
    [field: SerializeField] public EventReference bossExplosion { get; private set; }
    [field: SerializeField] public EventReference recycleEndingSFX { get; private set; }
    [field: SerializeField] public EventReference uIClick { get; private set; }

    [field: Header("Sound Puzzle SFX")]
    [field: SerializeField] public EventReference soundPuzzleNoise1 { get; private set; }
    [field: SerializeField] public EventReference soundPuzzleNoise2 { get; private set; }
    [field: SerializeField] public EventReference soundPuzzleNoise3 { get; private set; }

    [field: SerializeField] public EventReference puzzleFailTone { get; private set; }

    [field: Header("Scale Puzzle SFX")]
    [field: SerializeField] public EventReference milkPour { get; private set; }
    [field: SerializeField] public EventReference successDing { get; private set; }
    [field: SerializeField] public EventReference successViolin { get; private set; }
    [field: SerializeField] public EventReference audienceClapping { get; private set; }
    [field: SerializeField] public EventReference glassShatter { get; private set; }
    [field: SerializeField] public EventReference meleeAttackSound { get; private set; }
    [field: SerializeField] public EventReference vomitAttackSound { get; private set; }
    [field: SerializeField] public EventReference rangedAttackSound { get; private set; }
    [field: SerializeField] public EventReference vomitLoopSound { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMODEVENTS in the scene");
        }
        instance = this;
    }
}
