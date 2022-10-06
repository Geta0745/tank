using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointSystem : MonoBehaviour
{
    [SerializeField]
    public float maxHP = 550f;
    [SerializeField]
    public float currentHP;
    //x = front y = side z = rear w = top/button
    public Vector4 armor = new Vector4(10f,7f,5f,2f);
    public Vector3 dmgMultiply = new Vector3(.7f,1.2f,2f);
    [SerializeField]
    GameObject distructionPref;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void CalculateHit(Vector3 hitPos,float damage,float penetrate){
        Debug.Log("Damage Input : " + damage);
        float angle = Vector3.Angle(hitPos, transform.forward);
        if(Mathf.Approximately(angle, 0)){
            Debug.Log("front");
            if(isPenatrate(penetrate,armor.x)){
                TakeDamage(damage * dmgMultiply.x);
            }
            // back
        }else if(Mathf.Approximately(angle, 180)){
            Debug.Log("back");
            if(isPenatrate(penetrate,armor.y)){
                TakeDamage(damage * dmgMultiply.z);
            }
            //front
        }else if(Mathf.Approximately(angle, 90)){
            /*Vector3 cross = Vector3.Cross(transform.forward,hitPos);
            if(cross.y > 0){
                Debug.Log("right");
                //right
            }else{
                Debug.Log("left");
                //left
            }*/
            if(isPenatrate(penetrate,armor.z)){
                TakeDamage(damage * dmgMultiply.y);
            }
        }else{
            Debug.Log("Top or under Hit");
            if(isPenatrate(penetrate,armor.w)){
                TakeDamage(damage * dmgMultiply.y);
            }
        }
    }
    
    bool isPenatrate(float penetrate,float armor){
        if(armor >= penetrate){
            Debug.Log("Non penatrate : " + penetrate + "-" + armor);
            return false;
        }
        Debug.Log("penatrated! : " + penetrate + "-" + armor);
        return true;
    }

    public void TakeDamage(float damage){
        Debug.Log("Taken dmg : " + damage);
        if(currentHP - damage <= 0f){
            Die();
            return;
        }else{
            currentHP -= damage;
        }
    }

    public void ForceTakeDamage(float damage){
        if(currentHP - damage <= 0f){
            Die();
            return;
        }else{
            currentHP -= damage;
        }
    }

    void Die(){
        //playanimation or something
        //GameObject df = Instantiate(distructionPref,transform.position, transform.rotation);
        //Destroy(df,5f);
        Destroy(gameObject);
    }
}
