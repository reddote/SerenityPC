using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public enum ActionTypeDrone {idle,follow,combat}

    public ActionTypeDrone droneActionType;
    
    public float targetDistance;
    public float currentDistance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        droneActionType = ActionTypeDrone.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (droneActionType)
        {
            case ActionTypeDrone.idle:
                
                
                break;
            case ActionTypeDrone.follow:
                
                
                break;
            case ActionTypeDrone.combat:
                
                
                break;
            default:
                droneActionType = ActionTypeDrone.idle;
                break;
        }
    }
    
    
    
    
    
}
