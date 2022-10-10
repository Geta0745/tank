using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerFireController : MonoBehaviour
{
    Camera cam;
    PlayerMainTurret maingun;
    PlayerControl PlayerControls;
    public LayerMask aimMask;
    public LayerMask muzzleMask;
    public Vector3 aimPoint;
    public bool AimMode = false;
    private InputAction look;
    private InputAction fire;
    private InputAction zoom;
    //Zoom
    //Zoom Aim
    [SerializeField] GameObject VirtualAimPoint;
    CinemachineTargetGroup targetbrain;
    [SerializeField] float deZoomSpeed = 5f;
    private void Awake() {
        PlayerControls = new PlayerControl();
    }

    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>();
        targetbrain = GameObject.FindObjectOfType<CinemachineTargetGroup>().GetComponent<CinemachineTargetGroup>();
        maingun = gameObject.GetComponent<PlayerMainTurret>();
        VirtualAimPoint = new GameObject("AimPoint");
        targetbrain.AddMember(VirtualAimPoint.transform,.6f,0f);
    }

    private void OnEnable() {
        look = PlayerControls.Player.Look;
        fire = PlayerControls.Player.Fire;
        zoom = PlayerControls.Player.Aim;
        zoom.Enable();
        zoom.performed += ctx => ChangeAimMode();
        look.Enable();
        fire.Enable();
        fire.performed += ctx => maingun.FireMainGun();
    }

    void ChangeAimMode(){
        AimMode = !AimMode;
    }

    private void OnDisable() {
        fire.Disable();
        zoom.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(look.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,500f,aimMask))
        {
            aimPoint = hit.point;
        }

        float distance = Vector3.Distance(transform.position,VirtualAimPoint.transform.position);
        if(AimMode){
            VirtualAimPoint.transform.position = hit.point;
        }else{
            VirtualAimPoint.transform.position = Vector3.Lerp(VirtualAimPoint.transform.position,transform.position,deZoomSpeed * Time.deltaTime);
        }
    }
}
