using UnityEngine;
using System.IO;
using System;
namespace TurryWoods
{
    public class JsonHelper
    {
        private class Wrapper<T>
        {
            public T[] array;
        }


        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{\"array\" : " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }
    public class QuestManager : MonoBehaviour, IMessageReceiver
    {

        public Quest[] quests;
        private PlayerStats m_PlayerStats;

        private void Awake()
        {
            LoadQuestsFromDB();
            AssignQuests();
            m_PlayerStats = FindObjectOfType<PlayerStats>();
        }

        private void LoadQuestsFromDB()
        {
            using StreamReader reader = new StreamReader("Assets/Main/DB/QuestDB.json");
            string json = reader.ReadToEnd();
            var loadedQuests = JsonHelper.GetJsonArray<Quest>(json);
            quests = new Quest[loadedQuests.Length];
            quests = loadedQuests;
        }

        private void AssignQuests()
        {
            var questGivers = FindObjectsOfType<QuestGiver>();

            if (questGivers != null && questGivers.Length > 0)
            {
                foreach (var questGiver in questGivers)
                {
                    AssignQuestTo(questGiver);
                }
            }
        }
        private void AssignQuestTo(QuestGiver questGiver)
        {
            foreach (var quest in quests)
            {
                if (quest.questGiver == questGiver.GetComponent<UniqueId>().Uid)
                {
                    questGiver.quest = quest;
                }
            }
        }

        public void OnRecieveMessage(IMessageReceiver.MessageType type, object sender, object message)
        {
            if (type == IMessageReceiver.MessageType.DEAD)
            {

                CheckQuestWhenEnemyIsDead((Damagable)sender, (Damagable.DamageMessage)message);

            }
        }
        private void CheckQuestWhenEnemyIsDead(Damagable sender, Damagable.DamageMessage message)
        {

            var questLog = message.damageSource.GetComponent<QuestLog>();
            if (questLog == null) { return; }
            //            Debug.Log(questLog.quests.Count);
            foreach (var quest in questLog.quests)
            {
                if (quest.status == QuestStatus.ACTIVE)
                {
                    if (quest.type == QuestType.HUNT && Array.Exists(quest.targets, (targetUid) => sender.GetComponent<UniqueId>().Uid == targetUid))
                    {
                        quest.amount -= 1;
                        if (quest.amount == 0)
                        {
                            quest.status = QuestStatus.COMPLETED;
                            m_PlayerStats.GainExp(quest.experience);
                        }
                    }
                }

            }
        }
    }
}