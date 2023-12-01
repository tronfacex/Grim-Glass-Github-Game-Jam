using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    //For loading scenes additively safe
    public void LoadHUDSceneAdditively()
    {
        if (SceneManager.GetSceneByName("HUD").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
            //Debug.Log("Additive scene loaded: HUD");
        }
    }

    public void LoadUISceneAdditively()
    {
        if (SceneManager.GetSceneByName("UI").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            //Debug.Log("Additive scene loaded: UI");
        }
    }
}
