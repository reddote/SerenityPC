using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static List<GameObject> livingTowDefMonsters = new List<GameObject>();
    public static Vector3 position = new Vector3(0, 0, 0);
    public GameObject MonsterToSpawn;
    public List<GameObject> spawnMonsters = new List<GameObject>();
    private int randomNumber;
    public float Timer=0;
    public int spawnPointLevel = 1;
    //Başlangıçtaki zaman değerine eşitlemek için.
    public float startingTimerCd=3;
    private float rastgele;

    // Start is called before the first frame update
    void Start()
    {
        rastgele = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        position.x = Random.Range(100, 160)*rastgele;
        position.y = -0.4f;
        rastgele = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        position.z = Random.Range(100, 160)*rastgele;
        
        transform.position = position;
     // 122,y,127
    }

    // Update is called once per frame
    void Update()
    {
        

        if (PlayType.type == PlayType.playType.drone)
        {
            Timer -= Time.deltaTime;
        }
       
        if (Timer < 0 && TowDefTimer.time>0 && PlayType.type==PlayType.playType.drone)
        {
            randomNumber = Random.Range(0, 100);
            if (randomNumber < 50)
            {
                MonsterToSpawn = spawnMonsters[0];
            }
            else
            {
                MonsterToSpawn = spawnMonsters[1];
            }

            livingTowDefMonsters.Add(Instantiate(MonsterToSpawn, transform.position, Quaternion.identity));

            Timer = startingTimerCd;
            
        }

    }

    public void levelSpawnPoint()
    {
        if (spawnPointLevel < 3)
        {
            spawnPointLevel++;
            if (spawnPointLevel == 1)
            {
                startingTimerCd = 6;

            }
            else if (spawnPointLevel == 2)
            {
                startingTimerCd = 5;
            }
            else if (spawnPointLevel == 3)
            {
                startingTimerCd = 4;
            }
          
        }
    }






}
