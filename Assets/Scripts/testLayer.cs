using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLayer : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Player","german projectile");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
