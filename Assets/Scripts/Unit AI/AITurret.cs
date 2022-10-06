using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : MonoBehaviour
{
    AIUnit mainAI;
    public Transform turret;
    //public Transform gunMount;
    [SerializeField]
    float turretTurnSpeed = .5f;
    public Transform firePoint;
    public float pushForce;
    public float reloadTime = 3f;
    [SerializeField]
    public float countdownBeforeShot;
    [SerializeField] private bool readyToFire = false;
    void Start()
    {
        mainAI = gameObject.GetComponent<AIUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        setAimPoint();
        if(countdownBeforeShot < reloadTime){
            countdownBeforeShot = Mathf.Clamp(countdownBeforeShot+Time.deltaTime,0,reloadTime);
        }else{
            readyToFire = true;
        }
    }

    void setAimPoint(){
        rotateTurret();
    }

    void rotateTurret(){
        Vector3 direction = mainAI.targetPosition.position - turret.position;
        direction = Vector3.ProjectOnPlane(direction, transform.up);
        turret.rotation = Quaternion.Lerp(turret.rotation,Quaternion.LookRotation(direction, transform.up),turretTurnSpeed * Time.deltaTime);
    }
}
