using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsReader : MonoBehaviour
{
    // Note: There only needs to be one of these in the scene.
    // This class is a Singleton, which allows other classes to reference its instance without a class reference. ty
    // The advantage is that you only need to drag and drop a new GameSettingsScriptableObjects into this class and it will be available globally.

    [SerializeField]
    private GameSettingsSO _gameSettings;
    public GameSettingsSO GameSettings
    {
        get
        {
            return _gameSettings;
        }
        private set
        {

        }
    }
    private static SettingsReader _instance;
    public static SettingsReader Instance
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
