using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    private bool startedMusic;



    private List<EventInstance> eventInstances;

    private EventInstance ambienceWindEventInstance;

    private EventInstance platformingMusicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one audio manager in the scene");
        }
        instance = this;
        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    //THIS UPDATE SHOULD BE CHANGED TO A PUBLIC METHOD TRIGGERED BY UI SLIDER CHANGED EVENT
    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(sfxVolume);
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.vinylAmbience);
        AudioManager.instance.InitializeMusic(FMODEvents.instance.platformingMusic);
        SetMusicAreaParameter(MusicArea.Level3);
        //StartCoroutine(DelayMusicStart());
    }

    public bool IsBankLoaded(string bankName)
    {
        Bank bank;
        RuntimeManager.StudioSystem.getBank(bankName, out bank);
        LOADING_STATE loadingState;
        bank.getLoadingState(out loadingState);
        if (loadingState == LOADING_STATE.LOADED)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*IEnumerator DelayMusicStart()
    {
        yield return new WaitForSeconds(.5f);
        InitializeAmbience(FMODEvents.instance.vinylAmbience);
        AudioManager.instance.InitializeMusic(FMODEvents.instance.platformingMusic);
        SetMusicAreaParameter(MusicArea.Level3);
    }*/

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceWindEventInstance = CreateEventInstance(ambienceEventReference);
        FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(GameObject.FindGameObjectWithTag("Player").transform);
        ambienceWindEventInstance.set3DAttributes(attributes);
        ambienceWindEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceWindEventInstance.setParameterByName(parameterName, parameterValue);
    }
    public void InitializeMusic(EventReference ambienceEventReference)
    {
        platformingMusicEventInstance = CreateEventInstance(ambienceEventReference);
        //FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(GameObject.FindGameObjectWithTag("Player"));
        //.set3DAttributes(attributes);
        platformingMusicEventInstance.start();
    }
    public void SetMusicAreaParameter(MusicArea area)
    {
        platformingMusicEventInstance.setParameterByName("area", (float)area);
    }
}
