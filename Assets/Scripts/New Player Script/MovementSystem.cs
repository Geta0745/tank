using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float backwardSpeed = 4f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float acceleration = .0008f;
    [SerializeField] private float deceleration = .005f;
    [SerializeField] private float currentSpeed;
    private Rigidbody rb;
    [SerializeField] private Vector2 movement;
    [SerializeField] float detectGroundDistance = 1f;
    public bool moveable = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /*
    movement.y = move forward and backward
    movement.x = turn tank
    */
    private void Update()
    {
        bool groundHit = Physics.Raycast(transform.position,-transform.up,detectGroundDistance);
        // Accelerate or decelerate based on input
        if (movement.y > 0 && moveable && groundHit)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, acceleration * Mathf.Abs(movement.y));
        }
        else if (movement.y == 0 || !moveable || !groundHit)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration);
        }
        else if (movement.y < 0 && moveable && groundHit)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, -backwardSpeed, deceleration * Mathf.Abs(movement.y));
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position,transform.position-transform.up* detectGroundDistance);
        Vector3 moveDir = transform.forward * currentSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0f, movement.x * rotationSpeed * Time.deltaTime, 0f);
        rb.MovePosition(rb.position + moveDir);
        if (moveable && Physics.Raycast(transform.position,-transform.up,detectGroundDistance))
        {
            rb.MoveRotation(rb.rotation * rotation);
        }
    }

    public void SetMovement(Vector2 movement)
    {
        this.movement = movement;
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer != LayerMask.NameToLayer("german projectile") && other.gameObject.layer != LayerMask.NameToLayer("soviet projectile") && other.gameObject.layer != LayerMask.NameToLayer("none obstacle")){
            currentSpeed = 0f;
        }
    }
}
