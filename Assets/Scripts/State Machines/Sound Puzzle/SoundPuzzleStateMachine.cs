using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SoundPuzzleStateMachine : StateMachine
{
    [field: SerializeField] public PlayerStateMachine playerStateMachine { get; private set; }

    [field: SerializeField] public ButtonStateMachine BlueBottleButtonStateMachine { get; private set; }

    [field: SerializeField] public Health BlueHealth;
    [field: SerializeField] public ButtonStateMachine GreenBottleButtonStateMachine { get; private set; }
    [field: SerializeField] public Health GreenHealth;

    [field: SerializeField] public ButtonStateMachine RedBottleButtonStateMachine { get; private set; }
    [field: SerializeField] public Health RedHealth;

    [field: SerializeField] public ButtonStateMachine ExamplePlayButtonStateMachine { get; private set; }

    [field: SerializeField] private UIStateMachine uIStateMachine;

    [field: SerializeField] private HUDStateMachine hUDStateMachine;

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;

    [field: SerializeField] public GameObject PuzzleWall;

    [field: SerializeField] public GameObject Explosion;

    [field: SerializeField] public GameObject BoomObject;

    [field: SerializeField] public float CameraHoldDuration { get; private set; }

    [field: SerializeField] public List<string> AnswerKey { get; private set; }

    [field: SerializeField] public List<string> PlayerAnswers;

    [field: SerializeField] public GameEventScriptableObject SoundPuzzleCompletedEvent;

    [field: SerializeField] public GameEventScriptableObject SoundPuzzleFailedEvent;

    [field: SerializeField] public GameEventScriptableObject SoundPuzzlePlayExampleEvent;

    [field: SerializeField] public MeshRenderer Answer1Sign { get; private set; }
    [field: SerializeField] public MeshRenderer Answer2Sign { get; private set; }
    [field: SerializeField] public MeshRenderer Answer3Sign { get; private set; }

    [field: SerializeField] public Material GreenLight { get; private set; }
    [field: SerializeField] public Material RedLight { get; private set; }
    [field: SerializeField] public Material BlueLight { get; private set; }
    [field: SerializeField] public Material NeutralLight { get; private set; }


    [field: SerializeField] public CinemachineVirtualCamera PlayExampleCam { get; private set; }

    public void OnPlayerDied()
    {
        PlayerAnswers.Clear();
        SwitchState(new SoundPuzzleFailState(this));
    }

    public void OnMilkBottleHit(string playerAnswer)
    {
        PlayerAnswers.Add(playerAnswer);
        Material AnswerMaterial = null;
        if (playerAnswer == "green")
        {
            AnswerMaterial = GreenLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise2, GreenBottleButtonStateMachine.transform.position);
        }
        if (playerAnswer == "blue")
        {
            AnswerMaterial = BlueLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise1, BlueBottleButtonStateMachine.transform.position);
        }
        if (playerAnswer == "red")
        {
            AnswerMaterial = RedLight;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.soundPuzzleNoise3, RedBottleButtonStateMachine.transform.position);
        }

        if (AnswerMaterial == null) { return; }

        int answerIndex = PlayerAnswers.Count;
        if (answerIndex == 1)
        {
            Answer1Sign.material = AnswerMaterial;
        }
        if (answerIndex == 2)
        {
            Answer2Sign.material = AnswerMaterial;
        }
        if (answerIndex == 3)
        {
            Answer3Sign.material = AnswerMaterial;
        }
        if (PlayerAnswers.Count == 3)
        {
            if (PlayerAnswers[0] != AnswerKey[0])
            {
                PlayerAnswers.Clear();
                SwitchState(new SoundPuzzleFailState(this));
                return;
            }
            if (PlayerAnswers[1] != AnswerKey[1])
            {
                PlayerAnswers.Clear();
                SwitchState(new SoundPuzzleFailState(this));
                return;
            }
            if (PlayerAnswers[2] != AnswerKey[2])
            {
                PlayerAnswers.Clear();
                SwitchState(new SoundPuzzleFailState(this));
                return;
            }
            SwitchState(new SoundPuzzleCompletedState(this));
        }
    }

    public void OnPlayExample()
    {
        PlayerAnswers.Clear();
        playerStateMachine.SwitchState(new PlayerIdleState(playerStateMachine));
        SwitchState(new SoundPuzzlePlayExampleState(this));
    }

    public void OnSoundPuzzleSkip()
    {
        SwitchState(new SoundPuzzleCompletedState(this));
    }
}
