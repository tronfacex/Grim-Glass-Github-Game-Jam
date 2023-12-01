using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class LookSensitivityXSlider : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLookCam;

    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        if (freeLookCam == null)
        {
            freeLookCam = GameObject.FindGameObjectWithTag("Free Look Cam").GetComponent<CinemachineFreeLook>();
        }

    }

    private void Start()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = SettingsReader.Instance.GameSettings.XAxisLookSensitivity;
    }

    public void OnSliderChanged()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = slider.value;
    }
}
