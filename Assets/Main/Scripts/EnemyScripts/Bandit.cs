using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace TurryWoods
{
    public class Bandit : MonoBehaviour, IMessageReceiver, IAttackAnimListener
    {
        public PlayerScanner playerScanner;
        public MeleeWeapon meleeWeapon;
        public float timeToStopPursuit = 2.0f;
        public float timeToWaitOnPursuit = 2.0f;
        public float attackDistance;
        public bool HasFollowTarget
        {
            get
            {
                return m_FollowTarget != null;
            }
        }
        private PlayerController m_FollowTarget;

        private EnemyController m_EnemyController;


        private float m_Timesincelosttarget = 0f;

        private Vector3 m_OriginalPosition;

        private readonly int m_HashInPursuit = Animator.StringToHash("inPursuit");
        private readonly int m_HashInOrigin = Animator.StringToHash("NearBase");
        private readonly int m_HashAttack = Animator.StringToHash("Attack");
        private readonly int m_HashHurt = Animator.StringToHash("Hurt");
        private readonly int m_HashDead = Animator.StringToHash("Dead");



        private void Start()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_OriginalPosition = transform.position;
            meleeWeapon.SetOwner(gameObject);

            //if(PlayerController.Instance == null) Debug.Log("null");
            meleeWeapon.SetTargetLayer(1 << PlayerController.Instance.gameObject.layer);

            //Debug.Log("lydaia");
        }
        private void Update()
        {
            if(PlayerController.Instance.IsRespawning)
            {
                GotoOriginalpos();
                CheckIfNearBase();
                return;
            }
            GuardPosition();
        }

        public void GuardPosition()
        {
            var detectedTarget = playerScanner.Detect(transform);
            bool hasDetectedTarget = detectedTarget != null;
            if (hasDetectedTarget)
            {
                m_FollowTarget = detectedTarget;
            }
            if (HasFollowTarget)
            {
                AttackOrFollowTarget();
                if (hasDetectedTarget)
                {
                    m_Timesincelosttarget = 0;
                }
                else
                {
                    StopPursuit();
                }
            }

            CheckIfNearBase();
        }

        public void OnRecieveMessage(IMessageReceiver.MessageType type, object sender, object message)
        {
            switch (type)
            {
                case IMessageReceiver.MessageType.DEAD:
                    OnDead();
                    break;
                case IMessageReceiver.MessageType.DAMAGED:
                    OnRecieveDamage();
                    break;
                default:
                    break;
            }
        }

        private void OnRecieveDamage()
        {
            m_EnemyController.Animator.SetTrigger(m_HashHurt);
        }

        private void GotoOriginalpos()
        {
            m_FollowTarget = null;
            m_EnemyController.Animator.SetBool(m_HashInPursuit , false);
            m_EnemyController.Followtarget(m_OriginalPosition);
        }

        private void OnDead()
        {
            m_EnemyController.Animator.SetTrigger(m_HashDead);
        }
        private void AttackOrFollowTarget()
        {
            Vector3 toTarget = m_FollowTarget.transform.position - transform.position;
            if (toTarget.magnitude <= attackDistance)
            {
                transform.rotation = Quaternion.LookRotation(toTarget);
                m_EnemyController.StopFollowTarget();
                m_EnemyController.Animator.SetTrigger(m_HashAttack);
            }
            else
            {
                m_EnemyController.Animator.SetBool(m_HashInPursuit, true);
                m_EnemyController.Followtarget(m_FollowTarget.transform.position);
            }
        }
        private void StopPursuit()
        {
            m_Timesincelosttarget += Time.deltaTime;

            if (m_Timesincelosttarget >= timeToStopPursuit)
            {
                m_FollowTarget = null;
                m_EnemyController.Animator.SetBool(m_HashInPursuit, false);
                StartCoroutine(WaitOnPursuit());
            }
        }
        private void CheckIfNearBase()
        {
            Vector3 toBase = m_OriginalPosition - transform.position;
            toBase.y = 0;
            m_EnemyController.Animator.SetBool(m_HashInOrigin, toBase.magnitude < playerScanner.detectionRadius);
        }

        private IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            m_EnemyController.Followtarget(m_OriginalPosition);
        }
        public void MeleeAttackStart()
        {
            Debug.Log("Start");
            meleeWeapon.BeginAttack();
        }

        public void MeleeAttackEnd()
        {
            Debug.Log("End");
            meleeWeapon.EndAttack();
        }

#if UNITY_EDITOR//this method will not be part of the build but only for debugging purposes
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotateForward = Quaternion.Euler(0, -playerScanner.detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, playerScanner.detectionAngle, playerScanner.detectionRadius);
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, 360, playerScanner.meleeDetectionRadius);

        }
#endif
    }

}
