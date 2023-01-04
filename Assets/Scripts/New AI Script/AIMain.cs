using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementSystem))]
public class AIMain : MonoBehaviour
{
    [SerializeField] MovementSystem movementMaster;
    public Transform target;
    private NavMeshPath path;
    public Vector2 movement;
    [SerializeField] float nextWaypointDistance = 4f;
    private int currentNode = 0;
    [SerializeField] float calPathRate = 10f;
    private void Start() {
        path = new NavMeshPath();
        movementMaster = GetComponent<MovementSystem>();
        InvokeRepeating("CalculatePath",0f,calPathRate);
    }
    void Update()
    {
        if (currentNode < path.corners.Length)
        {
            Vector3 targetPos = path.corners[currentNode];
            Vector3 direction = targetPos - transform.position;
            float dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position);
            float dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position);
            Debug.Log(new Vector2(Mathf.Clamp(dotProdRear,-1,1),Mathf.Clamp(dotProdFront,-1,1)));
            movement = new Vector2(Mathf.Clamp(dotProdRear,-1,1),Mathf.Clamp(dotProdFront,-1,1));
            movementMaster.SetMovement(movement);
            if (Vector3.Distance(transform.position, targetPos) < nextWaypointDistance)
            {
                currentNode++;
            }
        }else{
            movement = Vector2.zero;
            movementMaster.SetMovement(movement);
        }

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }

    void CalculatePath()
    {
        if(target != null){
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            Debug.LogWarning(path.corners.Length);
        }
    }
}
