using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    Vector3 startPos;
    [SerializeField] float damage = 10f;
    [SerializeField] float penetrate = 2f;
    GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject,10f);
        startPos = transform.position;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponent<HealthPointSystem>() != null){
            HealthPointSystem hp = other.gameObject.GetComponent<HealthPointSystem>();
            hp.CalculateHit(other.contacts[0].normal,damage,penetrate);
            Debug.Log("Hit :  " + other.contacts[0].normal);
        }
        if(hitEffect != null){
            GameObject effect = Instantiate(hitEffect,transform.position,Quaternion.identity);
            Destroy(effect,3f);
        }
        Destroy(gameObject);
    }
}
