using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TurryWoods
{
    public class PlayerStats : MonoBehaviour , IMessageReceiver
{
    public int currentExp;
    public int maxLvl;
    public int currentLevel;
    public int[] availableLvl;

    public int ExpToNextLvl
    {
        get { return availableLvl[currentLevel] - currentExp; }
    }

    public void Awake()
    {
        availableLvl = new int[maxLvl];
        ComputeLVL(maxLvl);
    }

        

        private void ComputeLVL(int levelCount)
    {
        for(int i = 0 ; i < levelCount ; i++)
        {
            var level = i + 1;
            var levelPow = Mathf.Pow(level , 2);
            var ExpToLevel = Convert.ToInt32(levelPow*levelCount);
            availableLvl[i] = ExpToLevel;
        }
    }
        public void OnRecieveMessage(IMessageReceiver.MessageType type, object sender, object message)
        {
            if (type == IMessageReceiver.MessageType.DEAD)
            {
                GainExp((sender as Damagable).experience);
            }
            
        }

        public void GainExp(int gainedExp)
        {
            if(gainedExp > ExpToNextLvl)
            {
                var RemainderExp = gainedExp - ExpToNextLvl;
                currentExp = 0;
                currentLevel++;
                GainExp(RemainderExp);
            }
            else
            {
                if(gainedExp == ExpToNextLvl)
                {
                    currentLevel++;
                    currentExp = 0;
                }else
                {
                    currentExp += gainedExp;
                }
            }
        }
}
}

