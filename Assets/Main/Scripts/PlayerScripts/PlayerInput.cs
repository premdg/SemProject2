using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurryWoods
{
    public class PlayerInput : MonoBehaviour
    {
        public static PlayerInput Instance { get { return s_Instance; } }
        public bool playerControllerInputBlocked;
        public static PlayerInput s_Instance;
        public float distanceToInteract = 2.0f;
        private Vector3 m_Movement;
        private bool m_Attack;
        private Collider m_OptionClickTarget;


        public Collider OptionClickTarget { get { return m_OptionClickTarget; } }
        public Vector3 MoveInput 
            { 
                get
                {
                    if(playerControllerInputBlocked)
                    {
                        return Vector3.zero;
                    }
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
                    return !playerControllerInputBlocked && m_Attack; 
                } 
            }

        private void Awake()
        {
            s_Instance = this;
        }
        void Update()
        {
            m_Movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
            if (!m_Attack && !IsPointerOverUI())
            {
                StartCoroutine(triggerAttack());
            }
        }
        private bool IsPointerOverUI()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
            {
                StartCoroutine(triggerOptionTarget(hit.collider));
            }
        }
        private IEnumerator triggerOptionTarget(Collider other)
        {
            m_OptionClickTarget = other;
            yield return new WaitForSeconds(0.03f);
            m_OptionClickTarget = null;
        }
        private IEnumerator triggerAttack()
        {
            m_Attack = true;
            yield return new WaitForSeconds(0.03f);
            m_Attack = false;
        }
    }
}