using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH.VehiclePhysics;

public class ActivateDrive : MonoBehaviour
{
    VehicleController vehicle;
    PlayerController playerController;
    bool isInCollider=false;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponentInParent<VehicleController>();
    }

    // Update is called once per frame
    void Update()
    {
        
   
        if (isInCollider && Input.GetKeyDown(KeyCode.F))
        {
            PlayType.type = PlayType.playType.driving;
            playerController.transform.parent = this.gameObject.transform;
            //other.gameObject.SetActive(false);
            playerController.playerCamera.gameObject.SetActive(false);
            //this.enabled = false;
            //playerController.weaponsController.enabled = false;
            playerController.playerObjInteract.enabled = false;
            playerController.characterController.enabled = false;
            playerController.audioSource.enabled = false;
            playerController.droneFollowMode.enabled = false;
            isInCollider = false;
            vehicle.Active = true;
        }
        else if (PlayType.type.Equals(PlayType.playType.driving) && Input.GetKeyDown(KeyCode.F))
        {
            PlayType.type = PlayType.playType.fps;
            playerController.transform.parent = null;
            //other.gameObject.SetActive(false);
            playerController.playerCamera.gameObject.SetActive(true);
            //this.enabled = false;
            playerController.playerLoadOut.enabled = true;
            playerController.playerObjInteract.enabled = true;
            playerController.characterController.enabled = true;
            playerController.audioSource.enabled = true;
            playerController.droneFollowMode.enabled = true;
            vehicle.Active = false;
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
    }

}
