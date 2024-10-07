using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods{
public class dissolve : MonoBehaviour
{
    public float dissolveTime = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        dissolveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= dissolveTime)
        {
            Destroy(gameObject);
        }
    }
}

}
