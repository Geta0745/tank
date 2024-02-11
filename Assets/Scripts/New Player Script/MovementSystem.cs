using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] protected float speed = 8f;
    [SerializeField] protected float backwardSpeed = 4f;
    [SerializeField] protected float rotationSpeed = 20f;
    [SerializeField] protected float acceleration = .0008f;
    [SerializeField] protected float deceleration = .005f;
    [SerializeField] protected float currentSpeed;
    protected Rigidbody rb;
    [SerializeField] protected Vector2 movement;
    [SerializeField] protected float detectGroundDistance = 1f;
    public bool moveable = true;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /*
    movement.y = move forward and backward
    movement.x = turn tank
    */
    protected virtual void Update()
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

    protected virtual void FixedUpdate()
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

    public float GetMaxSpeed(){
        return speed;
    }

    protected virtual void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer != LayerMask.NameToLayer("german projectile") && other.gameObject.layer != LayerMask.NameToLayer("soviet projectile") && other.gameObject.layer != LayerMask.NameToLayer("none obstacle")){
            currentSpeed = 0f;
        }
    }
}
