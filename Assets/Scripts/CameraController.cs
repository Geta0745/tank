using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera fpsCam;
    [SerializeField] CinemachineVirtualCamera obitalCam;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        SetTargetMainCam(player.transform);
        SetFollowCamTarget(player.transform);
        SwitchCam();
    }

    private void Update() {
        if(player != null){
            SetTargetMainCam(player.transform);
            SetFollowCamTarget(player.transform);
        }else{
            DisableAllCam();
        }
    }

    public void DisableAllCam(){
        fpsCam.gameObject.SetActive(false);
        obitalCam.gameObject.SetActive(false);
    }
    public void ResetUp(Transform target){
        fpsCam.gameObject.SetActive(false);
        obitalCam.gameObject.SetActive(true);
        if(target != null){
            fpsCam.Follow = target;
            fpsCam.LookAt = target;
            obitalCam.Follow = target;
            obitalCam.LookAt = target;
            player = target.gameObject;
        }
    }

    public void SwitchCam(){
        fpsCam.gameObject.SetActive(!fpsCam.gameObject.activeSelf);
        obitalCam.gameObject.SetActive(!obitalCam.gameObject.activeSelf);
    }

    public void SetTargetMainCam(Transform target){
        if(target != null){
            fpsCam.Follow = target;
            fpsCam.LookAt = target;
        }
    }

    public void SetFollowCamTarget(Transform target){
        if(target != null){
            obitalCam.Follow = target;
            obitalCam.LookAt = target;
        }
    }
}

/*
cam1 setactive !cam1.activeself
cam2 setactive !cam2. activeself
*/