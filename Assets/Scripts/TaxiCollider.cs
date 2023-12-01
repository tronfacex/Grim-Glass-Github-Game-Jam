using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class TaxiCollider : MonoBehaviour
{
    [SerializeField] private GameEventScriptableObject EndEventTriggered;
    [SerializeField] private GameEventScriptableObject EndEventEnded;

    [SerializeField] private MusicArea MusicChange;

    [SerializeField] private float timer = 25f;

    [SerializeField] private bool sceneLoaded = false;
    [SerializeField] private bool countdownStarted = false;
    [SerializeField] private bool audioPlayed = false;

    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject ThankYouPanel;
    [SerializeField] private GameObject platformFailCollider;

    [SerializeField] private EventInstance recycleBinSFX;

    /*private void OnEnable()
    {
        videoPanel = GameObject.FindGameObjectWithTag("Video Panel");
        videoPanel.SetActive(false);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        //recycleBinSFX = AudioManager.instance.CreateEventInstance(FMODEvents.instance.recycleEndingSFX);

        platformFailCollider.SetActive(false);

        AudioManager.instance.SetMusicAreaParameter(MusicChange);

        /*FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(transform.position);
        recycleBinSFX.set3DAttributes(attributes);*/

        EndEventTriggered?.Raise();


        Debug.Log("Recycle Event Fired THIS BETTER HAVE BEEN ON PURPOSE");
        //recycleBinSFX.start();

        //videoPanel.SetActive(true);

        countdownStarted = true;


    }

    /*private void Start()
    {
        //StartCoroutine(videoCheckDelay());
    }*/

    private IEnumerator videoCheckDelay()
    {
        yield return new WaitForSeconds(0.2f);
        if (videoPanel == null)
        {
            videoPanel = GameObject.FindGameObjectWithTag("Video Panel");
            videoPanel.SetActive(false);
        }
    }

    private void Update()
    {
        /*if (videoPanel.activeSelf && !audioPlayed)
        {
            audioPlayed = true;
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.recycleEndingSFX, gameObject.transform.position);
            recycleBinSFX.start();
        }*/



        if (countdownStarted)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0f);

            if (timer == 0 && !sceneLoaded)
            {
                sceneLoaded = true;
                StartCoroutine(DelayEndEventComplete());
                EndEventEnded?.Raise();

            }
        }

    }

    private IEnumerator DelayEndEventComplete()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Loading");
    }
}

