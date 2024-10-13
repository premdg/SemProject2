using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 m_Movement;
        private bool m_Attack;

        public float distanceToInteract = 2.0f;

        public Vector3 MoveInput
        {
            get
            {
                return m_Movement;
            }
        }

        public bool IsMoveInput
        {
            get
            {
                return !Mathf.Approximately(MoveInput.magnitude, 0f);
            }
        }
        public bool IsAttacking
        {
            get
            {
                return m_Attack;
            }
        }
        void Update()
        {
            m_Movement.Set(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
            bool isMouseLeftInput = Input.GetMouseButtonDown(0);
            bool isMouseRightInput = Input.GetMouseButtonDown(1);
            if (isMouseLeftInput)
            {
                HandleLeftMouseBtnDown();
            }

            if (isMouseRightInput)
            {
                HandleRightMouseBtnDown(); 
            }
            
        }
        private void HandleLeftMouseBtnDown()
        {
            if(!m_Attack)
                {
                    StartCoroutine(attackAndWait());
                }
        }
        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

                if (hasHit && hit.collider.CompareTag("QuestGiverNpc"))
                {
                    var distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                    if(distanceToTarget <= distanceToInteract)
                    {
                        Debug.Log("Hitting the taget");
                    }
                }
        }

        private IEnumerator attackAndWait()
        {
        m_Attack = true;
        yield return new WaitForSeconds(0.03f);
        m_Attack = false;
        }
    }
}