using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

[ExecuteInEditMode]
public class ActiveCinemachineCam : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera[] virtualCameras ;

    private double lastDirectorTime;

    private void Start()
    {
        if (playableDirector == null)
        {
            Debug.LogError("PlayableDirector not assigned.");
            return;
        }
        virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>();
        lastDirectorTime = playableDirector.time;
    }

    private void Update()
    {
        if (lastDirectorTime != playableDirector.time)
        {
            UpdateActiveCamera();
            lastDirectorTime = playableDirector.time;
        }
    }

    private void UpdateActiveCamera()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain == null) return;

        CinemachineVirtualCamera activeCamera = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (activeCamera == null) return;

        foreach (CinemachineVirtualCamera cam in virtualCameras)
        {
            cam.gameObject.SetActive(cam == activeCamera);
        }
    }
}
