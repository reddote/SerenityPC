using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    //TowDefTimeraBağlı
    
    public GameObject spawnPointpf;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();



    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>().ToList();

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void endTowDef()
    {
       for(int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].levelSpawnPoint();   
        }

       if(TowDefTimer.newSpawn > 2)
        newSpawnPoint(spawnPointpf);
    }


    public void newSpawnPoint(GameObject spawnPointprefab) {

        GameObject go = Instantiate(spawnPointprefab); 
     
        spawnPoints.Add(go.GetComponent<SpawnPoint>());

    }



}
