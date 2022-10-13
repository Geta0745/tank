using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimPointSystem : MonoBehaviour
{
    Camera cam;
    public float reachedAngle = 10f;
    [SerializeField] SpriteRenderer aimIcon;
    [SerializeField] PlayerMainTurret player;
    [SerializeField] LineRenderer lrMuzzleError;
    [SerializeField] LineRenderer lrMuzzle;

    private void Awake() {
        cam = GameObject.FindObjectOfType<Camera>();
    }

    public void UpdatePoint(Vector3 aimHit){
        transform.position = aimHit;
        transform.position = new Vector3(transform.position.x,transform.position.y + .05f,transform.position.z);
    }

    public void DrawPointLine(Transform muzzlePos,Vector3 aimHit,LayerMask MuzzleLayer){
        float distanceFromMuzzle = Vector3.Distance(muzzlePos.position,aimHit);

        //Raycast Stuff
        RaycastHit MuzzleHit;

        //Raycast From Muzzle to Muzzle.forward **Check if obsticle onthe way
        if(Physics.Raycast(muzzlePos.position,muzzlePos.forward,out MuzzleHit,distanceFromMuzzle,MuzzleLayer)){
            lrMuzzle.SetPosition(0,muzzlePos.position);
            lrMuzzle.SetPosition(1,MuzzleHit.point);
            lrMuzzleError.SetPosition(0,MuzzleHit.point);
            lrMuzzleError.SetPosition(1,muzzlePos.forward * Vector3.Distance(MuzzleHit.point,aimHit) + MuzzleHit.point);
        }else{
            lrMuzzle.SetPosition(0,muzzlePos.position);
            lrMuzzle.SetPosition(1,muzzlePos.forward * distanceFromMuzzle + muzzlePos.position);
            lrMuzzleError.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
        }

    }

    public void BackToZeroPoint(){
        lrMuzzle.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
        lrMuzzleError.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
    }

}
