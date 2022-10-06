using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody tankRigidbody;
    Animator animator;
    public PlayerControl PlayerControls;
    private InputAction move;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float maxReverseSpeed = -3.5f;
    [SerializeField] float bakeForce =  1f;
    [SerializeField] float currentSpeed;
    [SerializeField] float accelerate = .1f;
    [SerializeField] float turnSpeed;
    [SerializeField]
    Vector2 movement;

    private void Awake() {
        PlayerControls = new PlayerControl();
    }

    private void OnEnable() {
        move = PlayerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable() {
        move.Disable();
    }

    private void Start() {
        if(tankRigidbody == null){
            tankRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        animator = gameObject.GetComponent<Animator>();
        tankRigidbody.centerOfMass = new Vector3(0f,-0.05f,0f);
    }

    private void Update() {
        HandleInput();
        CalSpeed();
    }

    void HandleInput(){
        movement = move.ReadValue<Vector2>();
    }

    void CalSpeed(){
        if(movement.y > 0f){
            currentSpeed = Mathf.Lerp(currentSpeed,maxSpeed * movement.y,accelerate * Mathf.Abs(movement.y));
        }else if(movement.y < 0f){
            currentSpeed = Mathf.Lerp(currentSpeed,maxReverseSpeed,accelerate * Mathf.Abs(movement.y));
        }else{
            currentSpeed = Mathf.Lerp(currentSpeed,0f,bakeForce);
        }
    }

    private void FixedUpdate() {
        Vector3 movement_ = transform.forward * currentSpeed * Time.deltaTime;
        tankRigidbody.MovePosition(tankRigidbody.position + movement_);
        float turn = movement.x * turnSpeed  * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        tankRigidbody.MoveRotation(tankRigidbody.rotation * turnRotation);
    }
    
}
