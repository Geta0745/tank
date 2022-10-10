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
    [SerializeField] LineRenderer lrTurret;
    [SerializeField] LineRenderer lrTurretErr;
    [SerializeField] LineRenderer lrMuzzleError;
    //[SerializeField] LineRenderer lrMuzzle;
    [SerializeField] LayerMask aimMask;
    [SerializeField] LayerMask gunAim;
    private InputAction look;
    PlayerControl PlayerControls;
    private void Awake() {
        PlayerControls = new PlayerControl();
        cam = GameObject.FindObjectOfType<Camera>();
    }

    private void OnEnable() {
        look = PlayerControls.Player.Look;
        look.Enable();
    }

    private void OnDisable() {
        look.Disable();
    }

    public void UpdatePoint(Vector3 aimHit){
        transform.position = aimHit;
        transform.position = new Vector3(transform.position.x,transform.position.y + .05f,transform.position.z);
    }

    public void DrawPointLine(Transform turretPos,Transform muzzlePos,Vector3 aimHit,LayerMask MuzzleLayer){
        float distanceFromTurret = Vector3.Distance(player.turret.position,aimHit);
        float distanceFromMuzzle = Vector3.Distance(muzzlePos.position,aimHit);
        Vector3 aimDirection = aimHit - turretPos.position;

        //Raycast Stuff
        RaycastHit TurretHit;
        RaycastHit MuzzleHit;

        //Assigned Start Line
        lrTurret.SetPosition(0,turretPos.position);
        //lrMuzzle.SetPosition(0,muzzlePos.position);

        //Raycast From Turret to AimDirection **Check if obsticle obthe way
        if(Physics.Raycast(player.turret.position,aimDirection,out TurretHit,distanceFromTurret,MuzzleLayer)){
            lrTurret.SetPosition(1,TurretHit.point);
            lrTurretErr.SetPosition(0,TurretHit.point);
            lrTurretErr.SetPosition(1,aimHit);
        }else{
            lrTurret.SetPosition(1,aimHit);
            lrTurretErr.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
        }

        //Raycast From Muzzle to Muzzle.forward **Check if obsticle onthe way
        if(Physics.Raycast(muzzlePos.position,muzzlePos.forward,out MuzzleHit,distanceFromMuzzle,MuzzleLayer)){
            lrMuzzleError.SetPosition(0,muzzlePos.position);
            lrMuzzleError.SetPosition(1,MuzzleHit.point);
        }else{
            lrMuzzleError.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*lrTurret.SetPosition(0,player.turret.position);
        Ray ray = cam.ScreenPointToRay(look.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,500f,aimMask))
        {
            Vector3 codinatePoint = new Vector3(hit.point.x,player.firePoint.position.y,hit.point.z);
            transform.position = hit.point;
            transform.position = new Vector3(transform.position.x,transform.position.y + .05f,transform.position.z);

            float distance = Vector3.Distance(player.turret.position,hit.point);
            RaycastHit hit2;
            Vector3 MainDir = hit.point - player.turret.position;
            if(Physics.Raycast(player.turret.position,MainDir,out hit2,distance,gunAim)){
                lrTurret.SetPosition(1,hit2.point);
                lrTurretErr.SetPosition(0,hit2.point);
                lrTurretErr.SetPosition(1,hit.point);
            }else{
                lrTurret.SetPosition(1,hit.point);
                lrTurretErr.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
            }

            float distanceToFirePoint = Vector3.Distance(player.firePoint.position,hit.point);
            if(Physics.Raycast(player.firePoint.position,player.firePoint.forward,out hit2,distanceToFirePoint,gunAim)){
                lrMuzzle.SetPosition(0,player.firePoint.position);
                lrMuzzle.SetPosition(1,hit2.point);
                lrMuzzleError.SetPosition(0,hit2.point);
                lrMuzzleError.SetPosition(1,hit.point);
            }else{
                lrMuzzle.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
                lrMuzzleError.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
            }

            float angle = Vector3.Angle(player.firePoint.forward,codinatePoint - player.firePoint.position);
            if(angle <= reachedAngle){
                aimIcon.color = Color.green;
                lrMuzzle.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
            }else{
                aimIcon.color = Color.red;
            }*/

            /*if(player != null){
                float distance = Vector3.Distance(player.turret.position,hit.point);
                RaycastHit hit2;
                if(Physics.Raycast(player.turret.position,player.turret.forward,out hit2,distance,gunAim)){
                    //Change AimPoint to position
                    transform.position = hit.normal;
                    transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;

                    lrTurret.SetPosition(1,hit2.point);
                    lrTurretErr.SetPosition(0,hit2.point);
                    lrTurretErr.SetPosition(1,hit.point - hit.transform.up);

                    lrMuzzle.SetPosition(0,player.firePoint.position);
                    lrMuzzle.SetPosition(1,hit2.point);
                    lrMuzzleError.SetPosition(0,hit2.point);
                    lrMuzzleError.SetPosition(1,hit.point);
                }else{
                    //transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
                    lrTurret.SetPosition(1,hit.point);

                    //lrMuzzleError.SetPositions(new Vector3[2]{Vector3.zero,Vector3.zero});
                }
            }*/
    }
}
