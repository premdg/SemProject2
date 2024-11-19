using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
namespace TurryWoods
{
    public class DialogManager : MonoBehaviour
    {
        public bool HasActiveDialog { get { return m_ActiveDialog != null; } }
        public float DialogDistance
            {
                get
                {   
                    return Vector3.Distance(
                    m_Player.transform.position,
                    m_Npc.transform.position);
                }
            }
        public float timeToShowOptions = 2.0f;            
        public GameObject dialogUI;
        public float maxDialogDistance;
        public TMP_Text dialogHeaderText;
        public TMP_Text dialogWelcomeText;
        public GameObject dialogOptionList;
        public Button Optionsbutton;
        private PlayerInput m_Player;
        private GameObject m_Npc;
        private Dialog m_ActiveDialog;
        private float m_optionTopPosition;
        private float m_timerToShowOptions = 2.0f;
        private bool m_ForcedDialogQuit;

        const float c_DistBtwOptions = 32.0f;

        private void Start()
        {
            m_Player = PlayerInput.Instance;
        }
        
        private void Update()
        {
            if(!HasActiveDialog && m_Player.OptionClickTarget != null)
            {
                if(m_Player.OptionClickTarget.CompareTag("QuestGiverNpc"))
                {
                    m_Npc = m_Player.OptionClickTarget.gameObject;
                    
                    if(DialogDistance < maxDialogDistance)
                    {
                        StartDialog();
                    }
                }   
            }
            if(HasActiveDialog && DialogDistance > maxDialogDistance + 1.0f)
            {
                StopDialog();
            }
            if(m_timerToShowOptions > 0)
            {
                m_timerToShowOptions += Time.deltaTime;
                if(m_timerToShowOptions >= timeToShowOptions)
                {
                    m_timerToShowOptions = 0;
                    if(m_ForcedDialogQuit)
                    {
                        StopDialog();
                    }
                    else
                    {
                        if(HasActiveDialog)
                        {
                            DisplayDialogOptions();
                        }
                    }                    
                }
            }
        }

        private void StartDialog()
        {
            m_ActiveDialog = m_Npc.GetComponent<QuestGiver>().dialog;
            dialogHeaderText.text = m_Npc.name;
            dialogUI.SetActive(true);
            
            ClearDialogOption();
            DisplayAnswerText(m_ActiveDialog.welcomeText);
            TriggerDialogOptions();
             
        }

        private void DisplayAnswerText(string answerText)
        {
            dialogWelcomeText.gameObject.SetActive(true);
            dialogWelcomeText.text = answerText;
        }
        private void DisplayDialogOptions()
        {
            HideAnswerText();
            CreateDialogMenu();
        }
        private void TriggerDialogOptions()
        {
            m_timerToShowOptions = 0.001f; 
        }

        private void HideAnswerText()
        {
            dialogWelcomeText.gameObject.SetActive(false);
        }

        private void CreateDialogMenu()
        {   
            m_optionTopPosition = 0;
            var querries = Array.FindAll(m_ActiveDialog.queries, querry => !querry.isAsked);

            foreach(var querry in querries)
            {
                m_optionTopPosition += c_DistBtwOptions;
                var dialogOption = CreateDialogOption(querry.text);
                RegOptionClickHandler(dialogOption , querry);
            }
        }

        private Button CreateDialogOption(string optionText)
        {
            Button buttonInstance = Instantiate(Optionsbutton , dialogOptionList.transform);
            buttonInstance.GetComponentInChildren<TMP_Text>().text = optionText;

            RectTransform rt = buttonInstance.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_optionTopPosition, rt.rect.height);
            return buttonInstance;
            
        }
        private void RegOptionClickHandler(Button dialogOption , DialogQuerry querry)
        {
            EventTrigger trigger = dialogOption.gameObject.AddComponent<EventTrigger>();
            var PointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            PointerDown.callback.AddListener((e) =>
            {
                if(!String.IsNullOrEmpty(querry.answer.questID))
                {
                    m_Player.GetComponent<QuestLog>().AddQuest(m_Npc.GetComponent<QuestGiver>().quest);
                }
                if(querry.answer.forcedDialogQuit)
                {
                    m_ForcedDialogQuit = true;
                }
                if(!querry.isAlwaysAsked)
                {
                    querry.isAsked = true;
                }

                ClearDialogOption();
                DisplayAnswerText(querry.answer.text);
                TriggerDialogOptions();
            });
            trigger.triggers.Add(PointerDown);
        }
        private void StopDialog()
        {
            m_Npc = null;
            m_ActiveDialog = null;
            m_timerToShowOptions = 0;
            m_ForcedDialogQuit = false;
            dialogUI.SetActive(false);
        }

        private void ClearDialogOption()
        {
            foreach (Transform child in dialogOptionList.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
