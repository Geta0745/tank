using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerMainTurret : MonoBehaviour
{
    public Transform turret;
    //public Transform gunMount;
    [SerializeField] float turretTurnSpeed = .5f;
    PlayerFireController aimStatus;

    //FIre
    public Rigidbody bullet;
    public Transform firePoint;
    public float pushForce;
    public float reloadTime = 3f;
    [SerializeField]
    public float countdownBeforeShot;
    [SerializeField] private bool readyToFire = false;

    void Start()
    {
        aimStatus = gameObject.GetComponent<PlayerFireController>();
    }

    // Update is called once per frame
    void Update()
    {
        rotateTurret();
        if(countdownBeforeShot < reloadTime){
            countdownBeforeShot = Mathf.Clamp(countdownBeforeShot+Time.deltaTime,0,reloadTime);
        }else{
            readyToFire = true;
        }
    }

    public void FireMainGun(){
        Debug.Log("Fire");
        if(readyToFire && bullet != null){
            readyToFire = false;
            countdownBeforeShot = 0f;
            Rigidbody rb = Instantiate(bullet.gameObject,firePoint.position,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(firePoint.transform.forward * pushForce);
        }
    }
    void rotateTurret(){
        Vector3 direction = aimStatus.aimPoint - turret.position;
        direction = Vector3.ProjectOnPlane(direction, transform.up);
        turret.rotation = Quaternion.Lerp(turret.rotation,Quaternion.LookRotation(direction, transform.up),turretTurnSpeed * Time.deltaTime);
    }

}
