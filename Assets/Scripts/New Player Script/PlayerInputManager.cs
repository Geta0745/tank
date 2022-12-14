using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TurretSystem))]
[RequireComponent(typeof(MovementSystem))]
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] MovementSystem movementMaster;
    [SerializeField] TurretSystem turretMaster;
    public PlayerControl PlayerControls;
    private InputAction move;
    private InputAction look;
    private InputAction fire;
    private InputAction zoom;
    public LayerMask aimMask;
    // Start is called before the first frame update
    private void Awake() {
        PlayerControls = new PlayerControl();
        cam = GameObject.FindObjectOfType<Camera>();
    }

    private void OnEnable() {
        move = PlayerControls.Player.Move;
        look = PlayerControls.Player.Look;
        fire = PlayerControls.Player.Fire;
        zoom = PlayerControls.Player.Aim;
        move.Enable();
        look.Enable();
        fire.Enable();
        fire.performed += ctx => turretMaster.FireMainGun();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //set movement
        movementMaster.SetMovement(move.ReadValue<Vector2>());

        //manage look position
        Ray ray = cam.ScreenPointToRay(look.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,500f,aimMask))
        {
            turretMaster.SetTarget(hit.point);
        }

        
    }
}
