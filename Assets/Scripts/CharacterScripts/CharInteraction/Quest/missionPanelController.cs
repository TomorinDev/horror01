using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelController : MonoBehaviour
{
    public Text missionTitleText;        // Mission Title
    public Text missionDescriptionText;  // Mission Description

    // Update Mission Data
    public void updateMission(string title, string description)
    {
        // Update Mission Data
        missionTitleText.text = title;             
        missionDescriptionText.text = description; 
    }

    // Hide Mission Panel
    public void hideMissionPanel()
    {
        gameObject.SetActive(false);
    }

    // Show Mission Panel
    public void showMissionPanel()
    {
        gameObject.SetActive(true);
    }
}