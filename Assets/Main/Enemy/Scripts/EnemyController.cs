using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
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
        if (m_NavmeshAgent == null)
        {
            return;
        }

        m_NavmeshAgent.speed = (m_Animator.deltaPosition / Time.fixedDeltaTime).magnitude * m_SpeedModifier;
    }

    public bool SetFollowtarget(Vector3 position)
    {
        return m_NavmeshAgent.SetDestination(position);
    }
}
