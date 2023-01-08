using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArmorInfo : ScriptableObject
{
    /*public enum ArmorType
    {
        RHA, // Rolled homogeneous armor
        LHA, // Laminated homogeneous armor
        CERA, // Ceramic armor
        NERA // Non-explosive reactive armor
    }

    public ArmorType armorType;*/
    public Vector4 armorThickness = new Vector4(102f,80f,80f,30f); //x = front , y = rear , z = back , w = up-down
    public Vector4 armorAngle = new Vector4(15f,20f,10f,0f); //x = front , y = rear , z = back , w = up-down
}
