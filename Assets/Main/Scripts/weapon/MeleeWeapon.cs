using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TurryWoods
{
    public class MeleeWeapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform rootTransform;
        }
        public int damage=15;
        public AttackPoint [] attackPoints = new AttackPoint[0];

        private bool m_IsAttack = false;
        private Vector3 [] m_OriginalAttackPosition;

        private RaycastHit [] m_RayCasthitcache = new RaycastHit[32];

        void FixedUpdate()
        {
            if (m_IsAttack)
            {
                for(int i = 0;i<attackPoints.Length;i++)
                {
                    AttackPoint ap = attackPoints[i];
                    Vector3 worldPos = ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);
                    Vector3 attackVector = (worldPos - m_OriginalAttackPosition[i]).normalized;

                    Ray ray = new Ray(worldPos, attackVector);
                    Debug.DrawRay(worldPos,attackVector, Color.red , 4.0f);

                    int contacts = Physics.SphereCastNonAlloc(ray,ap.radius,m_RayCasthitcache,attackVector.magnitude,~0,QueryTriggerInteraction.Ignore);
                    Debug.Log(contacts);

                    for(int c = 0; c < contacts; c++)
                    {
                        Collider collider = m_RayCasthitcache[c].collider;

                        if (collider != null)
                        {
                            Debug.Log("Hit");
                        }
                    }
                    m_OriginalAttackPosition[0] = worldPos;
                }
            }
        }
        public void BeginAttack()
        {
            m_IsAttack = true;
            m_OriginalAttackPosition = new Vector3 [attackPoints.Length];
            for (int i = 0;i<attackPoints.Length;i++)
            {
                AttackPoint ap=attackPoints[i];
                m_OriginalAttackPosition[i] = ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset);
            }
        }

        public void EndAttack()
        {
            m_IsAttack = false;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0;i<attackPoints.Length;i++)
            {

            }
            foreach(AttackPoint attackPoint in attackPoints)
            {
                if(attackPoint.rootTransform !=null)
                {
                    Vector3 worldPosition = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                    Gizmos.color = new Color(1f, 0.92f, 0.016f, 1f);
                    Gizmos.DrawWireSphere(attackPoint.rootTransform.position + worldPosition, attackPoint.radius);
                }
            }
        }
#endif
    }    
}