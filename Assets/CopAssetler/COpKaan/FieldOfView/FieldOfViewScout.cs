using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class FieldOfViewScout : FieldOfView
{


    UnityEvent onTargetFind = new UnityEvent();
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        onTargetFind.AddListener(callSoldiers);
    }

    // Update is called once per frame
    void Update()
    {
        //FindVisibleTargets();
    }

    private void callSoldiers()
    {
        //Buraya monsterları çağıracak fonksiyon çağırılacak
        Debug.Log("Scout Gördü.");
        //MonsterAI.actionType = MonsterAI.ActionType.caughtByScout;
    }


    public override void FindVisibleTargets()
    {
        base.FindVisibleTargets();
        if(visibleTargets.Any())
            callSoldiers();
    }



}
