using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using TMPro;
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
        public GameObject dialogUI;
        public float maxDialogDistance;

        public TMP_Text NPCName;
        private PlayerInput m_Player;
        private GameObject m_Npc;
        private Dialog m_ActiveDialog;

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
        }

        private void StartDialog()
        {
            m_ActiveDialog = m_Npc.GetComponent<QuestGiver>().dialog;
            dialogUI.SetActive(true);
            NPCName.text = m_Npc.name;
        }
        private void StopDialog()
        {
            m_Npc = null;
            m_ActiveDialog = null;
            dialogUI.SetActive(false);
        }
    }

}
