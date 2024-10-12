using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace TurryWoods
{
    public class QuestManager : MonoBehaviour
    {
        public Quest [] quests;

        public void Awake()
        {
            LoadQuestFromDB();
        }

        public void LoadQuestFromDB()
        {
            using (StreamReader reader = new StreamReader("C:/Users/Tejas/Documents/GitHub/SemProject2/Assets/Main/DB/QuestDB.json"))
            {
                string json = reader.ReadToEnd();
                quests = JsonUtility.FromJson<Quest[]>(json);
            }
        }
    }
}