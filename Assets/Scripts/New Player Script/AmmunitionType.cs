using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AmmunitionType : ScriptableObject
{
    public float penerationPoint = 102f;
    public float mass = 10f;
    public float velocity = 20f;
    public enum Shape
    {
        APHEBC, // Armor-piercing hight explosive ballistic capped
        APCR, // Armor-piercing composite rigid
        APCBC, // Armor-piercing capped ballistic capped
        HE, // High explosive
        HEAT // High explosive anti-tank
    }

    public Shape shape;
}
