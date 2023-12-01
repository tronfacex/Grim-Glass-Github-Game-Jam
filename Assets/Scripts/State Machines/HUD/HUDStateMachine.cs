using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class HUDStateMachine : StateMachine
{
    //[field: SerializeField] public HUDTweener HUDTweener { get; private set; }

    //[field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }

    [field: SerializeField] public GameStateMachine GameStateMachine { get; private set; }

    [field: SerializeField] public string CurrentState;

    [field: SerializeField] public int CurrentStateHash;

    [field: SerializeField] public GameEventScriptableObject FadeToBlackCompleteEvent;

    [field: SerializeField] public GameEventScriptableObject FadeReturnCompleteEvent;

    [field: SerializeField] public Transform FadeToBlackPanel { get; private set; }

    [field: SerializeField] public RectTransform HUDPanel { get; private set; }
    [field: SerializeField] public RectTransform HUDPanelHidingPos { get; private set; }
    [field: SerializeField] public RectTransform HUDPanelShowingPos { get; private set; }
    [field: SerializeField] public RectTransform CollectableCounter { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CollectableCounterText { get; private set; }
    [field: SerializeField] public Image CollectableCounterIcon { get; private set; }

    [field: SerializeField] public RectTransform HealthCounter { get; private set; }
    [field: SerializeField] public RectTransform HealthParentObj;
    [field: SerializeField] public GameObject HealthParentObj5 { get; private set; }
    [field: SerializeField] public GameObject HealthParentObj4 { get; private set; }
    [field: SerializeField] public GameObject HealthParentObj3 { get; private set; }
    [field: SerializeField] public GameObject HealthParentObj2 { get; private set; }
    [field: SerializeField] public GameObject HealthParentObj1 { get; private set; }
    [field: SerializeField] public List<Image> HealthIcons;

    [field: SerializeField] public RectTransform BossHealthPanel { get; private set; }
    [field: SerializeField] public RectTransform MilkBoss1ImageRect { get; private set; }
    [field: SerializeField] public RectTransform MilkBoss2ImageRect { get; private set; }
    [field: SerializeField] public Image MilkBoss1HealthImage { get; private set; }
    [field: SerializeField] public Image MilkBoss2HealthImage { get; private set; }

    [field: SerializeField] public GameObject VideoPanel;
    [field: SerializeField] public GameObject HappyEndingPanel;
    [field: SerializeField] public GameObject ThankYouPanel;

    //[SerializeField] public float healthFillAmount;


    [field: SerializeField] public GameEventScriptableObject HUDPanelShowAllComplete;

    [field: SerializeField] public GameEventScriptableObject HUDPanelHideAllComplete;

    /*[field: SerializeField] public GameEventScriptableObject HideAllUIEvent;

    [field: SerializeField] public GameEventScriptableObject PauseMenuOnEvent;

    [field: SerializeField] public GameEventScriptableObject PauseMenuOffEvent;*/

    //[field: SerializeField] private int idleStateHash = Animator.StringToHash("Idle State");

    //[field: SerializeField] private int movingStateHash = Animator.StringToHash("Moving State");

    //public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        //MainCameraTransform = Camera.main.transform;

        //SetHUDAccordingToDifficulty();

        SwitchState(new HUDHiddenState(this, 0f));
    }

    private void OnEnable()
    {
        //PlayerStateMachine.Health.OnPlatformFailTakeDamage += HandleResetToCheckpoint;
    }

    private void OnDisable()
    {
        //GameDataReader.Instance.GameData.MilkBottle1StateMachine.Health.OnTakeDamage -= OnMilkBottle1TakeDamage;
        //GameDataReader.Instance.GameData.MilkBottle2StateMachine.Health.OnTakeDamage -= OnMilkBottle2TakeDamage;
        //PlayerStateMachine.Health.OnPlatformFailTakeDamage += HandleResetToCheckpoint;
    }

    private void OnDestroy()
    {
        GameDataReader.Instance.GameData.MilkBottle1StateMachine = null;
        GameDataReader.Instance.GameData.MilkBottle2StateMachine = null;
    }

    public void OnMilkBottle1TakeDamage()
    {
        //MilkBoss1ImageRect.DOShakePosition(.2f, .5f, 10, 20);
        float healthMeterfill = (float)GameDataReader.Instance.GameData.MilkBottle1StateMachine.Health.health / (float)GameDataReader.Instance.GameData.MilkBossMaxHealth;
        //Debug.Log("Milk Bottle 1" + healthMeterfill);
        MilkBoss1HealthImage.fillAmount = healthMeterfill;
    }
    public void OnMilkBottle2TakeDamage()
    {
        //MilkBoss1ImageRect.DOShakePosition(.2f, .5f, 10, 20);
        float healthMeterfill = (float)GameDataReader.Instance.GameData.MilkBottle2StateMachine.Health.health / (float)GameDataReader.Instance.GameData.MilkBossMaxHealth;
        //Debug.Log("Milk Bottle 2" + healthMeterfill);
        MilkBoss2HealthImage.fillAmount = healthMeterfill;
    }
    public void OnScalePuzzleComplete()
    {
        //delayOnTakeDamageEventSubscribe();
        BossHealthPanel.gameObject.SetActive(true);
    }
    public void OnBossesDefeated()
    {
        BossHealthPanel.gameObject.SetActive(false);
    }

    private IEnumerator delayOnTakeDamageEventSubscribe()
    {
        yield return new WaitForSeconds(3f);
        //GameDataReader.Instance.GameData.MilkBottle1StateMachine.Health.OnTakeDamage += OnMilkBottle1TakeDamage;
        //GameDataReader.Instance.GameData.MilkBottle2StateMachine.Health.OnTakeDamage += OnMilkBottle2TakeDamage;
    }

    public void OnRestoreHealthEvent()
    {
        OnMilkBottle1TakeDamage();
        OnMilkBottle2TakeDamage();
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
    }

    public void OnRestartLevel()
    {
        SwitchState(new GameResetToLevelState(this));
    }*/

    public void SetHUDAccordingToDifficulty()
    {
        int difficultySettings = SettingsReader.Instance.GameSettings.PlayerCharacterProperties.MaxHealth;

        if (difficultySettings == 1)
        {
            HealthParentObj1.SetActive(true);
            HealthParentObj = HealthParentObj1.GetComponent<RectTransform>();

        }
        if (difficultySettings == 2)
        {
            HealthParentObj2.SetActive(true);
            HealthParentObj = HealthParentObj2.GetComponent<RectTransform>();
        }
        if (difficultySettings == 3)
        {
            HealthParentObj3.SetActive(true);
            HealthParentObj = HealthParentObj3.GetComponent<RectTransform>();
        }
        if (difficultySettings == 4)
        {
            HealthParentObj4.SetActive(true);
            HealthParentObj = HealthParentObj4.GetComponent<RectTransform>();
        }
        if (difficultySettings == 5)
        {
            HealthParentObj5.SetActive(true);
            HealthParentObj = HealthParentObj5.GetComponent<RectTransform>();
        }

        GameObject[] healthIconArray = GameObject.FindGameObjectsWithTag("Health Icon");

        foreach (GameObject icon in healthIconArray)
        {
            Image iconImage = icon.GetComponent<Image>();
            HealthIcons.Add(iconImage);
        }

        SwitchState(new HUDShowAllState(this, 1.5f));
    }

    public void HideAllHUDElements(float delayAmount)
    {
        StartCoroutine(HideHUDDelay(delayAmount));
    }

    private IEnumerator HideHUDDelay(float delayAmount)
    {
        yield return new WaitForSeconds(delayAmount);
        //HUDTweenerHideAllCustomMethod(); ON COMPLETE CALL SWITCH TO HUD HIDDENSTATE??
    }

    public void SwitchToHUDHiddenState(float hideDelay)
    {
        SwitchState(new HUDHiddenState(this, hideDelay));
    }

    public void SwitchToHUDRemoveHealthState(bool platformFailureDetected)
    {
        SwitchState(new HUDRemoveHealthState(this, platformFailureDetected));
    }
    public void SwitchToHUDRemoveHealthState()
    {
        SwitchState(new HUDRemoveHealthState(this, false));
    }
    public void SwitchHUDFadeToBlackState(float delayTime)
    {
        SwitchState(new HUDFadeToBlackState(this, delayTime));
    }

    public void SwitchHUDReturnFromBlackState(float delayTime)
    {
        SwitchState(new HUDReturnFromBlackState(this, delayTime));
    }

    public void SwitchHUDToShowAllState(float animationTime)
    {
        SwitchState(new HUDShowAllState(this, animationTime));
    }
    /*public void HandleResetToCheckpoint()
    {
        SwitchHUDFadeToBlackState();
    }*/
    public void OnTakeDamage()
    {
        if (CurrentState != "HUD Show All State")
        {
            SwitchState(new HUDShowAllState(this, .5f));
        }
        
        HealthIcons[GameDataReader.Instance.GameData.PlayerHealth]?.rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), .5f).OnComplete(ScaleHealthIconToZero);
        
    }

    public void TurnOnThankYouPanel()
    {
        ThankYouPanel.SetActive(true);
        ThankYouPanel.GetComponent<RectTransform>().DOScale(Vector3.one, .5f);
    }

    public void TurnOnVideoPanel()
    {
        VideoPanel.SetActive(true);
    }

    public void TurnOnHappyPanel()
    {
        HappyEndingPanel.SetActive(true);
    }

    private void ScaleHealthIconToZero()
    {
        HealthIcons[GameDataReader.Instance.GameData.PlayerHealth]?.rectTransform.DOScale(Vector3.zero, .2f);
    }
    public void OnHealthCollected()
    {
        if (CurrentState != "HUD Show All State")
        {
            SwitchState(new HUDShowAllState(this, .5f));
        }
        foreach (Image healthIcon in HealthIcons)
        {
            healthIcon.rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), .5f).OnComplete(ScaleHealthIconToOne);
        }
    }
    public void ScaleHealthIconToOne()
    {
        foreach (Image healthIcon in HealthIcons)
        {
            healthIcon.rectTransform.DOScale(Vector3.one, .2f);
        }
    }

    public void OnCollectableCollected()
    {
        if (CurrentState != "HUD Show All State")
        {
            SwitchState(new HUDShowAllState(this, .5f));
        }
        CollectableCounterText.text = GameDataReader.Instance.GameData.CollectablesCount.ToString();
        CollectableCounterText.rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), .75f).SetEase(Ease.OutBack).OnComplete(ReturnCollectableCounterScale);
    }
    public void ReturnCollectableCounterScale()
    {
        //StartCoroutine(ReturnCollectableCounterScaleDelay());
        CollectableCounterText.rectTransform.DOScale(new Vector3(1, 1, 1), .5f).SetEase(Ease.OutBack);
    }
    IEnumerator ReturnCollectableCounterScaleDelay()
    {
        yield return new WaitForSeconds(.05f);
        CollectableCounterText.rectTransform.DOScale(new Vector3(1, 1, 1), .5f).SetEase(Ease.OutBack);
    }

    public void OnPlayerGameOverReset()
    {
        //fade to black and then UI goes over the top 
        foreach (Image healthIcon in HealthIcons)
        {
            healthIcon.rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), .5f).OnComplete(ScaleHealthIconToOne);
        }
    }

    public void OnTimerStarted()
    {

    }
}