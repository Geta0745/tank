using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNewMovement : MovementSystem
{
    // Reference to the left and right track colliders
    [Header("Test")] public Collider leftTrackCollider;
    public Collider rightTrackCollider;

    protected override void Update()
    {
        // Check if the left track collider is colliding with anything
        if (leftTrackCollider)
        {
            Debug.Log("Left track colliding with the ground!");
        }

        // Check if the right track collider is colliding with anything
        if (rightTrackCollider)
        {
            Debug.Log("Right track colliding with the ground!");
        }
    }
}
