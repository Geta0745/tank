using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankComponent : MonoBehaviour
{
  public float damageMultiply = 2f; //like 10f -2f = 2f
  public float size = 1f;
  public float durability = 10f;
  public float maxdurability = 10f;
  public string name = "Some Component";
  public HPSystem hpMaster;
  void Start(){
    durability = maxdurability;
  }
  
  public void TakeDamage(float damge){
    if(damage > 0f && durability > 0f){
      durability = Mathf.Clamp(durability-damage,0f,maxdurability);
      if(durability <= 0f && hpMaster != null){
        hpMaster.TakeDamageToMainTank(damageMultiply);
      }
    }
  }
}
