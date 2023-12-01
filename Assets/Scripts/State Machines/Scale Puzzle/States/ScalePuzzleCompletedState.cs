using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScalePuzzleCompletedState : ScalePuzzleBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Scale Puzzle Completed State");
    [SerializeField] private string currentState = "Scale Puzzle Completed State";

    private float delayTime = 3.5f;
    private float vfxDelayTime = 2f;
    private bool puzzleComplete;
    private bool vfxTriggered;
    private bool milkBossesDefeated;
    private float bossCompletedCameraDelay = 0.5f;
    private float bossCompletedCameraHold = 3f;
    private bool cameraHoldCompleted = false;
    private bool bossDefeatedGameEventFired = false;

    public ScalePuzzleCompletedState(ScalePuzzleStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.LeftMilkFillLevel > stateMachine.RightMilkFillLevel)
        {
            //stateMachine.RightMilkFillLevel = stateMachine.LeftMilkFillLevel;
            stateMachine.RightMilkRenderer.sprite = stateMachine.GetSpriteForFillLevel(stateMachine.LeftMilkFillLevel);
        }

        if (stateMachine.LeftMilkFillLevel < stateMachine.RightMilkFillLevel)
        {
            //stateMachine.RightMilkFillLevel = stateMachine.LeftMilkFillLevel;
            stateMachine.LeftMilkRenderer.sprite = stateMachine.GetSpriteForFillLevel(stateMachine.RightMilkFillLevel);
        }

        

        stateMachine.CurrentStateHash = currentStateHash;
        stateMachine.CurrentState = currentState;


        AudioManager.instance.PlayOneShot(FMODEvents.instance.successDing, stateMachine.transform.position);

        stateMachine.BossRevealVCAM.Priority = 2;


    }

    public override void Tick(float deltaTime)
    {
        delayTime = Mathf.Max(delayTime - deltaTime, 0f);
        vfxDelayTime = Mathf.Max(vfxDelayTime - deltaTime, 0f);

        if (vfxDelayTime == 0 && !vfxTriggered)
        {
            vfxTriggered = true;
            
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bossExplosion, stateMachine.transform.position);
            stateMachine.OnPuzzleComplete();
        }

        if (delayTime == 0 && !puzzleComplete)
        {
            puzzleComplete = true;
            AudioManager.instance.SetMusicAreaParameter(MusicArea.Combat);
            stateMachine.LeftMilkBoss.SetActive(true);
            stateMachine.RightMilkBoss.SetActive(true);
            stateMachine.ScalePuzzleCompletedEvent?.Raise();
            stateMachine.BossRevealVCAM.Priority = 0;
        }

        if (GameDataReader.Instance.GameData.NumberOfBossesDefeated == 2 && !cameraHoldCompleted)
        {
            if (!bossDefeatedGameEventFired)
            {
                bossDefeatedGameEventFired = true;
                stateMachine.MilkBossesCompletedEvent?.Raise();
            }
            
            bossCompletedCameraDelay = Mathf.Max(bossCompletedCameraDelay - deltaTime, 0f);
            if (bossCompletedCameraDelay == 0 && !cameraHoldCompleted)
            {
                stateMachine.BossCompleteVCAM.Priority = 2;
                bossCompletedCameraHold = Mathf.Max(bossCompletedCameraHold - deltaTime, 0f);
                if (bossCompletedCameraHold == 0)
                {
                    cameraHoldCompleted = true;
                    stateMachine.BossCompleteVCAM.Priority = 0;
                    AudioManager.instance.SetMusicAreaParameter(MusicArea.Level2);
                }
            }
        }
    }

    public override void Exit()
    {
        

    }
}
