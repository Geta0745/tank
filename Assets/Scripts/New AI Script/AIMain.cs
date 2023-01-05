using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementSystem))]
public class AIMain : MonoBehaviour
{
    [SerializeField] MovementSystem movementMaster;
    [SerializeField] Transform target;
    [SerializeField] Vector3 currentTagetNode;
    private NavMeshPath path;
    [SerializeField] Vector2 movement;
    [SerializeField] float nextWaypointDistance = 4f;
    private int currentNode = 0;
    [SerializeField] float calPathRate = 10f;
    [Header("Avoid Obstacle")] public LayerMask obstacleMask;
    public Vector3 boxcastHalfExtents = new Vector3(1.0f, 1.0f, 1.0f);
    public float AvoidDistance = 5.0f;
    [Header("Reach")][SerializeField] Vector2 possibleTurn = new Vector2(-3f, 3f);
    RaycastHit hitInfo;
    Rigidbody rb;
    private void Start()
    {
        path = new NavMeshPath();
        rb = GetComponent<Rigidbody>();
        movementMaster = GetComponent<MovementSystem>();
        InvokeRepeating("CalculatePath", 0f, calPathRate);
    }
    void Update()
    {
        if (currentNode < path.corners.Length)
        {
            currentTagetNode = path.corners[currentNode];
            Vector3 direction = currentTagetNode - transform.position;
            float dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position);
            float dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position);
            //if(dotProdFront < 0 && Vector3.Distance(transform.position,path.corners[currentNode]))
            movement = CalMoveDirection(new Vector2(dotProdRear, dotProdFront));
            movement = AvoidanceObstacle();
            movementMaster.SetMovement(movement);
            if (Vector3.Distance(transform.position, currentTagetNode) < nextWaypointDistance)
            {
                currentNode++;
            }
        }
        else
        {
            movement = Vector2.zero;
            movementMaster.SetMovement(movement);
        }

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * (AvoidDistance), Color.green);
    }

    void CalculatePath()
    {
        if (target != null)
        {
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        else
        {
            NavMesh.CalculatePath(transform.position, RandomPositionOnNavmesh(), NavMesh.AllAreas, path);
        }
        currentNode = 0;
        Debug.LogWarning("Calculated Path : " + path.corners.Length);
    }

    Vector3 RandomPositionOnNavmesh()
    {
        // Create a random position within the bounds of the navmesh
        Vector3 randomPos = new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-50.0f, 50.0f));

        // Create a NavMeshHit to store the result of the sampling
        NavMeshHit hit;

        // Sample the navmesh to see if the random position is walkable
        if (NavMesh.SamplePosition(randomPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            // If the position is walkable, return it
            return hit.position;
        }
        else
        {
            // If the position is not walkable, try again with a new random position
            return RandomPositionOnNavmesh();
        }
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    Vector2 AvoidanceObstacle()
    {
        // Perform the Boxcast to check for obstacles
        bool hit = Physics.BoxCast(transform.position, boxcastHalfExtents, transform.forward, out hitInfo, Quaternion.identity, AvoidDistance, obstacleMask);

        // If an obstacle was hit, adjust the movement to avoid the obstacle
        if (hit)
        {
            Vector3 reflectedDirection = Vector3.Reflect(transform.forward, hitInfo.normal);
            Vector2 movement = new Vector2(Mathf.Clamp(Vector2.Dot(-transform.right,path.corners[currentNode]),-1f,1f),Mathf.Clamp(Vector2.Dot(transform.forward,path.corners[currentNode]),-1,1f));
            return movement;
            /*// Calculate the relative position and velocity between the object and the obstacle
            Vector3 obstaclePosition = hitInfo.collider.transform.position;
            Vector3 relativePosition = obstaclePosition - transform.position;
            Vector3 relativeVelocity = Vector3.zero;
            if(hitInfo.collider.GetComponent<Rigidbody>() != null){
                relativeVelocity = hitInfo.collider.GetComponent<Rigidbody>().velocity - rb.velocity;
            }

            // Calculate the time it would take for the two objects to collide
            float timeToCollision = -1 * Vector3.Dot(relativePosition, relativeVelocity) / relativeVelocity.sqrMagnitude;

            // Calculate the avoidance force based on the distance and relative velocity
            Vector3 avoidanceForce = relativePosition + (relativeVelocity * timeToCollision);

            // Scale the force based on the distance to the obstacle
            avoidanceForce = avoidanceForce.normalized * (1 / relativePosition.magnitude);

            // Calculate the new movement direction based on the avoidance force
            Vector3 newMovementDirection = movementDirection + avoidanceForce;

            // Clamp the new movement direction to the valid range for the movement script
            float x = Mathf.Clamp(newMovementDirection.x, -1f, 1f);
            float y = Mathf.Clamp(newMovementDirection.y, -1f, 1f);

            // Return the new movement direction as a Vector2
            return new Vector2(x, y);*/
        }
        else
        {
            // No obstacle was hit, so set the velocity to the original movement velocity
            return movement;
        }
    }

    Vector2 CalMoveDirection(Vector2 prodMovement)
    {
        if ((prodMovement.x <= possibleTurn.y && prodMovement.x >= possibleTurn.x) && prodMovement.y > 0f)
        { //normal turn
            return new Vector2(Mathf.Clamp(prodMovement.x, -1, 1), Mathf.Clamp(prodMovement.y, -1, 1));
        }
        else
        {
            return new Vector2(Mathf.Clamp(prodMovement.x, -1, 1), 0f);
        }
        //return Vector2.zero;
    }
    private void OnDrawGizmos()
    {
        // Perform the boxcast
        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(transform.position, boxcastHalfExtents, transform.forward, out hitInfo, Quaternion.identity, AvoidDistance, obstacleMask);

        // Draw the boxcast area
        Gizmos.color = hit ? Color.red : Color.green;
        // Draw the boxcast area
        Gizmos.color = hit ? Color.red : Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * AvoidDistance, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxcastHalfExtents * 2);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
