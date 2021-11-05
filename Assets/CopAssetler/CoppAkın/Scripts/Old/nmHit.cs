using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class nmHit : MonoBehaviour
{
    
    NavMeshHit nmhit;
    public bool hitted;
    private GameObject go;
    string goName;
    
    string thisArea;
    int sayac = 0,thisIndex;

    
    // Start is called before the first frame update
    void Start()
    {
        
       go = this.gameObject;
        goName = go.name;
        
    }
    void areaFinder(string area)
    {
        if (area == "first")
            thisIndex = 5;
            else if (area == "second")
                thisIndex = 6;
                else if (area == "third")
                    thisIndex = 7;
                   else if (area == "fourth")
                        thisIndex = 8;

    }

    
    void Update()
    {

        
        if (TowDefMonster.deathCount > 8)
        {
            areaFinder(thisArea);
            if (sayac > 10)
                NavMesh.SetAreaCost(thisIndex, 15);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //print("goname bu " + goName);
        sayac++;
        thisArea = goName;
    }
   
}
