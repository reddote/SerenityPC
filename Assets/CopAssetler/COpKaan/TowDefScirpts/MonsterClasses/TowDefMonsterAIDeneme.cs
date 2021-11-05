//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//
//public class TowDefMonsterAIDeneme : MonoBehaviour
//{
//
//    public Transform target;
//    NavMeshAgent monsterAgent;
//    BasicMonster monster;
//    private float timeForNewPath;
//    bool inCoRoutine = false;
//    private Transform splitTarget;
//    Vector3 spawnPointpos,splitVector;
//    float rX, rZ;
//   
//    
//    // Start is called before the first frame update
//    void Start()
//    {
//
//        spawnPointpos = SpawnPoint.position;
//        target = GameObject.FindGameObjectWithTag("Base").transform;
//        this.gameObject.transform.LookAt(target.transform);
//        monsterAgent = GetComponent<NavMeshAgent>();
//        monster=GetComponent<BasicMonster>();
//        //monsterAgent.speed = monster.moveSpeed;
//        StartCoroutine(splitUp());
//
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//        
//        if (monster!=null &&!monster.isAlive)
//        {
//           monsterAgent.speed = 0;
//           monster.transform.localEulerAngles = new Vector3(0, 0, 90);
//        }
//      
//
//    }
//    IEnumerator splitUp()
//    {
//        inCoRoutine = true;
//        timeForNewPath = Random.Range(4, 7);
//        int sagsol = Random.Range(1, 3);
//
//        if (sagsol == 1)
//            splitVector = new Vector3(spawnPointpos.x + Random.Range(-25, -99), -0.4f, spawnPointpos.z);
//        if (sagsol == 2)
//            splitVector = new Vector3(spawnPointpos.x, -0.4f, spawnPointpos.z - Random.Range(25, 99));
//
//        monsterAgent.destination = splitVector;
//        yield return new WaitForSeconds(timeForNewPath);
//        monsterAgent.destination = target.transform.position;
//
//        inCoRoutine = false;
//        
//    }
//    /*IEnumerator goBack()
//    {
//        Vector3 prevPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
//        
//        if(true)
//            monsterAgent.destination = prevPos;
//        yield return new WaitForSeconds(4f);
//    }*/
//}
