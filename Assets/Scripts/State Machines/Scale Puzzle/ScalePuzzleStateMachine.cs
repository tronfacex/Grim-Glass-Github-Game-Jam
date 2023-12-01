using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;

public class ScalePuzzleStateMachine : StateMachine
{
    //[field: SerializeField] public InputReader InputReader { get; private set; }
    //[field: SerializeField] public PlatformTweener PlatformTweener { get; private set; }

    [field: SerializeField] public PlayerStateMachine playerStateMachine { get; private set; }

    [field: SerializeField] public ButtonStateMachine RightFillButtonStateMachine { get; private set; }

    [field: SerializeField] public ButtonStateMachine LeftFillButtonStateMachine { get; private set; }

    [field: SerializeField] public ButtonStateMachine RedBottleButtonStateMachine { get; private set; }

    [field: SerializeField] public Transform ScaleArm { get; private set; }

    [field: SerializeField] public GameObject RightMilk;

    [field: SerializeField] public GameObject RightMilkEffect;

    [field: SerializeField] public GameObject RightMilkBoss;

    [field: SerializeField] public GameObject LeftMilk;

    [field: SerializeField] public GameObject LeftMilkEffect;

    [field: SerializeField] public GameObject LeftMilkBoss;

    [field: SerializeField] public float RightMilkFillLevel;

    [field: SerializeField] public float LeftMilkFillLevel;

    [field: SerializeField] public EventInstance milkPour;

    [field: SerializeField] public float FillSpeedModifier { get; private set; }
    [field: SerializeField] public float ArmRotationSpeedModifier { get; private set; }

    [field: SerializeField] public float DelayTime { get; private set; }


    [field: SerializeField] private UIStateMachine uIStateMachine;

    [field: SerializeField] private HUDStateMachine hUDStateMachine;

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;
    [field: SerializeField] public CinemachineVirtualCamera BossCompleteVCAM;
    [field: SerializeField] public CinemachineVirtualCamera BossRevealVCAM;

    [field: SerializeField] public GameObject Explosion;

    [field: SerializeField] public GameObject BoomObject;

    [field: SerializeField] public GameObject MilkPourRight;
    [field: SerializeField] public GameObject MilkPourLeft;

    [field: SerializeField] public SpriteRenderer LeftMilkRenderer;

    [field: SerializeField] public SpriteRenderer RightMilkRenderer;

    [field: SerializeField] public float CameraHoldDuration { get; private set; }

    [field: SerializeField] public GameEventScriptableObject ScalePuzzleCompletedEvent;

    [field: SerializeField] public GameEventScriptableObject ScalePuzzleResetEvent;

    [field: SerializeField] public GameEventScriptableObject ScalePuzzleFillRightStartEvent;
    [field: SerializeField] public GameEventScriptableObject ScalePuzzleFillRightEndEvent;

    [field: SerializeField] public GameEventScriptableObject ScalePuzzleFillLeftStartEvent;
    [field: SerializeField] public GameEventScriptableObject ScalePuzzleFillLeftEndEvent;
    [field: SerializeField] public GameEventScriptableObject MilkBossesCompletedEvent;

    private void Start()
    {
        SwitchState(new ScalePuzzleListeningState(this));
    }

    public void OnRightFill()
    {
        if (CurrentState == "Scale Puzzle Completed State") { return; }
        SwitchState(new ScalePuzzleFillRightState(this));
        

    }

    public void OnEndRightFill()
    {
        if (CurrentState == "Scale Puzzle Completed State") { return; }
        SwitchState(new ScalePuzzleListeningState(this));
    }

    public void OnLeftFill()
    {
        if (CurrentState == "Scale Puzzle Completed State") { return; }
        SwitchState(new ScalePuzzleFillLeftState(this));
        
    }

    public void OnEndLeftFill()
    {
        if (CurrentState == "Scale Puzzle Completed State") { return; }
        SwitchState(new ScalePuzzleListeningState(this));
        //ScalePuzzleFillLeftEndEvent?.Raise();
    }

    public void OnPuzzleComplete()
    {
        LeftMilk.SetActive(false);
        RightMilk.SetActive(false);
        LeftMilkEffect.SetActive(true);
        RightMilkEffect.SetActive(true);
        //LeftMilkBoss.SetActive(true);
        //RightMilkBoss.SetActive(true);
    }

    public void OnMilkBossesDefeated()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.audienceClapping, transform.position);
    }

    [System.Serializable]
    public class FillLevelSprite
    {
        public float fillLevel;
        public Sprite sprite;
    }

    public List<FillLevelSprite> fillLevelSprites;

    public Sprite GetSpriteForFillLevel(float fillAmount)
    {
        foreach (var item in fillLevelSprites)
        {
            if (fillAmount <= item.fillLevel)
            {
                return item.sprite;
            }
        }
        return null;
    }
}
