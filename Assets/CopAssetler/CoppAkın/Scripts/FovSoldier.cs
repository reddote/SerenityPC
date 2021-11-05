using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FovSoldier : FieldOfView
{
    private bool _inCoroutine = false;
    public bool caught = false, wander= true;
    UnityEvent onTargetFind = new UnityEvent();
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        onTargetFind.AddListener(CallSoldiers);
    }

    // Update is called once per frame
    
    private IEnumerator SwitchCaught()
    {
        const float waitSeconds = 2f;
        _inCoroutine = true;
        yield return new WaitForSeconds(waitSeconds);
        caught = false;
        wander = true;
        _inCoroutine = false;
    }
    private void CallSoldiers()
    {

        caught = true;
        wander = false;
        /*if (MonsterAI.keepWanderin && !MonsterAI.cokyakin)
        {
            if (!_inCoroutine)
                StartCoroutine(SwitchCaught());

        }*/

    }
    public override void FindVisibleTargets()
    {
        base.FindVisibleTargets();
        if (visibleTargets.Any())
            CallSoldiers();
        else
        {
            if (!_inCoroutine)
                StartCoroutine(SwitchCaught());
        }
    }
}
