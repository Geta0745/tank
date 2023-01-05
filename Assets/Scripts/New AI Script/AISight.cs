using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
    // The range of the line of sight
    public float range = 100f;
    [SerializeField] AIMain ai;
    // The layer mask of objects that should be considered for line of sight
    public LayerMask lineOfSightMask;

    private void Start()
    {
        ai = GetComponent<AIMain>();
    }
    void Update()
    {
        /*
        float distance = Vector3.Distance(transform.position, ai.target.position);

        if (ai.target != null)
        {
            // Create a ray from the source to the target
            Ray ray = new Ray(transform.position, ai.target.position - transform.position);

            // Use Physics.RaycastAll to get an array of RaycastHit structures
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // Loop through the array and check if any of the hits are obstacles
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.gameObject.gameObject.gameObject.gameObject)
                {
                    // There is an obstacle blocking the line of sight to the target
                    ai.targetTracked = false;
                    break;
                }
                else
                {
                    if (distance <= range)
                    {
                        // There are no obstacles blocking the line of sight to the target
                        ai.targetTracked = true;
                    }
                }
            }
        }*/
    }
}
