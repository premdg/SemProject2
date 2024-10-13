using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TurryWoods
{
    public enum QuestStatus
    {
        ACTIVE,
        FAILED,
        COMPLETED
    }
    public class AcceptedClass : Quest
    {
        public QuestStatus questStatus;
    }
    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedClass> quests;
    }
}