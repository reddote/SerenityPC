using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterColliderControl : MonoBehaviour
{
    /*public static bool caught=false;
    bool inCoRoutine = false;
    float waitSeconds = 2f;
  
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            caught = true;
         //   Debug.Log("caught True");
            if (MonsterAI.keepWanderin && !MonsterAI.cokyakin)
            {
                if (!inCoRoutine)
                    StartCoroutine(switchCaught());

            }
        }
        

    }
    IEnumerator switchCaught()
    {
        inCoRoutine = true;
        yield return new WaitForSeconds(waitSeconds);
        caught = false;
        inCoRoutine = false;
    }
    private void OnTriggerStay(Collider col)
    {

        if (col.tag == "Player")
        {
            caught = true;
            //Debug.Log("caught True");
            if (MonsterAI.keepWanderin && !MonsterAI.cokyakin)
            {
                if (!inCoRoutine)
                    StartCoroutine(switchCaught());

            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!inCoRoutine)
                StartCoroutine(switchCaught());
            //caught = false;
            //Debug.Log("caught False");
        }
    }*/
}
