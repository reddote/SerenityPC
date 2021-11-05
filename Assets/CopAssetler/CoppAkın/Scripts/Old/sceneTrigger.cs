using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTrigger : MonoBehaviour
{
    public GameObject player, monsters ;
   
    public Camera drone;
    bool ananisikm = false;
    private PlayerController playerController;
    private SafeInventory saveInven;
    private CraftingInteractables craftCont;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayType.type= PlayType.playType.fps;
        playerController = FindObjectOfType<PlayerController>();
        saveInven = FindObjectOfType<SafeInventory>();
        craftCont = FindObjectOfType<CraftingInteractables>();
        //player = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
            drone.gameObject.SetActive(true);

        if(PlayType.type == PlayType.playType.fps)
        {
            player.gameObject.SetActive(true);
            if (playerController.inventoryisActive || saveInven.isActive || craftCont.isActive  || playerController.bestiaryisActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            //monsters.SetActive(true);
            //fpscam.enabled = true;
            
        }
        if(PlayType.type == PlayType.playType.drone)
        {
     
            drone.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(PlayType.type == PlayType.playType.driving)
        {
         
            drone.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }



        
    }
   
}
