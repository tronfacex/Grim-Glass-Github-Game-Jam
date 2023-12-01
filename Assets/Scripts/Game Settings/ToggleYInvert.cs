using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ToggleYInvert : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLookCam;

    [SerializeField]
    private Toggle toggle;

    private void Awake()
    {
        if (freeLookCam == null)
        {
            freeLookCam = GameObject.FindGameObjectWithTag("Free Look Cam").GetComponent<CinemachineFreeLook>();
        }

    }

    private void Start()
    {
        freeLookCam.m_YAxis.m_InvertInput = SettingsReader.Instance.GameSettings.InvertYAxis;
    }

    public void OnToggleChanged()
    {
        if (!freeLookCam.m_YAxis.m_InvertInput)
        {
            freeLookCam.m_YAxis.m_InvertInput = true;
        }
        else
        {
            freeLookCam.m_YAxis.m_InvertInput = false;
        }
        Debug.Log("YInvert toggled");
    }
}
