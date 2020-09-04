using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook freelookCam;
    public Cinemachine.CinemachineVirtualCamera lockOnCam;

    public bool freeLookCamera = true;
    // Update is called once per frame
    void FixedUpdate()
    {
        CameraSwitch();
        if(lockOnCam.m_LookAt == null)
        {
            lockOnCam.enabled = false;
            freelookCam.enabled = true;
            freeLookCamera = true;
        }
    }

    void CameraSwitch()
    {
        if (Input.GetKeyDown("left alt"))
        {
            if (freelookCam.enabled && lockOnCam.m_LookAt != null)
            {
                lockOnCam.enabled = true;
                freelookCam.enabled = false;
                freeLookCamera = false;
            }
            else
            {
                lockOnCam.enabled = false;
                freelookCam.enabled = true;
                freeLookCamera = true;
            }
        }
    }
}
