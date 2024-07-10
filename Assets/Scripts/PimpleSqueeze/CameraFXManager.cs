using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VirtualCamTarget
{
    Hand,
    Top,
    EndGame
}

public class CameraFXManager : MonoBehaviour
{
    public CinemachineVirtualCamera topVirtualCam;
    public CinemachineVirtualCamera handVirtualCam;
    public CinemachineVirtualCamera endGameCam;

    public CinemachineVirtualCamera[] virtualCams;

    public Transform focusObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GoToHandCam();
        }
    }

    public void GoToCam(VirtualCamTarget camTargetType, float delay, bool releaseBefore = true)
    {
        //if (releaseBefore)
        //    ReleaseCam();

        if (camTargetType == VirtualCamTarget.Hand)
        {
            Invoke(nameof(GoToHandCam), delay);
        }else if(camTargetType == VirtualCamTarget.Top)
        {
            Invoke(nameof(GoToTopCam), delay);
        }else if(camTargetType == VirtualCamTarget.EndGame)
        {

        }
        //if (releaseAfter)
        //    ReleaseCam();
    }


    // TODO: Berbat kod
    public void GoToHandCam()
    {
        topVirtualCam.gameObject.SetActive(false);
        handVirtualCam.gameObject.SetActive(true);
        endGameCam.gameObject.SetActive(false);
        if (focusObject != null)
        {
            handVirtualCam.transform.parent = focusObject;
        }
    }

    public void GoToTopCam()
    {
        topVirtualCam.gameObject.SetActive(true);
        handVirtualCam.gameObject.SetActive(false);
        endGameCam.gameObject.SetActive(false);
    }

    public void GoToEndGameCam()
    {
        topVirtualCam.gameObject.SetActive(false);
        handVirtualCam.gameObject.SetActive(false);
        endGameCam.gameObject.SetActive(true);
    }


    public void ReleaseCam()
    {
        topVirtualCam.gameObject.SetActive(false);
        handVirtualCam.gameObject.SetActive(false);
    }

    public void SetVirtualCam(int camIndex)
    {
       
    }

}
