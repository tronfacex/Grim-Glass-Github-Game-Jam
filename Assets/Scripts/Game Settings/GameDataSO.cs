using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Game Data", menuName = "Scriptable Objects/Game Data")]
public class GameDataSO : ScriptableObject
{
    //Check point data
    public int CurrentLevelIndex;
    public int CurrentLevelCheckpointIndex;
    public List<Transform> CurrentLevelCheckpointList;

    //Level 3 Checkpoint data
    public List<Transform> Level3CheckpointList;
    public List<Transform> Level2CheckpointList;
    public List<Transform> Level1CheckpointList;

    public bool LevelCheckpointListsLoaded;

    //Current music
    public MusicArea currentMusicArea;


    //Player health data
    public int PlayerHealth;

    //Player collectable data
    public int CollectablesCount;
    public int AllCollectablesInSceneCount;

    //Player opening cutscene data
    public bool HasMetBobby;
    public bool InGameOver;

    //Game state data
    public string CurrentGameState;
    public int CurrentGameStateHash;

    public float CurrentTimeScale;

    //Controller data
    public string ControllerType;

    //Final Boss fight data
    public int NumberOfBossesDefeated;
    public MilkBottleEnemyStateMachine MilkBottle1StateMachine;
    public MilkBottleEnemyStateMachine MilkBottle2StateMachine;
    public int MilkBossMaxHealth;
}
