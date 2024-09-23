using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace TurryWoods
{
    public class Bandit : MonoBehaviour
{
    public PlayerScanner playerScanner;
    public float timeToStopPursuit = 2.0f;
    public float timeToWaitOnPursuit=2.0f;
    private PlayerController m_Target;

    private EnemyController m_EnemyController;
    private Animator animator;

    private float m_Timesincelosttarget = 0f;

    private Vector3 m_OriginalPosition;

    private readonly int m_HashInPursuit = Animator.StringToHash("inPursuit");
    private readonly int m_HashInOrigin = Animator.StringToHash("NearBase");

    void Awake()
    {
        m_EnemyController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        m_OriginalPosition=transform.position;
    }
    void Update()
    {
        var target = playerScanner.Detect(transform);

        if (m_Target == null)
        {
            if(target != null)
            {
                m_Target = target;
            }

        }
        else
        {
            m_EnemyController.SetFollowtarget(m_Target.transform.position);
            animator.SetBool(m_HashInPursuit,true);
            if(target == null)
            {
                m_Timesincelosttarget += Time.deltaTime;

                if(m_Timesincelosttarget>= timeToStopPursuit)
                {
                    m_Target = null;
                    animator.SetBool(m_HashInPursuit,false);
                    StartCoroutine(WaitOnPursuit());
                }
            }
            else
            {
                m_Timesincelosttarget = 0;
            }
        }
        Vector3 toBase = m_OriginalPosition - transform.position;
        toBase.y=0;

        

        animator.SetBool(m_HashInOrigin, toBase.magnitude < playerScanner.detectionRadius);

        //Debug.Log(toBase.magnitude);
    }

    private IEnumerator WaitOnPursuit()
    {
        yield return new WaitForSeconds(timeToWaitOnPursuit);
        m_EnemyController.SetFollowtarget(m_OriginalPosition);
    }
   
#if UNITY_EDITOR//this method will not be part of the build but only for debugging purposes
    private void OnDrawGizmosSelected()
    {
        Color c = new Color(0,0,0.7f,0.4f);
        UnityEditor.Handles.color = c;

        Vector3 rotateForward = Quaternion.Euler(0,-playerScanner.detectionAngle*0.5f,0) *transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position,Vector3.up,rotateForward,playerScanner.detectionAngle,playerScanner.detectionRadius);
    }
#endif
}

}
