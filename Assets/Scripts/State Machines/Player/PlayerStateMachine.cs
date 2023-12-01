using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using FMOD.Studio;
using MoreMountains.Feedbacks;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public string previousAction { get; private set; }
    [field: SerializeField] public float gamepadSprintSpeedThreshold { get; private set; }
    [field: SerializeField] public float KBMSprintSpeedThreshold { get; private set; }
    [field: SerializeField] public CharacterController CharController { get; private set; }
    [field: SerializeField] public CinemachineFreeLook FreeLookCamera { get; private set; }
    [field: SerializeField] public Transform CameraLookTransform { get; private set; }
    [field: SerializeField] public Transform CameraFollowTransform { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public float GravityModifier { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public WeaponDamage GroundSlamWeapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    [field: SerializeField] public float StandardMovementSpeed { get; private set; }
    [field: SerializeField] public float SprintMovementSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeedThreshold { get; private set; }
    [field: SerializeField] public float JumpMovementSpeed { get; private set; }
    [field: SerializeField] public float RoationDamping { get; private set; }

    [field: SerializeField] public bool StowWeaponCountdownStarted;

    [field: SerializeField] public float StowWeaponDelaySpeed;
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float DoubleJumpForce { get; private set; }
    [field: SerializeField] public float FloatDuration { get; private set; }
    [field: SerializeField] public float FloatTimeRemaining;
    [field: SerializeField] public float GroundRaycastDistance { get; private set; }
    [field: SerializeField] public bool IsGrounded;
    [field: SerializeField] public bool CanDoubleJump;
    [field: SerializeField] public bool DoubleJumping;
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float DodgeTime { get; private set; }
    [field: SerializeField] public float StepBackDodgeLength { get; private set; }
    [field: SerializeField] public float StepBackDodgeTime { get; private set; }

    [field: SerializeField] public bool StepBackDodgeCountdownStarted;
    [field: SerializeField] public bool DodgeToAttackStarted;
    [field: SerializeField] public bool DodgeToJumpStarted;
    [field: SerializeField] public bool OnMovingPlatform { get; private set; }
    [SerializeField] public Transform ActivePlatform;
    [SerializeField] public Vector3 ActivePlatformVelocity;

    [field: SerializeField] public float PlatformGravityModifier { get; private set; }
    [field: SerializeField] public PlayerAttackSO[] Attacks { get; private set; }
    [field: SerializeField] public GameObject[] TrailEffects { get; private set; }
    [field: SerializeField] public GameObject BottleBackpack { get; private set; }
    [field: SerializeField] public MeshRenderer[] BackpackMeshRenderers { get; private set; }
    [field: SerializeField] public GameObject BottleWeapon { get; private set; }
    [field: SerializeField] public MeshRenderer[] WeaponMeshRenderers { get; private set; }
    [field: SerializeField] public bool HasMetBobby;

    [field: SerializeField] public GameEventScriptableObject PlayerDamageTakenEvent { get; private set; }
    [field: SerializeField] public GameEventScriptableObject PlayerDiedEvent { get; private set; }
    [field: SerializeField] public GameEventScriptableObject ResetToCheckPointCompleteEvent { get; private set; }
    [field: SerializeField] public GameEventScriptableObject OpeningCutSceneEndedEvent { get; private set; }
    [field: SerializeField] public GameEventScriptableObject PlayerDiedFadeOutEvent { get; private set; }

    [field: SerializeField] public bool PlayerInCutscene;

    [field: SerializeField] public EventInstance playerImpact;

    [field: SerializeField] public MMF_Player playerImpactFeedback;
    [field: SerializeField] public MMF_Player playerGroundSlamFeedback;

    [field: SerializeField] public LevelCheckpoints levelCheckpoints { get; private set; }


    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        FreeLookCamera.m_YAxis.m_InvertInput = SettingsReader.Instance.GameSettings.InvertYAxis;

        SprintSpeedThreshold = KBMSprintSpeedThreshold;

        WeaponMeshRenderers = BottleWeapon.GetComponentsInChildren<MeshRenderer>();
        BackpackMeshRenderers = BottleBackpack.GetComponentsInChildren<MeshRenderer>();

        SwitchState(new PlayerFreeMoveState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnPlatformFailTakeDamage += HandleTakePlatformFailDamage;
        Health.OnDie += HandleOnDie;
    }
    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleOnDie;
    }
    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
        PlayerDamageTakenEvent?.Raise();
    }

    public void HandlePlatformImpact()
    {
        SwitchState(new PlayerImpactState(this));
    }

    public void HandleTakePlatformFailDamage()
    {
        SwitchState(new PlayerReturnToCheckpointState(this));
    }

    public void MovePlayerToCheckpoint()
    {
        SwitchState(new PlayerReturnToCheckpointState(this));
    }

    private void HandleOnDie()
    {
        GameDataReader.Instance.GameData.InGameOver = true;
        SwitchState(new PlayerDeadState(this));
        PlayerDiedEvent?.Raise();
    }

    public void OnOpeningCutsceneEnd()
    {
        HasMetBobby = true;
        StowWeaponDelaySpeed = 10;
        StowWeaponCountdownStarted = true;
    }

    public void OnOpeningCutsceneStarted()
    {
        GameDataReader.Instance.GameData.HasMetBobby = true;
    }

    private void CheckDevice(InputDevice device, InputDeviceChange change)
    {
        // Check if the current device is a gamepad
        if (Gamepad.current != null)
        {
            SprintSpeedThreshold = gamepadSprintSpeedThreshold;
        }
        else if (Keyboard.current != null || Mouse.current != null)
        {
            // If the current device is not a gamepad, check if it is a keyboard or mouse
            SprintSpeedThreshold = KBMSprintSpeedThreshold;
        }
    }
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (obj is InputAction action)
        {
            if (action.activeControl != null && action.activeControl.device.ToString() != previousAction)
            {
                if (action.activeControl.device is Gamepad)
                {
                    SprintSpeedThreshold = gamepadSprintSpeedThreshold;
                }
                else if (action.activeControl.device is Keyboard || action.activeControl.device is Mouse)
                {
                    SprintSpeedThreshold = KBMSprintSpeedThreshold;
                }
                previousAction = action.activeControl.device.ToString();
            }
        }
    }

    public void ChangeSprintSpeedThreshold(float threshold)
    {
        SprintSpeedThreshold = threshold;
    }

    public void OnPlayerOnPlatform()
    {
        OnMovingPlatform = true;
    }
    public void OnPlayerOffPlatform()
    {
        OnMovingPlatform = false;
    }
}
