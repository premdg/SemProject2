using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods
{
    public partial class  Damagable : MonoBehaviour
{
    [Range(0,360.0f)]
    public float hitAngle = 360.0f;
    public int maxHitPoints;
    public int experience;
    public int currentHitPoints {get ; private set; }
    public float invulnarabilityTime = 0.5f;
    public LayerMask playerActionReceicers;
    public List<MonoBehaviour> onDamageMessageReceivers;

    private bool m_Isvulnarable = false;   
    private float m_TimeSinceLastHit = 0.0f;

    public void Awake()
    {
        currentHitPoints = maxHitPoints;
        if(0 != (playerActionReceicers.value & 1 << gameObject.layer))
        {
            onDamageMessageReceivers.Add(FindObjectOfType<QuestManager>());
            onDamageMessageReceivers.Add(FindObjectOfType<PlayerStats>());
        }
        
    }
    public void Update()
    {
        if (m_Isvulnarable)
        {
            m_TimeSinceLastHit += Time.deltaTime;

            if(m_TimeSinceLastHit >= invulnarabilityTime)
            {
                m_Isvulnarable = false;
                m_TimeSinceLastHit = 0.0f; 
            }
        }
    }
    public void ApplyDamage(DamageMessage data)
    {
        if (currentHitPoints <=0 || m_Isvulnarable)
        {
            return;
        }

        Vector3 positionToDamager = data.damageSource.transform.position - transform.position;
        positionToDamager.y = 0;

        if(Vector3.Angle(transform.position , positionToDamager) > hitAngle * 0.5f)
        {
            return;
        } 
        m_Isvulnarable = true;
        currentHitPoints -= data.amnt;  
        var messageType = currentHitPoints <= 0 ? IMessageReceiver.MessageType.DEAD : IMessageReceiver.MessageType.DAMAGED;

        for (int i = 0 ; i < onDamageMessageReceivers.Count ; i++)
        {
            var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
            receiver.OnRecieveMessage(messageType , this, data);
           
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color(0.0f,0.0f,1.0f,0.5f);
        Vector3 rotatedForward = Quaternion.AngleAxis(-hitAngle * 0.5f,transform.up) * transform.forward;

        UnityEditor.Handles.DrawSolidArc(transform.position,transform.up,rotatedForward,hitAngle,1.0f);
    }
#endif
}

}
