using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankBullet : MonoBehaviour
{
    public AmmunitionType ammoType;
    Rigidbody rb;
    public float currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        rb.MovePosition(rb.transform.position+ rb.transform.forward * ammoType.velocity * Time.deltaTime);
        currentVelocity = rb.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision other) {
        HPSystem hp = other.gameObject.GetComponent<HPSystem>();
        if(hp != null){
            hp.HitAction(other.contacts[0].point,ammoType,transform.forward);
        }
        Destroy(gameObject);
    }
}
