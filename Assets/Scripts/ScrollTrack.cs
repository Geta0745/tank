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
    [SerializeField] float scrollSpeed = 0.5f;
    MovementSystem movementMaster;
    public float movement;
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
        movement = movementMaster.GetMovement().y;
        LTrack.SetFloat("_Speed",scrollSpeed * movement);
        RTrack.SetFloat("_Speed",scrollSpeed * movement);
    }
}
