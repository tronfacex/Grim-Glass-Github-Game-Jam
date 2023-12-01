using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

class LoadAudioAsync : MonoBehaviour
{
    // List of Banks to load
    [FMODUnity.BankRef]
    public List<string> Banks = new List<string>();

    // The name of the scene to load and switch to
    public string Scene = null;

    [SerializeField] private float timer = 10f;
    [SerializeField] private bool timerElapsed;

    [SerializeField] private RectTransform LoadingText;

    public void Start()
    {
        //StartCoroutine(LoadGameAsync());
        //LoadingText.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.875f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        LoadingText.DORotate(new Vector3(0, 0, -180), 3.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    void Update()
    {
        // Update the loading indication
        timer = Mathf.Max(timer - Time.deltaTime, 0f);
        if (timer == 0 && !timerElapsed)
        {
            LoadingText.DOPause();
            timerElapsed = true;
            StartCoroutine(LoadGameAsync());
            //SceneManager.LoadScene("Start Menu");
        }
    }

    IEnumerator LoadGameAsync()
    {
        // Start an asynchronous operation to load the scene
        AsyncOperation async = SceneManager.LoadSceneAsync(Scene);

        // Don't let the scene start until all Studio Banks have finished loading
        async.allowSceneActivation = false;

        // Iterate all the Studio Banks and start them loading in the background
        // including the audio sample data
        foreach (var bank in Banks)
        {
            FMODUnity.RuntimeManager.LoadBank(bank, true);
        }

        // Keep yielding the co-routine until all the bank loading is done
        // (for platforms with asynchronous bank loading)
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        // Keep yielding the co-routine until all the sample data loading is done
        while (FMODUnity.RuntimeManager.AnySampleDataLoading())
        {
            yield return null;
        }

        // Allow the scene to be activated. This means that any OnActivated() or Start()
        // methods will be guaranteed that all FMOD Studio loading will be completed and
        // there will be no delay in starting events
        async.allowSceneActivation = true;

        // Keep yielding the co-routine until scene loading and activation is done.
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
