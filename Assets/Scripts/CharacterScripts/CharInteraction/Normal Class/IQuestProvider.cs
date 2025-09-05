using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestProvider
{
    bool CanCreateQuest();
    void CreateQuest();
}
