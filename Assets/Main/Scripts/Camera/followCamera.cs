using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    [SerializeField] Transform lookAt;

    // Update is called once per frame
    void LateUpdate()
    {
        if(!lookAt)
        {
            return;
        }

        float currentRotationAngle=transform.eulerAngles.y;
        float wantedRotationAngle=lookAt.eulerAngles.y;

        //a+(b-a)*t = interpolated value
        // currentRotationAngle + (wantedRotationAngle - currentRotationAngle)* time.Deltatime;

        currentRotationAngle=Mathf.LerpAngle(currentRotationAngle,wantedRotationAngle,0.5f);
        transform.position = new Vector3(lookAt.position.x,5.0f,lookAt.position.z);

        Quaternion currentRotation=Quaternion.Euler(0,currentRotationAngle,0);
       
        Vector3 rotatedPosition = currentRotation*Vector3.forward;

        transform.position-=rotatedPosition *10;

        transform.LookAt(lookAt);
    }
}
