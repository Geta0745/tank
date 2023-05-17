using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIMain))]
public class AISight : MonoBehaviour
{
    // The range of the line of sight

    [SerializeField] AIMain ai;
    // The layer mask of objects that should be considered for line of sight
    public LayerMask obstacleLayer;
    RaycastHit hit;
    [SerializeField] float fireAngle = 5f;
    public bool enableGizmos = false;
    TurretSystem turretMaster;
    private void Start()
    {
        ai = GetComponent<AIMain>();
        turretMaster = GetComponent<TurretSystem>();
    }
    void Update()
    {
        if (ai.target != null)
        {
            float Angle = Vector3.Angle(turretMaster.GetTurretPoint().forward,ai.target.position - turretMaster.GetTurretPoint().position);
            if(Angle <= fireAngle){
                if(!Physics.Raycast(turretMaster.GetMuzzlePoint().position,turretMaster.GetMuzzlePoint().forward,Vector3.Distance(turretMaster.GetMuzzlePoint().position,ai.target.position),obstacleLayer)){
                    turretMaster.FireMainGun();
                }
            }
            Debug.Log(Angle);
        }
    }

    private void OnDrawGizmos()
    {
        if (enableGizmos)
        {
            Gizmos.color = ai.targetTracked ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, Vector3.Distance(turretMaster.GetMuzzlePoint().position,ai.target.position));
        }

    }
}
