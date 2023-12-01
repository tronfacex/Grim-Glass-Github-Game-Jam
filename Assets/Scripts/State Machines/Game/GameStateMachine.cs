using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameStateMachine : StateMachine
{
    //[field: SerializeField] public InputReader InputReader { get; private set; }
    //[field: SerializeField] public PlatformTweener PlatformTweener { get; private set; }

    [field: SerializeField] public PlayerStateMachine playerStateMachine { get; private set; }

    [field: SerializeField] public LevelCheckpoints levelCheckpoints { get; private set; }

    [field: SerializeField] private UIStateMachine uIStateMachine;

    [field: SerializeField] private HUDStateMachine hUDStateMachine;
    [field: SerializeField] public AdditiveSceneLoader AdditiveSceneLoader { get; private set; }

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;

    [field: SerializeField] public List<GameObject> Enemies;

    [field: SerializeField] public GameEventScriptableObject Level1StagingEvent;

    [field: SerializeField] public GameEventScriptableObject Level2StagingEvent;

    [field: SerializeField] public GameEventScriptableObject Level3StagingEvent;

    [field: SerializeField] public GameEventScriptableObject SoundPuzzleSkipEvent;

    [field: SerializeField] public GameEventScriptableObject SoundPuzzleCompletedEvent;

    [field: SerializeField] public GameEventScriptableObject RestoreAllEnemyHealthEvent;
    [field: SerializeField] public GameEventScriptableObject MilkBossesDefeatedEvent;
    [field: SerializeField] public GameEventScriptableObject OpeningCutsceneEndedEvent;
    [field: SerializeField] public GameEventScriptableObject PauseMenuOffEvent;

    [field: SerializeField] public bool SoundPuzzleCompleted;

    [field: SerializeField] public string ControllerType { get; private set; }

    [field: SerializeField] public string PreviousAction { get; private set; }

    [field: SerializeField] public CinemachineFreeLook FreeLookCamera { get; private set; }

    //[field: SerializeField] private int idleStateHash = Animator.StringToHash("Idle State");

    //[field: SerializeField] private int movingStateHash = Animator.StringToHash("Moving State");

    //public Transform MainCameraTransform { get; private set; }


    //NEED A GAME EVENT LISTENER FOR PAUSE MENU ON

    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;

        SwitchState(new GameStartupState(this, true));
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }
    private void OnDisable()
    {
        GameDataReader.Instance.GameData.CurrentLevelIndex = 0;
        GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex = 0;
        GameDataReader.Instance.GameData.CollectablesCount = 0;
        GameDataReader.Instance.GameData.HasMetBobby = false;
        GameDataReader.Instance.GameData.CurrentTimeScale = 1;
        GameDataReader.Instance.GameData.NumberOfBossesDefeated = 0;
        GameDataReader.Instance.GameData.Level3CheckpointList.Clear();
        GameDataReader.Instance.GameData.CurrentLevelCheckpointList.Clear();
        InputSystem.onActionChange -= OnActionChange;
        //GameDataReader.Instance.GameData.CurrentLevelCheckpointList = null;
    }

    public void SwitchToResetToCheckPointState()
    {
        SwitchState(new GameResetToCheckpointState(this));
    }

    public void SwitchActiveGameState()
    {
        SwitchState(new GameActiveState(this));
    }

    public void SwitchGameOverState()
    {
        SwitchState(new GameOverState(this));
    }

    public void OnSkipToNextCheckpoint()
    {
        int lastCheckpoint = levelCheckpoints.LevelCheckpointList.Count - 1;
        if (GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex > 5)
        {
            Level3StagingEvent?.Raise();
        }
        if (GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex == lastCheckpoint)
        {
            return;
        }
        else
        {
            GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex++;
            if (GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex == 15 && GameDataReader.Instance.GameData.CurrentLevelIndex == 3)
            {
                if (!SoundPuzzleCompleted)
                {
                    SoundPuzzleSkipEvent?.Raise();
                }
            }
            SwitchState(new GameStartupState(this, false));
            //PauseMenuOffEvent?.Raise();
        }
    }

    public void OnSoundPuzzleCompleted()
    {
        SoundPuzzleCompleted = true;
    }

    /*public void SwitchToActiveState()
    {
        if (CurrentState != "Game Active State")
        {
            SwitchState(new GameActiveState(this));
            return;
        }
    }

    public void ReturnToActiveState()
    {
        if (CurrentState != "Game Active State")
        {
            SwitchState(new GameActiveState(this));
            return;
        }
    }*/

    /*public void OnPlatformFailureDetected()
    {
        Debug.Log("DOES IT RUN");
        playerStateMachine.Health.DealPlatformFailDamage();
        float playerHealth = playerStateMachine.Health.health;
        if (playerHealth > 0)
        {
            SwitchState(new GameResetToCheckpointState(this));
        }
        if (playerHealth == 0)
        {
            //UIStateMachine.SwitchState(Gameovermenu);

            SwitchState(new GameOverState(this));
        }
    }*/

    public void OnRestartLevel()
    {
        SwitchState(new GameResetToLevelState(this));
    }

    public void OnPlayerDied()
    {
        SwitchState(new GameOverState(this));
    }

    public void OnPauseMenuTweened()
    {
        SwitchState(new GamePauseState(this));
    }

    public void OnPauseMenuExited()
    {
        SwitchState(new GameActiveState(this));
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        // Check the type of control that triggered the action
        if (obj is InputAction action)
        {
            if (action.activeControl != null && action.activeControl.device.ToString() != PreviousAction)
            {
                if (action.activeControl.device is Gamepad)
                {
                    // Gamepad input received
                    playerStateMachine.ChangeSprintSpeedThreshold(playerStateMachine.gamepadSprintSpeedThreshold);

                    if (action.activeControl.device.layout == "XInputControllerWindows")
                    {
                        ControllerType = "Xbox";
                        GameDataReader.Instance.GameData.ControllerType = ControllerType;
                        //Debug.Log("Xbox controller detected" + action.activeControl.device.layout);
                        //Debug.Log(GameDataReader.Instance.GameData.ControllerType);
                        /*if (!setLookSensitivityAtStart && checkTimer == 0)
                        {
                            setLookSensitivityAtStart = true;
                        }*/
                    }
                    else
                    {
                        ControllerType = "Playstation";
                        GameDataReader.Instance.GameData.ControllerType = ControllerType;
                        //Debug.Log("Non-xbox controller detected" + action.activeControl.device.layout);
                        //Debug.Log(GameDataReader.Instance.GameData.ControllerType);
                        /*if (!setLookSensitivityAtStart && checkTimer == 0)
                        {
                            setLookSensitivityAtStart = true;
                        }*/
                    }

                }
                else if (action.activeControl.device is Keyboard || action.activeControl.device is Mouse)
                {
                    ControllerType = "KBM";
                    GameDataReader.Instance.GameData.ControllerType = ControllerType;
                    //Debug.Log("KBM Detected" + action.activeControl.device.layout);
                    //Debug.Log(GameDataReader.Instance.GameData.ControllerType);
                    // Keyboard or Mouse input received
                    playerStateMachine.ChangeSprintSpeedThreshold(playerStateMachine.KBMSprintSpeedThreshold);
                    /*if (!setLookSensitivityAtStart && checkTimer == 0)
                    {
                        setLookSensitivityAtStart = true;
                        FreeLookCamera.m_XAxis.m_MaxSpeed = FreeLookCamera.m_XAxis.m_MaxSpeed * .5f;
                        FreeLookCamera.m_YAxis.m_MaxSpeed = FreeLookCamera.m_YAxis.m_MaxSpeed * .5f;
                    }*/
                }
                PreviousAction = action.activeControl.device.ToString();
            }
        }
    }
}
