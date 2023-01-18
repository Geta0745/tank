using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public ArmorInfo _ArmorInfo;
    [SerializeField] float shockedDuration = 0f;
    MovementSystem movementMaster;
    TurretSystem turretMaster;
    [SerializeField] float shockedMultiply = 20f; // shocked += 20f / remainArmorPeneration (example 20f)=1f
    Rigidbody rb;
    float Hp = 10f;
    [SerializeField] TankComponent[] components;
    // Start is called before the first frame update
    void Start()
    {
        movementMaster = GetComponent<MovementSystem>();
        turretMaster = GetComponent<TurretSystem>();
        rb = GetComponent<Rigidbody>();
    }

    public float CalRelativeArmor(Vector3 hitPos)
    { // a = Armor Thickness , b = Angle of Impact , c = relative Armor
        float a, b = 0f;
        float angle = Vector3.Angle(hitPos, transform.forward);
        if (Mathf.Approximately(angle, 0))
        {
            Debug.Log("front");
            a = _ArmorInfo.armorThickness.x;
            b = _ArmorInfo.armorAngle.x;
        }
        else if (Mathf.Approximately(angle, 180))
        {
            Debug.Log("back");
            a = _ArmorInfo.armorThickness.z;
            b = _ArmorInfo.armorAngle.z;
        }
        else if (Mathf.Approximately(angle, 90))
        {
            Debug.Log("Rear");
            a = _ArmorInfo.armorThickness.y;
            b = _ArmorInfo.armorAngle.y;
        }
        else
        {
            Debug.Log("Top or under Hit");
            a = _ArmorInfo.armorThickness.z;
            b = _ArmorInfo.armorAngle.z;

        }
        float c = a / Mathf.Cos(Mathf.Deg2Rad * b);
        return c;
    }

    public void CalPeneration(float relativeArmor, AmmunitionType ammoType, Vector3 hitPos,Vector3 forwardDir)
    {
        float armorPenerationPoint = ammoType.penerationPoint;
        if (armorPenerationPoint > 0f)
        {
            if (relativeArmor > armorPenerationPoint)
            {
                WhenCantPenerated();
            }
            else if (relativeArmor == armorPenerationPoint)
            {
                WhenCantPenerated();
            }
            else
            {
                if (rb != null)
                {
                    rb.AddForceAtPosition(transform.up * 1f, hitPos, ForceMode.VelocityChange);
                }
                WhenPentrated(relativeArmor - armorPenerationPoint,forwardDir,hitPos);
            }
        }
    }

    public void HitAction(Vector3 hitPos,AmmunitionType ammoType,Vector3 forwardDir)
    {
        float relativeArmor = CalRelativeArmor(hitPos);
        CalPeneration(relativeArmor, ammoType, hitPos,forwardDir);
    }

    public virtual void WhenPentrated(float remaineArmorPenerate,Vector3 forwardDir,AmmunitionType ammoType,Vector3 hitPos)
    {
        shockedDuration += 0.2f;
        shockedDuration += shockedMultiply / Mathf.Abs(remaineArmorPenerate);
        //foreach for damage component
        /*foreach(TankComponent component in components){
            float distance = Vector3.Distance(hitPos + (forwardDir * ammoType.fuzeDelay),component.gameObject.transform.position);
        }*/
    }
    
    public void TakeDamageToMainTank(float damage){
        if(damage > 0f){
            //max HP is always 10f
            Hp = Mathf.Clamp(Hp-damage,0f,10f);
            if(Hp <= 0f){
                DestroyTank();
            }
        }
    }
    
    public void DestroyTank(){
        Destroy(gameObject);
    }
    
    public virtual void WhenCantPenerated()
    {
        Debug.LogWarning("Shocked : " + shockedDuration + " / 0.3f" );
        shockedDuration += 0.3f;
    }
    // Update is called once per frame
    void Update()
    {
        if (shockedDuration > 0f)
        {
            shockedDuration -= Time.deltaTime;
            if (movementMaster != null && turretMaster != null)
            {
                movementMaster.moveable = false;
                turretMaster.moveable = false;
            }
        }
        else
        {
            if (movementMaster != null && turretMaster != null)
            {
                movementMaster.moveable = true;
                turretMaster.moveable = true;
            }
        }
    }
}
