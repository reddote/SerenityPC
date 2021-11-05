using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractables : MonoBehaviour
{

    public GameObject staticGO;
    public bool isActive = false;

    public void Start()
    {
        staticGO = this.gameObject;
        StartSettings();
    }

    public void StartSettings()
    {
        
    }

    public void UpdateSettings()
    {

    }

    //hangi static objeyle interact ettiysek onun içindeki boolean ı true yapıyor
    public void staticInteractionController(GameObject interactionGO)
    {
        if(staticGO == interactionGO)
        {
            isActive = true;
        }
    }
}
