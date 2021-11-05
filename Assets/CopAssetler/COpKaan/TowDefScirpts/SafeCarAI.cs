using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SafeCarAI : MonoBehaviour
{

    public Vector3 currentTarget;
    public NavMeshAgent safecarAgent;
    public List<GameObject> targetQueue;
    public SafeInventory baseInventory;
    public GameObject Base;
    float collectTime = 0f;
    SafeCar safeCarInventory;
    Item itemInfo;


    // Start is called before the first frame update
    void Start()
    {
        safecarAgent = GetComponent<NavMeshAgent>();
        targetQueue = new List<GameObject>();
        safeCarInventory = this.GetComponent<SafeCar>();
    }



    public void enqueueTarget(GameObject target)
    {
        targetQueue.Add(target);
    }

    public void setSafeCarDest()
    {
        currentTarget = targetQueue.First().transform.position;
        Debug.Log("sa");
        safecarAgent.SetDestination(currentTarget);
       
    }
    public void setSafeCarToBase()
    {
        safecarAgent.SetDestination(Base.transform.position);
        //itemleri kasaya atan döngü
        if(Vector3.Distance(Base.transform.position, transform.position) < safecarAgent.stoppingDistance + 1)
        {
            safeCarInventory.sendItemSafe();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //İtem sınırı ile ilgili bir if yaz muhtemelen ilk ifin içine yazabilirsin.
        if (targetQueue.Count > 0)
        {

            if (Vector3.Distance(targetQueue.First().transform.position, transform.position) < safecarAgent.stoppingDistance + 1)
            {
                float coolDown = 2f;
                if(collectTime < coolDown)
                {
                    collectTime += Time.deltaTime;
                }
                if (collectTime >= coolDown)
                {
                    var tempTargetGO = targetQueue.First();
                    itemInfo = tempTargetGO.GetComponent<Item>();
                    safeCarInventory.AddItemSafe(itemInfo, itemInfo.ID, 1, itemInfo.itemCountLimit, itemInfo.itemName);
                    Destroy(tempTargetGO);
                    targetQueue.RemoveAt(0);
                    collectTime = 0f;
                    if (targetQueue.Count > 0)
                    {
                        setSafeCarDest();

                    }
                }
                
            }
            
        }
        else
        {
            setSafeCarToBase();

        }

    }

}
