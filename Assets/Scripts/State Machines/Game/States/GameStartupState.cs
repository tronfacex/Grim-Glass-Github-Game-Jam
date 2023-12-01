using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartupState : GameBaseState
{
    [SerializeField] private int currentStateHash = Animator.StringToHash("Game Startup State");
    [SerializeField] private string currentState = "Game Startup State";

    private bool InitialGameStart;

    public GameStartupState(GameStateMachine stateMachine, bool initialGameStart) : base(stateMachine) 
    {
        InitialGameStart = initialGameStart;
    }

    public override void Enter()
    {
        //Debug.Log("Game Active State");
        stateMachine.CurrentStateHash = currentStateHash;
        GameDataReader.Instance.GameData.CurrentGameStateHash = currentStateHash;

        stateMachine.CurrentState = currentState;
        GameDataReader.Instance.GameData.CurrentGameState = currentState;

        //stateMachine.playerStateMachine.PlayerInCutscene = false;

        if (InitialGameStart)
        {
            stateMachine.AdditiveSceneLoader.LoadHUDSceneAdditively();
            stateMachine.AdditiveSceneLoader.LoadUISceneAdditively();
            GameDataReader.Instance.GameData.NumberOfBossesDefeated = 0;
            List<GameObject> allCollectables = new List<GameObject>();
            allCollectables.AddRange(GameObject.FindGameObjectsWithTag("Bottle Cap"));
            GameDataReader.Instance.GameData.AllCollectablesInSceneCount = allCollectables.Count;
        }


        if (GameDataReader.Instance.GameData.CurrentLevelIndex > 0)
        {
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 1)
            {
                //stateMachine.Level1StagingEvent.Raise();
            }
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 2)
            {
                //stateMachine.Level1StagingEvent.Raise();
                //stateMachine.Level2StagingEvent.Raise();
            }
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 3)
            {
                //stateMachine.Level1StagingEvent.Raise();
                //stateMachine.Level2StagingEvent.Raise();
                //stateMachine.Level3StagingEvent?.Raise();
                if (GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex > 14)
                {
                    if (!stateMachine.SoundPuzzleCompleted)
                    {
                        stateMachine.SoundPuzzleSkipEvent?.Raise();
                    }
                    
                    //stateMachine.SoundPuzzleCompletedEvent?.Raise();
                }
                if (GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex > 18)
                {
                    stateMachine.MilkBossesDefeatedEvent?.Raise();
                    //stateMachine.SoundPuzzleCompletedEvent?.Raise();
                }
            }
        }

        

        // ADD A METHOD TO LOAD ANY GAME SETTING DATA NEEDED AT STARTUP (might be useful to call an event here) opening art or whatever then exit to active game states

        //stateMachine.SwitchState(new GameActiveState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {
        if (GameDataReader.Instance.GameData.LevelCheckpointListsLoaded)
        {
            //GameDataReader.Instance.GameData.CurrentLevelCheckpointList.Clear();
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 1)
            {
                //GameDataReader.Instance.GameData.CurrentLevelCheckpointList = GameDataReader.Instance.GameData.Level1CheckpointList;
            }
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 2)
            {
                //GameDataReader.Instance.GameData.CurrentLevelCheckpointList = GameDataReader.Instance.GameData.Level2CheckpointList;
            }
            if (GameDataReader.Instance.GameData.CurrentLevelIndex == 3)
            {
                /*if (GameDataReader.Instance.GameData.CurrentLevelCheckpointList.Count == 0)
                {
                    GameDataReader.Instance.GameData.CurrentLevelCheckpointList = GameDataReader.Instance.GameData.Level3CheckpointList;
                }*/

            }
            if (GameDataReader.Instance.GameData.CurrentLevelIndex > 0)
            {
                stateMachine.playerStateMachine.HasMetBobby = true;
                GameDataReader.Instance.GameData.HasMetBobby = true;
                stateMachine.OpeningCutsceneEndedEvent?.Raise();
                AudioManager.instance.SetMusicAreaParameter(MusicArea.Level3);
                stateMachine.playerStateMachine.MovePlayerToCheckpoint();
            }
            stateMachine.SwitchState(new GameActiveState(stateMachine));
        }

    }

    public override void Exit()
    {
        
    }
}
