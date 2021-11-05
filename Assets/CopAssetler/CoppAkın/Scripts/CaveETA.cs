using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveETA : MonoBehaviour
{
    private Transform _caveTransform,_baseTransform; 
    
    private void Update()
    {
        
    }

    private int CalcEta()
    {
        _caveTransform = this.transform;
        _baseTransform = GameObject.FindGameObjectWithTag("Base").transform;
        var distance = Vector3.Distance(_baseTransform.position, _caveTransform.position);
        var eta = (int) distance / 10;
        return eta;
    }

   
}
