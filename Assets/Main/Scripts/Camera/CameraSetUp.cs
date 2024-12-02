using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TurryWoods;
using UnityEngine;

public class CameraSetUp : MonoBehaviour
{
    public CinemachineFreeLook CMFreeLook; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Subhash is pakka gay");
        GameObject player = FindAnyObjectByType<PlayerController>().gameObject;
        Transform LookAt = player.transform.Find("LookAt");
        CMFreeLook.LookAt = LookAt;
        CMFreeLook.Follow = LookAt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
