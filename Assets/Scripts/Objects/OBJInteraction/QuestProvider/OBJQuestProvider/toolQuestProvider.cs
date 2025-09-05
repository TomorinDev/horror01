using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolQuestProvider : BaseQuestProvider
{
    // Public Fields
    public string questName = "Find The Broken Pulse Machine";
    public string questDescription = "Mechanic Will Come To Fix. But you shall find it first!";

    public Vector3 targetPosition = new Vector3(324f, 151f, 201f);
    public Vector3 zoneSize = new Vector3(5f, 1f, 5f);

    // Mission Name & Description
    protected override string QuestName => questName;
    protected override string QuestDescription => questDescription;


    // Target Position & Size
    protected override Vector3 TargetPosition => targetPosition;
    protected override Vector3 ZoneSize => zoneSize;
}

