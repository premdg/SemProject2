using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods
{
    public partial class Damagable : MonoBehaviour
    {
        public struct DamageMessage//has less behaviour than a normal class//this should have little to no data or no behaviour to it 
        {
            
            public MonoBehaviour damager;
            public int amnt;
            public GameObject damageSource;
        }
    }
}
    
