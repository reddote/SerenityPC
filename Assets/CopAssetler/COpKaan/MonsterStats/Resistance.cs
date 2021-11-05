using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resistance
{

    public DamageType resistanceType;
    public int damageRedcution;
    public bool isDiscovered;

    public Resistance(DamageType resType, int reductionLevel,bool isDiscoveredResistance)
    {
        resistanceType = resType;
        damageRedcution = reductionLevel;
        isDiscovered = isDiscoveredResistance;
    }



}
