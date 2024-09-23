using System.Collections;
using System.Collections.Generic;
using TurryWoods;
using UnityEngine;

[System.Serializable]
public class PlayerScanner 
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;
    public PlayerController Detect(Transform detector)
    {
        if (PlayerController.Instance == null)
        {
            return null;
        }

        Vector3 enemyPosition = detector.position;
        Vector3 PlayerPos = PlayerController.Instance.transform.position-detector.position;
        PlayerPos.y=0;

        if(PlayerPos.magnitude <=detectionRadius)
        {
            if(Vector3.Dot(PlayerPos.normalized,detector.forward)>Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                return PlayerController.Instance;

            }
        }return null;
    }
   
}
