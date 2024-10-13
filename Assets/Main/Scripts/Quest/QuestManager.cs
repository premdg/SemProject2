using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public class QuestManager : MonoBehaviour //IMessageReceiver
        {
            public Quest[] quests;
            //private PlayerStats m_PlayerStats;

            private void Awake()
            {
                LoadQuestsFromDB();
                AssignQuests();

                //m_PlayerStats = FindObjectOfType<PlayerStats>();
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
        }
}