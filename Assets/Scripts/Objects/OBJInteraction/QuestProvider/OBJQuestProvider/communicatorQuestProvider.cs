using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class communicatorQuestProvider : BaseQuestProvider
{
    // Public Fields
    public string questName = "Find the Mechanic";
    public string questDescription = "See the Red & Blue Beacon. Go to the top of hill";

    public Vector3 targetPosition = new Vector3(324f, 151f, 201f);
    public Vector3 zoneSize = new Vector3(5f, 1f, 5f);


    // Mission Name & Description
    protected override string QuestName => questName;
    protected override string QuestDescription => questDescription;


    // Target Position & Size
    protected override Vector3 TargetPosition => targetPosition;
    protected override Vector3 ZoneSize => zoneSize;
}

