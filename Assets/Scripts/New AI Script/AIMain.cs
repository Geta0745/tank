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
    public float nextWaypointDistance = 4f;
    [SerializeField] private int currentNode = 0;

    private void Start() {
        path = new NavMeshPath();
        movementMaster = GetComponent<MovementSystem>();
        //InvokeRepeating("CalculatePath",0f,calPathRate);
    }
    void Update()
    {
        CalculatePath();
        if(path.status == NavMeshPathStatus.PathComplete){
            UpdatePath();
        }
        //Debug.Log(path.status);
    }
    
    void UpdatePath(){
        if (currentNode < path.corners.Length)
        {
            Vector3 targetPos = path.corners[currentNode];
            Vector3 direction = targetPos - transform.position;
            //Debug.Log(new Vector2(Mathf.Clamp(dotProdRear,-1,1),Mathf.Clamp(dotProdFront,-1,1)));
            if (Vector3.Distance(transform.position, targetPos) < nextWaypointDistance)
            {
                currentNode++;
            }
            movementMaster.SetMovement(CalculateMovement());
            DrawPathLine();
        }
    }

    Vector2 CalculateMovement(){
        float dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position);
        float dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position);
        movement = new Vector2(Mathf.Clamp(dotProdRear,-1,1),Mathf.Clamp(dotProdFront,-1,1));
        return movement;
    }

    void DrawPathLine(){
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green);
        }
    }

    void CalculatePath()
    {
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        Debug.LogWarning("Length : " + path.corners.Length);
    }
}
