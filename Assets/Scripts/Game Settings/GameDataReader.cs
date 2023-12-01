using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataReader : MonoBehaviour
{
    // Note: There only needs to be one of these in the scene.

    [SerializeField]
    private GameDataSO _gameData;
    public GameDataSO GameData
    {
        get
        {
            return _gameData;
        }
        private set
        {

        }
    }
    private static GameDataReader _instance;
    public static GameDataReader Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
}
