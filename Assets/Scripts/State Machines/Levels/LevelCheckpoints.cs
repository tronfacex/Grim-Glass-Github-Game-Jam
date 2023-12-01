using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour
{
    //Level 3 Checkpoint datas
    [SerializeField] public List<Transform> LevelCheckpointList;
    [SerializeField] public int LevelIndex;

    public void SetCheckpointList()
    {
        GameDataReader.Instance.GameData.CurrentLevelCheckpointList = LevelCheckpointList;
        //GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex = 0;
        //Debug.Log("Checkpoint reset to 0 " + GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex);
        GameDataReader.Instance.GameData.CurrentLevelIndex = LevelIndex;
        foreach (var transform in GameDataReader.Instance.GameData.CurrentLevelCheckpointList)
        {
            //Debug.Log(transform.name);
        }
        
    }

    private void Start()
    {
        /*if (LevelIndex == 1)
        {
            GameDataReader.Instance.GameData.Level1CheckpointList = LevelCheckpointList;
        }
        if (LevelIndex == 2)
        {
            GameDataReader.Instance.GameData.Level2CheckpointList = LevelCheckpointList;
        }
        if (LevelIndex == 3)
        {
            GameDataReader.Instance.GameData.Level3CheckpointList = LevelCheckpointList;
        }*/
        GameDataReader.Instance.GameData.Level3CheckpointList.Clear();
        //GameDataReader.Instance.GameData.Level3CheckpointList = LevelCheckpointList;
        //GameDataReader.Instance.GameData.CurrentLevelCheckpointList = LevelCheckpointList;
        StartCoroutine(DelaySetCheckpointsList());
        //GameDataReader.Instance.GameData.LevelCheckpointListsLoaded = true;
    }

    public void CheckpointReached(int checkPointIndex)
    {
        GameDataReader.Instance.GameData.CurrentLevelCheckpointIndex = checkPointIndex;
    }

    private IEnumerator DelaySetCheckpointsList()
    {
        yield return new WaitForSeconds(.25f);
        SetCheckpointList();
    }
}
