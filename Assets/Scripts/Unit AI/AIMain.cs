using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIMain : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private Rigidbody rb;

    public Path path;

    //Movement State 
    public float maxSpeed = 12f;
    float maxReverseSpeed = 6f;
    float accelerate = 5f;
    public float turnSpeed = 120f;
    [SerializeField] float currentTurnSpeed;
    [SerializeField] Vector2 movement;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;
    private float distanceToWaypoint;

    [SerializeField]
    private float breakDistance = 30f;

    [SerializeField]
    private float reCalPathRate = .5f;

    [SerializeField]
    private float currentTime = 0f;
    [SerializeField]
    private float distance;
    [SerializeField]
    private LayerMask layer;
    public bool reachedEndOfPath;

    public void Start () {
        //seek player every 2 second
        //InvokeRepeating("SeekPlayer",0f,2f);
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void CalPath(){
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void Update () {
        if(targetPosition == null){
            return;
        }
        Rotate();

    }

    void Rotate(){
        CalTurnSpeed();
        float turn = currentTurnSpeed;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void CalTurnSpeed(){
        /*Vector3 direction = (targetPosition.position-transform.position).normalized;
        float dotProd = Vector3.Dot(direction,transform.forward);*/
        movement.x = 1f;
        float absSpeed = 1f;
        if(Mathf.Abs(movement.y) != 0f){
            absSpeed = Mathf.Abs(movement.y);
        }
        currentTurnSpeed = movement.x * turnSpeed *  absSpeed * Time.deltaTime;
    }

    void MoveUnit(float distanceToWaypoint){
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        //Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = rb.transform.forward * accelerate * speedFactor;
        rb.AddForce(velocity);
    }

    void RotateUnit(Vector3 rotateionDirection){
        rotateionDirection = Vector3.ProjectOnPlane(rotateionDirection, transform.up);
        rb.MoveRotation(Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(rotateionDirection, transform.up),turnSpeed * Time.deltaTime));
    }

    void SeekPlayer(){ //single target
        if(targetPosition != null){
            return;
        }
        GameObject tf = GameObject.FindGameObjectWithTag("Player");
        if(tf != null){
            targetPosition = tf.gameObject.transform;
        }
    }

    bool haveObsticle(){
        if(Physics.Raycast(transform.position,targetPosition.position,Mathf.Infinity,layer)){
            Debug.DrawLine(targetPosition.position,transform.position);
            return true;
        }else{
            return false;
        }
    }
}
