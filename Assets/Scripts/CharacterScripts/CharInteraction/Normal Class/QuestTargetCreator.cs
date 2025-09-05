using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTargetCreator
{
    // Create a Target Zone for a Quest
    public static void CreateTargetZone(string questName, string questDescription, Vector3 targetPosition, Vector3 zoneSize)
    {
        // Create New Quest then Add it to the quest manager
        Quest newQuest = new Quest(questName, questDescription);
        questManager.instance.AddQuest(newQuest, targetPosition, zoneSize);

        // Debug Log
        Debug.Log("Target zone created for quest: " + questName);
    }
}

