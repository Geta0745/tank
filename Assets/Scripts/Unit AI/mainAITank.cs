using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class mainAITank : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private Rigidbody rb;

    public Path path;

    public float speed = 2;
    public float turnSpeed = 120;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;
    [SerializeField]
    private float breakDistance = 30f;
    public float reCalPathRate = .5f;
    [SerializeField]
    private float currentTime = 0f;

    public bool reachedEndOfPath;

    public void Start () {
        seeker = GetComponent<Seeker>();
        // If you are writing a 2D game you should remove this line
        // and use the alternative way to move sugggested further below.
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f,-0.05f,0f);
        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete (Path p) {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    void CalPath(){
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void Update () {
        float distance = Vector3.Distance(transform.position,targetPosition.position);
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }
        if(currentTime >= reCalPathRate){
            currentTime = 0f;
            CalPath();
        }else{
            currentTime += Time.deltaTime;
        }
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        float dotProd = Vector3.Dot(direction,transform.forward);
        /*if(dotProd > .99f && !reachedEndOfPath){
            Move(distanceToWaypoint);
        }else{
            Vector3 targetDir = targetPosition.position - transform.position;
            targetDir = Vector3.ProjectOnPlane(targetDir, transform.up);
            rb.MoveRotation(Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(direction, transform.up),turnSpeed * Time.deltaTime));
        }*/
        /*
        Logic Here ----------------------
        if endPoint AND distance from AI to player < breakDistance AND facing to path node 
        
        
        
        
        
        */
        if(!reachedEndOfPath && !(distance <= breakDistance ) ){
            if(dotProd > .995f){
                Move(distanceToWaypoint);
            }
            Vector3 targetDir = targetPosition.position - transform.position;
            targetDir = Vector3.ProjectOnPlane(targetDir, transform.up);
            rb.MoveRotation(Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(direction, transform.up),turnSpeed * Time.deltaTime));
            Debug.DrawLine(transform.position,path.vectorPath[currentWaypoint]);
        }else{
            Vector3 roDir = targetPosition.position - transform.position;
            roDir = Vector3.ProjectOnPlane(roDir, transform.up);
            rb.MoveRotation(Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(direction, transform.up),turnSpeed * Time.deltaTime));
        }
    }

    void Move(float distanceToWaypoint){
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        //Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = transform.forward * speed * speedFactor;
        rb.MovePosition(rb.position + velocity);
    }
}
