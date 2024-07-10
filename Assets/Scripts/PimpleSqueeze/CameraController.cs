using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        Preferences.mainCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
