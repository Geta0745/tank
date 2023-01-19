using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankComponent : MonoBehaviour
{
  public float damageMultiply = 2f; //like 10f -2f = 2f
  public float radius = 1f;

  [Range(0f,10f),SerializeField]
  float durability = 10f;
  [Range(0f,10f),SerializeField]
  float maxdurability = 10f;
  public HPSystem hpMaster;
  void Start(){
    durability = maxdurability;
  }
  
  public void TakeDamage(float damage){
    if(damage > 0f && durability > 0f){
      durability = Mathf.Clamp(durability-damage,0f,maxdurability);
      if(durability <= 0f && hpMaster != null){
        hpMaster.TakeDamageToMainTank(damageMultiply);
      }
    }
  }
}
