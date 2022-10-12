using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody rb;
    Animator animator;
    public PlayerControl PlayerControls;
    private InputAction move;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float maxReverseSpeed = 5f;
    [SerializeField] float accelerate = 5f;
    [SerializeField] float turnSpeed = 45f;
    [SerializeField] float currentTurnSpeed;
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
        if(rb == null){
            rb = gameObject.GetComponent<Rigidbody>();
        }
        animator = gameObject.GetComponent<Animator>();
        //rb.centerOfMass = new Vector3(0f,-0.05f,0f);
    }

    private void Update() {
        HandleInput();
        CalSpeed();
    }

    void HandleInput(){
        movement = move.ReadValue<Vector2>();
    }

    void CalSpeed(){
        if(movement.y > 0f && rb.velocity.magnitude < maxSpeed){
            Debug.Log("forward");
            rb.AddForce(rb.transform.forward * accelerate);
        }else if(movement.y < 0f && rb.velocity.magnitude < maxReverseSpeed){
            Debug.Log("backward");
            rb.AddForce(-rb.transform.forward * accelerate);
        }
    }

    private void FixedUpdate() {
        Rotate();
    }

    void Rotate(){
        CalTurnSpeed();
        float turn = currentTurnSpeed;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void CalTurnSpeed(){
        float absSpeed = 1f;
        if(Mathf.Abs(movement.y) != 0f){
            absSpeed = Mathf.Abs(movement.y);
        }
        currentTurnSpeed = movement.x * turnSpeed *  absSpeed * Time.deltaTime;
    }
}
