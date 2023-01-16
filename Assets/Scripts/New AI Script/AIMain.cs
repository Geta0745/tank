using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(TurretSystem))]
[RequireComponent(typeof(AISight))]
public class AIMain : MonoBehaviour
{
    [Header("Movement Controller Script")]
    [Tooltip("Movement Controller Script")]
    [SerializeField]
    MovementSystem movementMaster;
    [Tooltip("Turret Movement Controller Script")]
    [HideInInspector]
    public TurretSystem turretMaster;
    public Transform target;
    private NavMeshPath path;
    [Header("Calculate Path And Movement")]
    [SerializeField]
    [Tooltip("Holder Calculated Movement Value")]
    Vector2 movement;
    [SerializeField]
    float PicknextWaypointDistance = 4f;
    private int currentNode = 0;
    [SerializeField]
    [Tooltip("Calculate Path Rate")]
    float calPathRate = 10f;

    [Space]
    [Header("Recalculate Movement to Avoid Obstacle")]
    public bool enableAvoidOtherUnit = true;
    public LayerMask obstacleMask;
    public bool targetTracked = false;
    [Tooltip("Size of Avoid Obstacle Zone")]
    public Vector3 boxcastHalfExtents = new Vector3(1.0f, 1.0f, 1.0f);
    public float AvoidDistance = 5.0f;
    [SerializeField]
    [Tooltip("Possible Dot Prod Angle Turn")]
    float possibleTurnProd = 3f;
    [SerializeField] float dotProdFront, dotProdRear;

    private void Start()
    {
        path = new NavMeshPath();
        movementMaster = GetComponent<MovementSystem>();
        turretMaster = GetComponent<TurretSystem>();
        InvokeRepeating("CalculatePath", 0f, calPathRate);
    }
    void Update()
    {
        if (currentNode < path.corners.Length) //check if not reached destination
        {
            bool obstacleHit = CheckObstacle();
            //check if have other unit block 
            if (obstacleHit && enableAvoidOtherUnit)
            {
                dotProdFront = Vector3.Dot(transform.forward, transform.position - ReflectedObstaclePosition());
                dotProdRear = Vector3.Dot(transform.right, transform.position - ReflectedObstaclePosition());
                Debug.DrawLine(transform.position, transform.position - ReflectedObstaclePosition(), Color.red);
            }
            else
            {
                dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position);
                dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position);
            }

            if (!targetTracked)//if target is not in sight
            {
                movement = CalMoveDirection(new Vector2(dotProdRear, dotProdFront));
                movementMaster.SetMovement(movement);
                turretMaster.SetTarget(path.corners[currentNode]);
            }
            else if(targetTracked && obstacleHit) //target in sight
            {
                movement = CalMoveDirection(new Vector2(dotProdRear, dotProdFront));
                movementMaster.SetMovement(movement);
                turretMaster.SetTarget(target.position);
            }else if(targetTracked && !obstacleHit){
                movement = new Vector2(Mathf.Clamp(Vector3.Dot(transform.right, target.position - transform.position), -1f, 1f), 0f);
                movementMaster.SetMovement(movement);
                turretMaster.SetTarget(target.position);
            }

            if (Vector3.Distance(transform.position, path.corners[currentNode]) < PicknextWaypointDistance)//pick next node
            {
                currentNode++;
            }
        }
        else //reached destination
        {
            movement = Vector2.zero;
            movementMaster.SetMovement(movement);
        }

        for (int i = 0; i < path.corners.Length - 1; i++)//draw line
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
        }
    }

    void CalculatePath()
    {
        if (target != null)
        {
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            NavMesh.CalculateTriangulation();
        }
        else
        {
            NavMesh.CalculatePath(transform.position, RandomPositionOnNavmesh(), NavMesh.AllAreas, path);
        }
        currentNode = 0;

    }

    Vector3 RandomPositionOnNavmesh()
    {
        // Create a random position within the bounds of the navmesh
        Vector3 randomPos = new Vector3(Random.Range(-200f, 200f), 0, Random.Range(-200f, 200f));

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

    public bool CheckObstacle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AvoidDistance,obstacleMask);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                Debug.LogError(gameObject.name + " --> " + collider.gameObject.name);
                return true;
            }
        }
        return false;
        //return Physics.BoxCast(transform.position, boxcastHalfExtents, transform.forward, Quaternion.identity, AvoidDistance, obstacleMask);
    }

    Vector3 ReflectedObstaclePosition()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AvoidDistance, obstacleMask);
        Vector3 center = Vector3.zero;
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                center += collider.transform.position;
            }
            center = center / hitColliders.Length;
        }
        return center;
    }
    Vector2 AvoidanceObstacle()
    {
        RaycastHit hitInfo;
        // Perform the Boxcast to check for obstacles
        bool hit = Physics.BoxCast(transform.position, boxcastHalfExtents, transform.forward, out hitInfo, Quaternion.identity, AvoidDistance, obstacleMask);

        // If an obstacle was hit, adjust the movement to avoid the obstacle
        if (hit)
        {
            Vector3 reflectedDirection = Vector3.Reflect(transform.forward, hitInfo.normal);
            Vector2 movement = new Vector2(Mathf.Clamp(Vector3.Dot(transform.right, path.corners[currentNode]), -1f, 1f), Mathf.Clamp(Vector3.Dot(transform.forward, path.corners[currentNode]), -1, 1f));
            return movement;
        }
        else
        {
            // No obstacle was hit, so set the velocity to the original movement velocity
            return movement;
        }
    }

    Vector2 CalMoveDirection(Vector2 prodMovement)
    {
        if ((prodMovement.x <= possibleTurnProd && prodMovement.x >= -possibleTurnProd) && prodMovement.y > 0f)
        { //normal turn
            return new Vector2(Mathf.Clamp(prodMovement.x, -1, 1), Mathf.Clamp(prodMovement.y, -1, 1));
        }
        else
        {
            return new Vector2(Mathf.Clamp(prodMovement.x, -1, 1), 0f);
        }
        //return Vector2.zero;
    }
    /*private void OnDrawGizmos()
    {
        // Perform the boxcast
        RaycastHit hitInfo;
        bool hit = Physics.BoxCast(transform.position, boxcastHalfExtents, transform.forward, out hitInfo, Quaternion.identity, AvoidDistance, obstacleMask);

        // Draw the boxcast area
        Gizmos.color = hit ? Color.red : Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * AvoidDistance, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxcastHalfExtents * 2);
        Gizmos.matrix = Matrix4x4.identity;
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AvoidDistance);
    }
}
