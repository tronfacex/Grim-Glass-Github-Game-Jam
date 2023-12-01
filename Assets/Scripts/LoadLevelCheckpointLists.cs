using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelCheckpointLists : MonoBehaviour
{
    [SerializeField] private LevelCheckpoints levelCheckpoints;

    // Start is called before the first frame update
    void Start()
    {
        //GameDataReader.Instance.GameData.CurrentLevelIndex = 3;
        //GameDataReader.Instance.GameData.Level3CheckpointList.Clear();
        //GameDataReader.Instance.GameData.Level3CheckpointList = levelCheckpoints.LevelCheckpointList;

        /*if (levelCheckpoints.LevelIndex == 3)
        {
            GameDataReader.Instance.GameData.Level3CheckpointList.Clear();
            //GameDataReader.Instance.GameData.Level3CheckpointList.AddRange(levelCheckpoints.LevelCheckpointList);
            GameDataReader.Instance.GameData.Level3CheckpointList = GameDataReader.Instance.GameData.CurrentLevelCheckpointList;
        }
        if (levelCheckpoints.LevelIndex == 2)
        {
            GameDataReader.Instance.GameData.Level2CheckpointList.Clear();
            GameDataReader.Instance.GameData.Level2CheckpointList.AddRange(levelCheckpoints.LevelCheckpointList);
        }
        if (levelCheckpoints.LevelIndex == 1)
        {
            GameDataReader.Instance.GameData.Level1CheckpointList.Clear();
            GameDataReader.Instance.GameData.Level1CheckpointList.AddRange(levelCheckpoints.LevelCheckpointList);
        }

        GameDataReader.Instance.GameData.LevelCheckpointListsLoaded = true;*/

        StartCoroutine(DelaySetLevel3Checkpoints());
    }

    private IEnumerator DelaySetLevel3Checkpoints()
    {
        yield return new WaitForSeconds(.3f);
        GameDataReader.Instance.GameData.Level3CheckpointList.Clear();
        GameDataReader.Instance.GameData.Level3CheckpointList = GameDataReader.Instance.GameData.CurrentLevelCheckpointList;
        GameDataReader.Instance.GameData.LevelCheckpointListsLoaded = true;
    }

}
