using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Immunity : MonoBehaviour
{

    public DamageType immunityType;  
    public bool isDiscovered;

    public Immunity(DamageType immuneType , bool isDiscoveredForMonster)
    {
        immunityType = immuneType;
        isDiscovered = isDiscoveredForMonster;
    }


}
