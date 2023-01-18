using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSystem : MonoBehaviour
{
    [SerializeField] private Transform turret; // The transform of the turret
    [SerializeField] private Transform muzzlePoint; // Muzzle Point
    [SerializeField] private GameObject bulletPref; //Bullet Prefab
    public float turretRotateSpeed = 10f; // The speed at which the turret rotates
    [SerializeField] private float fireRate = 5f; // The fire rate in seconds
    [SerializeField] private float coolDown = 0f; // The time when the tank can fire again

    [SerializeField] Vector3 target;
    public bool moveable = true;
    Rigidbody rb;
    // Update is called once per frame
    private void Start()
    {
        coolDown = fireRate;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (coolDown >= 0)
        {
            coolDown -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        rotateTurret();
    }

    void rotateTurret()
    {
        if (turret != null && moveable)
        {
            Vector3 direction = target - turret.position;
            direction = Vector3.ProjectOnPlane(direction, transform.up);
            turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.LookRotation(direction, transform.up), turretRotateSpeed * Time.deltaTime);
        }
    }

    public Transform GetTurretTransform(){
        return turret;
    }

    public void FireMainGun()
    {
        if (coolDown <= 0f && moveable)
        {
            if (bulletPref != null && muzzlePoint != null)
            {
                TankBullet bullet = Instantiate(bulletPref,muzzlePoint.position,muzzlePoint.rotation).GetComponent<TankBullet>();
                // Destroy the bullet after 10 seconds
                if(rb != null){
                    rb.AddForceAtPosition(transform.up *0.5f,muzzlePoint.position,ForceMode.VelocityChange);
                }
                Destroy(bullet.gameObject, 10f);
            }
            coolDown = fireRate;
            //Debug.Log("Fired Main Gun!");
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetIdleTurret()
    {
        target = transform.position + transform.forward;
    }
}
