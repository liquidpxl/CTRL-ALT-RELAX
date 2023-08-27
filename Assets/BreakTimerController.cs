using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class BreakTimerController : MonoBehaviour
{
    public Text elapsedTimeText; // Assign this in the Inspector

    private float startTime;
    public float timerDuration = 5400; // 90 minutes
    public float alertTimer = 5100f; // Sound an alarm 5 min before sleep
    public float shutdownTimer = 5392f; // Sound an alert 8s before sleep

    private bool hasInstantiated = false; // Flag to track instantiation
    private bool alarmSounded = false; // Flag to trag alert sound before sleep

    private bool shutdownAlarmSounded = false; // Flag to track shutdown alert sound

    public GameObject alertSound; // Set in the inspector
    public GameObject shutdownSound; // Set in the inspector

    private void Start()
    {
        startTime = Time.time;
        Invoke("SoundAlert", alertTimer);
        Invoke("ShutdownAlert", shutdownTimer);
    }

    private void Update()
    {
        float elapsedTime = Time.time - startTime;

        // Update UI to display the elapsed time
        UpdateElapsedTimeUI(elapsedTime);

        if (elapsedTime >= timerDuration)
        {
            StartSleepMode();
            // Call a method to initiate sleep  
        }
        
      
    }

    private void SoundAlert()
    {
        if (!alarmSounded)
        {
            // Instantiate the audio prefab
            GameObject instantiatedObject = Instantiate(alertSound);
            alarmSounded = true;
        }
    }

    private void ShutdownAlert()
    {
        if (!shutdownAlarmSounded)
        {
            GameObject instantiatedObject = Instantiate(shutdownSound);
            shutdownAlarmSounded = true;
        }
    }

    private void UpdateElapsedTimeUI(float elapsedTime)
    {
        // Format elapsed time as minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Update the UI Text element
        elapsedTimeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    private void StartSleepMode()
    {

        Debug.Log("Sleep initiated");

        if (!hasInstantiated)
        {

            hasInstantiated = true;

            // Use Windows API to attempt to initiate sleep mode
            bool success = SetSuspendState(false, false, false);
        }


    }

    [DllImport("PowrProf.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
}

