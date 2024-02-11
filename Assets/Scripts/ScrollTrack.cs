using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementSystem))]
public class ScrollTrack : MonoBehaviour
{
    Material LTrack;
    Material RTrack;
    [SerializeField] MeshRenderer LtrackRenderer;
    [SerializeField] MeshRenderer RtrackRenderer;
    [SerializeField] float ScrollSpeedAdjustment = 3f;
    MovementSystem movementMaster;
    [ReadOnly] public float lastMoveDirection = 0f;
    [ReadOnly] public float trackMovement;
    // Start is called before the first frame update
    void Start()
    {
        movementMaster = GetComponent<MovementSystem>();
        LTrack = Instantiate(LtrackRenderer.sharedMaterial);
        LtrackRenderer.material = LTrack;

        RTrack = Instantiate(RtrackRenderer.sharedMaterial);
        RtrackRenderer.material = RTrack;
    }

    private void OnDestroy() {
        if(LTrack != null && RTrack != null){
            Destroy(LTrack);
            Destroy(RTrack);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(movementMaster.GetMovement().y != 0){
            lastMoveDirection = movementMaster.GetMovement().y;
        }
        trackMovement = Mathf.Abs(movementMaster.GetCurrentSpeed()/Mathf.Abs(movementMaster.GetMaxSpeed())) * lastMoveDirection;
        LTrack.SetFloat("_Speed",trackMovement);
        RTrack.SetFloat("_Speed",trackMovement);
    }
}
