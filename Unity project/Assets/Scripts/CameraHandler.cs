using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private enum CamStates { free, locked };
    private CamStates currentState;

    public Cinemachine.CinemachineFreeLook freelookCam;
    public Cinemachine.CinemachineVirtualCamera lockOnCam;

    void Start()
    {
        currentState = CamStates.free;
    }

    void Update()
    {
        if (Input.GetKeyDown("left alt"))
        {
            if (currentState == CamStates.free && lockOnCam.m_LookAt != null)
            {
                currentState = CamStates.locked;
            }
            else
            {
                currentState = CamStates.free;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case CamStates.free: freeCam(); break;
            case CamStates.locked: lockedCam(); break;
            default: break;
        }
    }

    void freeCam()
    {
        lockOnCam.enabled = false;
        freelookCam.enabled = true;
    }

    void lockedCam()
    {
        lockOnCam.enabled = true;
        freelookCam.enabled = false;
    }

    public bool isFreeCam()
    {
        if (currentState == CamStates.free)
        {
            return true;
        }
        else
            return false;
    }
}
