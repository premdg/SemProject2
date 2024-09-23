using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace TurryWoods
{
public class CameraController: MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCamera;

    public CinemachineFreeLook Playercam
    {
        get 
        {
            return freeLookCamera; 
        }
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 400;
            freeLookCamera.m_YAxis.m_MaxSpeed = 10;
        }
        if(Input.GetMouseButtonUp(1))
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
        
    }
}
}
