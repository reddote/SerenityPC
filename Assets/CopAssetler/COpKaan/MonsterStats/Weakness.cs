using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Weakness
{

    public DamageType weaknessType;
    public bool isDiscovered;

    public Weakness(DamageType weaknessType, bool isDiscoveredForMonster)
    {
        this.weaknessType = weaknessType;
        isDiscovered = isDiscoveredForMonster;
    }
}
