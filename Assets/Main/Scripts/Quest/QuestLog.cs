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

    [System.Serializable]
    public class AcceptedQuest : Quest
    {
        public QuestStatus status;

        public AcceptedQuest(Quest quest)
        {
            uniqueid = quest.uniqueid;
            title = quest.title;
            description = quest.description;
            experience = quest.experience;
            gold = quest.gold;
            amount = quest.amount;
            targets = quest.targets;
            taklTo = quest.taklTo;
            explore = quest.explore;
            questGiver = quest.questGiver;
            type = quest.type;
            status = QuestStatus.ACTIVE;
        }
    }

    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedQuest> quests = new List<AcceptedQuest>();

        public void AddQuest(Quest quest)
        {
            quests.Add(new AcceptedQuest(quest));
        }
    }
}