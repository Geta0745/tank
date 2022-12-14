using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(SimpleSmoothModifier))]
public class EnemyAI : MonoBehaviour
{
    public Transform targetPosition;
    [SerializeField] MovementSystem movementMaster;
    [SerializeField] TurretSystem turretMaster;
    private Seeker seeker;
    public float calPathRate = 20f;
    [SerializeField] float currentTime = 0f;

    public Path path;
    public Vector2 movement;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;
    Vector3 currentNodePos;
    //random part
    [SerializeField] GraphNode randomNode;
    public bool randomPath;
    public void Start () {
        seeker = GetComponent<Seeker>();
        currentTime = calPathRate;
        movementMaster = GetComponent<MovementSystem>();
    }

    public void OnPathComplete (Path p) {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.errorLog);

        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    Vector3 RandomPath(){
        var grid = AstarPath.active.data.gridGraph;

        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        return (Vector3)randomNode.position;
    }

    public void Update () {
        if(currentTime <= 0f){
            if(!randomPath){
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }else{
                seeker.StartPath(transform.position, RandomPath(), OnPathComplete);
            }
            currentTime = calPathRate;
        }else{
            currentTime -= Time.deltaTime;
        }
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
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

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;
        currentNodePos = path.vectorPath[currentWaypoint];
        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        float dotProdRear = Vector3.Dot(transform.right, path.vectorPath[currentWaypoint] - transform.position);
        float dotProdFront = Vector3.Dot(transform.forward, path.vectorPath[currentWaypoint] - transform.position);
        movement = new Vector2(Mathf.Clamp(dotProdRear,-1,1),Mathf.Clamp(dotProdFront,-1,1));
        
        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        movementMaster.SetMovement(movement);
        
        // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        // transform.position += velocity * Time.deltaTime;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position,currentNodePos);
    }
}
