using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Seeker))]
public class AIUnit : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private Rigidbody rb;

    public Path path;

    public float speed = 1;
    public float turnSpeed = .5f;

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
        InvokeRepeating("SeekPlayer",0f,2f);
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f,-0.05f,0f);
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
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
        distance = Vector3.Distance(transform.position,targetPosition.position);
        if (path != null) {
            if(currentTime >= reCalPathRate){
                currentTime = 0f;
                CalPath();
            }else{
                currentTime += Time.deltaTime;
            }
            reachedEndOfPath = false;
            while (true) {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance) {
                    if (currentWaypoint + 1 < path.vectorPath.Count) {
                        currentWaypoint++;
                    } else {
                        reachedEndOfPath = true;
                        break;
                    }
                } else {
                    break;
                }
            }
        }
        /*
        --check if unit facing to player
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float dotProd = Vector3.Dot(direction,transform.forward);


        */
        /*
        Logic Here ----------------------
        if endPoint AND distance from AI to player < breakDistance AND facing to path node 


        */
        if(!reachedEndOfPath && !(distance <= breakDistance && !haveObsticle()) ){
            MoveUnit(distanceToWaypoint);
            RotateUnit(path.vectorPath[currentWaypoint] - transform.position);
        }else{
            RotateUnit(targetPosition.position - transform.position);
        }
    }

    void MoveUnit(float distanceToWaypoint){
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        //Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = transform.forward * speed * speedFactor;
        rb.MovePosition(rb.position + velocity);
    }

    void RotateUnit(Vector3 rotateionDirection){
        rotateionDirection = Vector3.ProjectOnPlane(rotateionDirection, transform.up);
        rb.MoveRotation(Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(rotateionDirection, transform.up),turnSpeed * Time.deltaTime));
    }

    void SeekPlayer(){ //single target
        if(targetPosition != null){
            return;
        }
        Transform tf = GameObject.FindGameObjectWithTag("Player").transform;
        if(tf != null){
            targetPosition = tf;
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
