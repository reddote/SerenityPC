using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class FieldOfViewSoldier : FieldOfView
{
    public static bool caught = false;
    bool inCoRoutine = false;
    float waitSeconds = 2f;
    UnityEvent onTargetFind = new UnityEvent();
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        onTargetFind.AddListener(callSoldiers);
    }
    IEnumerator switchCaught()
    {
        inCoRoutine = true;
        yield return new WaitForSeconds(waitSeconds);
        caught = false;
        inCoRoutine = false;
    }
    private void callSoldiers()
    {
        
       // Debug.Log("Soldier Gördü.");
        caught = true;
        //   Debug.Log("caught True");
        if (MonsterAI.keepWanderin && !MonsterAI.cokyakin)
        {
            if (!inCoRoutine)
                StartCoroutine(switchCaught());

        }

    }

    public override void FindVisibleTargets()
    {
        base.FindVisibleTargets();
        if (visibleTargets.Any())
            callSoldiers();
        else
        {
            if (!inCoRoutine)
                StartCoroutine(switchCaught());
        }
    }

}
