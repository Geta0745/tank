using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProd : MonoBehaviour
{
    public Vector2 prod;
    public float angle;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        prod = new Vector2(Vector3.Dot(transform.right, target.position - transform.position),Vector3.Dot(transform.forward, target.position - transform.position));
        angle = Vector3.Angle(transform.position,target.position - transform.position);
    }
}
