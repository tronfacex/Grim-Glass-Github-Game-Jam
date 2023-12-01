using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class LookSensitivityYSlider : MonoBehaviour
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
        freeLookCam.m_YAxis.m_MaxSpeed = SettingsReader.Instance.GameSettings.YAxisLookSensitivity;
    }

    public void OnSliderChanged()
    {
        freeLookCam.m_YAxis.m_MaxSpeed = slider.value;
    }
}
