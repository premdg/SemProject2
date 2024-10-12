using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods
{
    public enum QuestType
    {
        HUNT,
        GATHER,
        TALK,
        EXPLORE
    }
    [System.Serializable]   
    public class Quest 
    {
        public string uniqueid;
        public string title;
        public string description;
        public string [] targets;
        public string taklTo;
        public string questGiver;

        public int experience;
        public int gold;
        public int amount;   
        
        public Vector3 explore;
        public QuestType type;
    }

}
