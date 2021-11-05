using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.VehiclePhysics;

public class VehicleDoor : MonoBehaviour
{
    public GameObject doorPlaneCollider;
    public GameObject doorCollider;
    private Animator vehicleAnimator;
    VehicleController vehicle;
    PlayerController playerController;
    bool isInCollider=false;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponentInParent<VehicleController>();
        vehicleAnimator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isInCollider)
        {
            vehicleAnimator.SetBool("OpenDoor",true);
            StartCoroutine(OpenDoorAnimColliders(true));
        }
        else
        {
            vehicleAnimator.SetBool("OpenDoor",false);
            StartCoroutine(OpenDoorAnimColliders(false));
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isInCollider = true;
            playerController = other.GetComponent<PlayerController>();


        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isInCollider = false;
           
        }
    }

    IEnumerator OpenDoorAnimColliders(bool doorState){
        yield return new WaitForSeconds(1);
        // trigger the stop animation events here
        doorPlaneCollider.SetActive(doorState);
        doorCollider.SetActive(!doorState);
                    
    }

}
