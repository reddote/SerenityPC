using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotateMech : MonoBehaviour
{
    Turret turret;


    // Start is called before the first frame update
    void Start()
    {
        turret = GetComponentInParent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {

        if (turret.targetMonster != null)
            transform.LookAt(turret.targetMonster.transform);

    }


    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
    }


}
