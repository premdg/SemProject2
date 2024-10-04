using System.Collections;
using System.Collections.Generic;
using TurryWoods;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour , IAttackAnimListener
{
    private NavMeshAgent m_NavmeshAgent;
    private Animator m_Animator;
    private float m_SpeedModifier = 0.7f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnAnimatorMove()
    {
        if (m_NavmeshAgent.enabled)
        {
            m_NavmeshAgent.speed = (m_Animator.deltaPosition / Time.fixedDeltaTime).magnitude * m_SpeedModifier;
        }

       
    }

    public bool Followtarget(Vector3 position)
    {
        if(!m_NavmeshAgent.enabled)
        {
            m_NavmeshAgent.enabled = true;
        }
        return m_NavmeshAgent.SetDestination(position);
    }

    public void StopFollowTarget()
{
    m_NavmeshAgent.enabled = false;
}

    public void MeleeAttackStart()
    {
        
    }

    public void MeleeAttackEnd()
    {
        
    }
}
