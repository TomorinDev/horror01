using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    // Dynamic Name
    public string questToComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Quest quest = questManager.instance.quests.Find(q => q.questName == questToComplete);
            if (quest != null && !quest.isCompleted)
            {
                questManager.instance.CompleteQuest(quest); // Complete Quest
                Debug.Log("Quest completed: " + questToComplete);

                // Destroy
                Destroy(gameObject);
            }
        }
    }
}
