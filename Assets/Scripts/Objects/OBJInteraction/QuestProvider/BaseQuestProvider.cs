using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseQuestProvider : MonoBehaviour, IQuestProvider
{
    // Quest Name & Description
    protected abstract string QuestName { get; }
    protected abstract string QuestDescription { get; }

    // Target Position & Size
    protected virtual Vector3 TargetPosition => new Vector3(324f, 151f, 201f);
    protected virtual Vector3 ZoneSize => new Vector3(5f, 1f, 5f);

    // Check if quest has been created
    private bool questAlreadyCreated = false;

    // Check if a quest can be created
    public bool CanCreateQuest()
    {
        return !questAlreadyCreated;
    }

    // Create Quest
    public void CreateQuest()
    {
        if (questAlreadyCreated)
        {
            Debug.Log("The quest for " + QuestName + " has already been created.");
            return;
        }

        // Create new quest and add target zone
        QuestTargetCreator.CreateTargetZone(QuestName, QuestDescription, TargetPosition, ZoneSize);

        // Mark quest as created
        questAlreadyCreated = true;

        Debug.Log("New quest added: " + QuestName);
    }
}

