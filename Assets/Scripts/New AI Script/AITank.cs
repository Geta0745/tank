using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementSystem)), RequireComponent(typeof(TurretSystem))]
public class AITank : MonoBehaviour
{
    [Header("Master Movement And Turret Controller"), SerializeField, HideInInspector]
    MovementSystem movementMaster;
    [Tooltip("Turret Movement Controller Script"), HideInInspector]
    public TurretSystem turretMaster;
    [Tooltip("Target Transform that will be the Ai destination if target = null Ai will random path")]
    public Transform target;
    private NavMeshPath path;
    [Header("Calculate Path And Movement"), SerializeField, Tooltip("Holder Calculated Movement Value")]
    Vector2 movement;
    [SerializeField]
    float PicknextWaypointDistance = 4f;
    [SerializeField] float stopDistance = 2f;
    [SerializeField]
    private int currentNode = 0;
    [SerializeField, Tooltip("Calculate Path Rate")]
    float calPathRate = 10f;

    [Space]
    [Header("Recalculate Movement to Avoid Obstacle")]
    public bool enableAvoidOtherUnit = true;
    [SerializeField] Vector3 hitboxSize = new Vector3(1f, 1f, 1f);
    public LayerMask obstacleMask;
    public bool targetTracked = false;
    [Tooltip("Size of Avoid Obstacle Zone")]
    public float AvoidDistance = 5.0f;
    [SerializeField]
    [Tooltip("Possible Dot Prod Angle Turn")]
    float possibleTurnProd = 3f;
    float predictionFactor = 2f;
    [SerializeField] float dotProdFront, dotProdRear;
    private Color RandomPathColor;
    [SerializeField] bool obstacleHit;
    private void Start()
    {
        RandomPathColor = new Color(Random.value, Random.value, Random.value);

        path = new NavMeshPath();
        movementMaster = GetComponent<MovementSystem>();
        turretMaster = GetComponent<TurretSystem>();
        InvokeRepeating("CalculatePath", 0f, calPathRate);
    }

    private void Update()
    {
        obstacleHit = CheckObstacle();
        //check if have other unit block and enable to check obstacle
        if (obstacleHit && enableAvoidOtherUnit)
        {
            //assign dotprod to avoid obstacle
            dotProdFront = Vector3.Dot(transform.forward, transform.position - ReflectedObstaclePosition());
            dotProdRear = Vector3.Dot(transform.right, transform.position - ReflectedObstaclePosition());
            Debug.DrawLine(transform.position, transform.position - ReflectedObstaclePosition(), Color.red);
        }
        else //if not hit or anything assign dotprod to path cornors
        {
            if (currentNode < path.corners.Length && Vector3.Distance(transform.position,target.position) > stopDistance)
            { // check path not reached destination
                dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position);
                dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position);
                movementMaster.SetMovement(CalMoveDirection(new Vector2(dotProdRear, dotProdFront)));
                if (Vector3.Distance(transform.position, path.corners[currentNode]) < PicknextWaypointDistance)//pick next node
                {
                    currentNode++;
                }
            }
            else
            {
                movement = Vector2.zero;
                movementMaster.SetMovement(movement);
            }
        }

        PointTurretToTarget();

        for (int i = 0; i < path.corners.Length - 1; i++)//draw line
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], RandomPathColor);
        }
    }

    void PointTurretToTarget()
    {
        if (target != null)
        {
            turretMaster.SetTarget(target.position);
        }
        else
        {
            turretMaster.SetIdleTurret();
        }
    }
    public Vector2 GetMovement()
    {
        return movement;
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
    }

    public bool CheckObstacle()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, hitboxSize, transform.localRotation, obstacleMask);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void CalculatePath()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas))
        {
            // object is on NavMesh
            if (target != null)
            {
                NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            }
            /*else
            {
                NavMesh.CalculatePath(transform.position, RandomPositionOnNavmesh(), NavMesh.AllAreas, path);
            }*/
        }
        else
        {
            // object is not on NavMesh

            // Calculate the closest point on the NavMesh to the object's position
            NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas);
            Vector3 closestPoint = hit.position;

            // Calculate a new NavMesh path to the desired end point
            NavMesh.CalculatePath(closestPoint, target.position, NavMesh.AllAreas, path);
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
    Vector3 ReflectedObstaclePosition()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, hitboxSize, transform.localRotation, obstacleMask);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(Vector3.zero, hitboxSize * 2);
    }
}
