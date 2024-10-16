using UnityEngine;
using System.Collections.Generic;
using System;

namespace TurryWoods
{
    [System.Serializable]
    public class DialogAnswer
    {
        [TextArea(3,9)]
        public string text;
        public bool forcedDialogQuit;
        public string questID;
    }

    [System.Serializable]
    public class DialogQuerry
    {
        [TextArea(3,9)]
        public string text;
        public DialogAnswer answer;
        public bool isAsked;
        public bool isAlwaysAsked;
    }            
    
    
    [System.Serializable]
    public class Dialog
    {
        [TextArea(3,9)]
        public string welcomeText;
        public DialogQuerry [] queries;
    }
}