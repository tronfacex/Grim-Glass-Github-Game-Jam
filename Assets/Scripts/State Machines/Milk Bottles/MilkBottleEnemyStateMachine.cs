using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using FMOD.Studio;

public class MilkBottleEnemyStateMachine : StateMachine
{
    /*[field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    [field: SerializeField] public float StandardMovementSpeed { get; private set; }
    [field: SerializeField] public float SprintMovementSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeedThreshold;
    [field: SerializeField] public float RoationDamping { get; private set; }

    [field: SerializeField] public bool StowWeaponCountdownStarted;
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float DodgeTime { get; private set; }
    [field: SerializeField] public float StepBackDodgeLength { get; private set; }
    [field: SerializeField] public float StepBackDodgeTime { get; private set; }

    [field: SerializeField] public bool StepBackDodgeCountdownStarted;
    [field: SerializeField] public bool DodgeToAttackStarted;
    [field: SerializeField] public PlayerAttackSO[] Attacks { get; private set; }
    [field: SerializeField] public GameObject BottleBackpack { get; private set; }
    [field: SerializeField] public MeshRenderer[] BackpackMeshRenderers { get; private set; }
    [field: SerializeField] public GameObject BottleWeapon { get; private set; }
    [field: SerializeField] public MeshRenderer[] WeaponMeshRenderers { get; private set; }*/

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController CharController { get; private set; }
    [field: SerializeField] public Transform StartingPos { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Knockdown Knockdown { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public AIDestinationSetter AIDestinationSetter { get; private set; }
    [field: SerializeField] public Seeker Seeker { get; private set; }
    [field: SerializeField] public GameObject RetreatDetector { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }

    [field: SerializeField] public float PlayerDetectionDistance;
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float AttackRange1 { get; private set; }
    [field: SerializeField] public float AttackRange2 { get; private set; }
    [field: SerializeField] public float AttackRange3 { get; private set; }
    [field: SerializeField] public float AttackRange4 { get; private set; }
    [field: SerializeField] public EnemyAttackSO[] Attacks { get; private set; }
    [field: SerializeField] public EnemyWeaponDamage EnemyWeapon { get; private set; }
    [field: SerializeField] public GameObject BarkSpew { get; private set; }
    [field: SerializeField] public GameObject MeleeWeaponGameObject { get; private set; }

    [field: SerializeField] public GameObject PoofEffect { get; private set; }

    [field: SerializeField] public EventInstance vomitSoundLoop;

    [field: SerializeField] public bool InKnockback;
    [field: SerializeField] public SpriteBillboard Billboard { get; private set; }
    [field: SerializeField] public SpriteDirectionalController SpriteDirectionalController { get; private set; }
    [field: SerializeField] public GameEventScriptableObject MilkBoss1DamageTaken;
    [field: SerializeField] public GameEventScriptableObject MilkBoss2DamageTaken;
    [field: SerializeField] public bool isBoss1;


    public GameObject Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        InKnockback = false;

        if (gameObject.name == "Milk Bottle Boss")
        {
            GameDataReader.Instance.GameData.MilkBottle1StateMachine = this;
            GameDataReader.Instance.GameData.MilkBossMaxHealth = Health.characterProperties.MaxHealth;
            isBoss1 = true;
        }
        else
        {
            GameDataReader.Instance.GameData.MilkBottle2StateMachine = this;
        }

        SwitchState(new MilkBottleEnemyIdleState(this, 0f));
        ForceReceiver.ChangeGravityModifier(1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionDistance);
    }

    private void OnEnable()
    {
        Health.OnDie += HandleOnDie;
        Health.OnTakeDamage += PlayImpactSFX;
        Health.OnTakeDamage += DecreaseBossMeter;
    }
    private void OnDisable()
    {
        Health.OnDie -= HandleOnDie;
        Health.OnTakeDamage -= PlayImpactSFX;
        Health.OnTakeDamage -= DecreaseBossMeter;
    }
    private void HandleTakeDamage()
    {
        if (name != gameObject.name) { return; }
        if (Knockdown.knockDownAmount >= Knockdown.knockDownThreshold)
        {
            Knockdown.knockDownAmount = 0;
            SwitchState(new MilkBottleEnemyKnockdownState(this));
        }
    }
    private void HandleOnDie()
    {
        SwitchState(new MilkBottleEnemyDeadState(this));
    }

    public void OnRestoreEnemyHealth()
    {
        SwitchState(new MilkBottleEnemyIdleState(this, 4.5f));
    }
    public void PlayImpactSFX()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bobbyImpact, gameObject.transform.position);
    }
    public void DecreaseBossMeter()
    {
        if (isBoss1)
        {
            MilkBoss1DamageTaken?.Raise();
        }
        else
        {
            MilkBoss2DamageTaken?.Raise();
        }
    }
    private void OnDestroy()
    {
        GameDataReader.Instance.GameData.NumberOfBossesDefeated++;
    }
}
