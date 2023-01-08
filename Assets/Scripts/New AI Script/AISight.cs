using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
    // The range of the line of sight
    public float range = 100f;
    [SerializeField] AIMain ai;
    // The layer mask of objects that should be considered for line of sight
    public LayerMask layerMask;
    RaycastHit hit;
    private void Start()
    {
        ai = GetComponent<AIMain>();
    }
    void Update()
    {
        Vector3 direction = ai.target.position - transform.position;
        bool clearVision = false;
        if (Physics.Raycast(transform.position, direction, out hit, layerMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }else{
            Debug.Log("No OBstacle Block");
        }
    }
}
