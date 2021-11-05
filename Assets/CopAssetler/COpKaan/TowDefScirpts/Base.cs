using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    public static float BaseHealth=1000;
    public static bool aliveBase=true;


    public static void takeDamage(float hitDamage)
    {
        BaseHealth -= hitDamage;
        Debug.Log("BaseHealth="+BaseHealth);
        Debug.Log(hitDamage);
        if (BaseHealth < 0)
        {
            aliveBase = false;
            Debug.Log("YOU DIED");
            Time.timeScale = 0; 
        }

    }



}
