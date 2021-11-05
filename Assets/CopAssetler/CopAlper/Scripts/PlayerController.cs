using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    //Tuş atamaları ve değiştirmesi rahat olacağından bu şekilde tanımlıyoruz.
    private string inventoryInputName;
    private string jetPackInputName;
    private string sprintInputName;
    private string crouchInputName;
    private string droneInputName;
    private string staticInteractablesExit;
    //sayısal değişkenler
    private float Health;
    private float jetPackSpeed;
    private float playerMoveSpeed;
    private float tempPlayerMoveSpeed;
    private float jetPackCoolDown;
    private float tempJetPackCD = 0f;
    public float crouchCameraSmooth;
    private float playerJumpPower;
    public float playerJumpRayLong;
    private float playerStamina;
    private float playerHeight;
    private float speedMultiplier = 1.75f;
    private float menuFixerF = 0.1f;
    //private unitye özel değişkenler
    public PlayerLoadOut playerLoadOut;
    public PlayerObjInteract playerObjInteract;
    public CharacterController characterController;
    public AudioSource audioSource;
    public GameObject playerGO;
    private Rigidbody playerRB;
    public GameObject inventory;
    public GameObject safeUI;
    private CapsuleCollider playerCapsuleCollider;
    public Camera playerCamera;
    private Player playerObject;
    [NonSerialized]public DroneFollowMode droneFollowMode;
    private LoadOutInteractables loadOut;
    private SafeInventory safeInvCont;
    private CraftingInteractables craftingCont;
    private FirstPersonController firstPerson;
    //private vector değişkenler
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 playerCameraPosition;
    private Vector3 tempCameraPosition;
    //boolean Değişkenler
    private bool isGround;
    private bool isJetPackUsable;
    private bool menuFixer;
    public bool isCrouch;//stealth mod için kullanılacağından başka class dan erişilmesi gerekiyor.
    public static bool stopMovement = false;
    //objects
    public CanvasGroup inventoryMenuCG;
    public CanvasGroup bestiaryMenuCG;
    public CanvasGroup craftingCG;
    public LayerMask layerMask;

    public bool inventoryisActive;
    public bool bestiaryisActive;

    private static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        AwakeSettings();
    }

    void Start()
    {
        StartSettings();
    }

    private void Update()
    {
        PlayerMenuAction();
    }

    void FixedUpdate()
    {
        if (!inventoryisActive && !safeInvCont.isActive && !craftingCont.isActive && !bestiaryisActive)
        {
            droneGameplay();
            stopMovement = false;
            firstPerson.enabled = true;
        }
        else
        {
            firstPerson.enabled = false;
            stopMovement = true;
        }
        FixedUpdateSettings();
    }

    private void droneGameplay()
    {
        if (Input.GetButtonDown(droneInputName) && PlayType.type.Equals(PlayType.playType.fps))
        {

            toDroneGameplay();

        }
    }

    public void toDroneGameplay()
    {
        PlayType.type = PlayType.playType.drone;
        playerCamera.gameObject.SetActive(false);
        this.enabled = false;
        //weaponsController.enabled = false;
        playerObjInteract.enabled = false;
        characterController.enabled = false;
        audioSource.enabled = false;
        droneFollowMode.enabled = false;
    }

    public void toFpsGameplay()
    {
        PlayType.type = PlayType.playType.fps;
        playerCamera.gameObject.SetActive(true);
        this.enabled = true;
        //weaponsController.enabled = true;
        playerObjInteract.enabled = true;
        characterController.enabled = true;
        audioSource.enabled = true;
        droneFollowMode.enabled = true;
    }

    //metodların başlangıcı...
    private void AwakeSettings()
    {
        playerGO = this.gameObject;
        playerLoadOut = GetComponent<PlayerLoadOut>();
        playerObjInteract = GetComponent<PlayerObjInteract>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        playerCapsuleCollider = playerGO.GetComponent<CapsuleCollider>();
        playerCamera = playerGO.GetComponentInChildren<Camera>();
        playerObject = playerGO.GetComponent<Player>();
        loadOut = FindObjectOfType<LoadOutInteractables>();
        safeInvCont = FindObjectOfType<SafeInventory>();
        craftingCont = FindObjectOfType<CraftingInteractables>();
        firstPerson = GetComponent<FirstPersonController>();
        playerHeight = playerCapsuleCollider.height;
        isCrouch = false;
        isJetPackUsable = true;
        inventoryInputName = ButtonKeyController.InventoryInputName;
        jetPackInputName = ButtonKeyController.jetPackInputName;
        crouchInputName = ButtonKeyController.crouchInputName;
        sprintInputName = ButtonKeyController.sprintInputName;
        droneInputName = ButtonKeyController.droneInputName;
        staticInteractablesExit = ButtonKeyController.staticInteractablesExit;
        Health = playerObject.Health;
        playerMoveSpeed = playerObject.playerMoveSpeed;
        jetPackSpeed = playerObject.jetPackSpeed;
        jetPackCoolDown = playerObject.jetPackCoolDown;
        playerJumpPower = playerObject.playerJumpPower;
        playerStamina = playerObject.playerStamina;
    }

    private void StartSettings()
    {
        Health = 50;
        Vector3 tempPlayerScale = this.transform.localScale;
        droneFollowMode = FindObjectOfType<DroneFollowMode>().GetComponent<DroneFollowMode>();
        playerRB = GetComponent<Rigidbody>();
        playerCameraPosition = playerCamera.transform.localPosition;
        tempPlayerMoveSpeed = playerMoveSpeed;
        inventoryisActive = false;
        bestiaryisActive = false;
        inventoryMenuCG.interactable = false;
        inventoryMenuCG.blocksRaycasts = false;
        inventoryMenuCG.alpha = 0;
        bestiaryMenuCG.interactable = false;
        bestiaryMenuCG.blocksRaycasts = false;
        bestiaryMenuCG.alpha = 0;
    }

    private void FixedUpdateSettings()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, playerJumpRayLong, layerMask))
        {
            isGround = true;
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            isGround = false;
        }
        if (!isJetPackUsable)
        {
            JetpackCDTimer();
        }
        if (!Input.GetButton(sprintInputName))
        {
            playerStamina += Time.fixedDeltaTime;
            if (playerStamina > 10)
                playerStamina = 10f;
        }
        if(playerStamina < 0)
        {
            playerStamina = 0;
        }
    }

    //player ulaşacağı inventory, craftin vb. menulerin çalıştırıldığı fonksiyonlar
    void PlayerMenuAction()
    {
        InventoryMenuController();
        SafeInventoryMenuController();
        CraftingTableMenuController();
        BestiaryMenuController();
       // CursorState();
    }
    

    //player ve zıplama jetpack metodu
    private void playerUpwardMovement()
    {
        //jetpack kullanıyorsak
        if (Input.GetButtonDown(jetPackInputName) && isJetPackUsable && !isCrouch)
        {
            Debug.Log("jetpack kullanıldı");
            playerRB.AddForce(transform.up * jetPackSpeed, ForceMode.Acceleration);
            isJetPackUsable = false;
        }
    }

    //jetpack cooldown settings
    private void JetpackCDTimer()
    {
        tempJetPackCD += Time.fixedDeltaTime;
        if(tempJetPackCD >= jetPackCoolDown)
        {
            isJetPackUsable = true;
            tempJetPackCD = 0;
        }
    }

    //player eğilme metodu
    private void playerCrouch()
    {
        Vector3 colliderCenterParam;
        if (Input.GetButtonDown(crouchInputName) && !isCrouch)
        {
            //burada collider boyutunu küçültüp local pozisyonunu değiştiriyoruz kamerada aynı işlemi yapıyoruz.
            colliderCenterParam = new Vector3(0, -0.5f, 0);
            playerCapsuleCollider.height = 1f;
            playerCapsuleCollider.center = colliderCenterParam;
            playerMoveSpeed = tempPlayerMoveSpeed / 2;
            isCrouch = true;
        }
        else if(Input.GetButtonDown(crouchInputName) && isCrouch)
        {
            //burada collider boyutunu küçültüp local pozisyonunu değiştiriyoruz kamerada aynı işlemi yapıyoruz.
            colliderCenterParam = Vector3.zero;
            playerCapsuleCollider.height = 2f;
            playerCapsuleCollider.center = colliderCenterParam;
            playerMoveSpeed = tempPlayerMoveSpeed;
            StartCoroutine(CrouchUp());
            isCrouch = false;
        }
    }

    IEnumerator CrouchDown()
    {
        Vector3 crouchStart = playerCameraPosition;
        Vector3 crouchEnd = new Vector3(playerCameraPosition.x, 0, playerCameraPosition.z);
        for(float t = 0; t<1f; t += Time.deltaTime / crouchCameraSmooth)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(crouchStart, crouchEnd, t);
            yield return null;
        }
    }

    IEnumerator CrouchUp()
    {
        Vector3 crouchStart = playerCameraPosition;
        Vector3 crouchEnd = new Vector3(playerCameraPosition.x, 0, playerCameraPosition.z);
        for (float t = 0; t < 1f; t += Time.deltaTime / crouchCameraSmooth)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(crouchEnd, crouchStart, t);
            yield return null;
        }

    }

    private void CursorState()
    {
        if (safeInvCont.isActive && bestiaryisActive && inventoryisActive && craftingCont.isActive)
        {
            print("sa");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!safeInvCont.isActive && !inventoryisActive && !craftingCont.isActive && !bestiaryisActive)
        {
            print("as");

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //LoadOut menüsünün açılıp açılmadığını kontrol eden metod
    private void InventoryMenuController()
    {
        
        if (!inventoryisActive && loadOut.isActive)
        {
            //menu aktif 
            inventoryMenuCG.interactable = true;
            inventoryMenuCG.blocksRaycasts = true;
            inventoryMenuCG.alpha = 1;
            inventoryisActive = true;
            inventory.SetActive(true);
            safeUI.SetActive(false);
            return;

        }
        if (Input.GetButtonDown(inventoryInputName) && inventoryisActive && loadOut.isActive)
        {
            //menu kapalı
            inventoryMenuCG.interactable = false;
            inventoryMenuCG.blocksRaycasts = false;
            inventoryMenuCG.alpha = 0;
            inventoryisActive = false;
            loadOut.isActive = false;
            return; 
        }
      
    }

    private void SafeInventoryMenuController()
    {
        if (safeInvCont.isActive)
        {
            //menu aktif 
            inventoryMenuCG.interactable = true;
            inventoryMenuCG.blocksRaycasts = true;
            inventoryMenuCG.alpha = 1;
            inventory.SetActive(false);
            safeUI.SetActive(true);
        }
        if (Input.GetButtonDown(staticInteractablesExit) && safeInvCont.isActive)
        {
            //menu kapalı
            inventoryMenuCG.interactable = false;
            inventoryMenuCG.blocksRaycasts = false;
            inventoryMenuCG.alpha = 0;
            safeInvCont.isActive = false;
        }
       
    }
    


    private void BestiaryMenuController()
    {
        if (Input.GetButtonDown(ButtonKeyController.BestiaryInputName) && !bestiaryisActive)
        {
            //menu aktif 
            bestiaryMenuCG.interactable = true;
            bestiaryMenuCG.blocksRaycasts = true;
            bestiaryMenuCG.alpha = 1;
            bestiaryisActive = true;
        }
        else if (Input.GetButtonDown(ButtonKeyController.BestiaryInputName) && bestiaryisActive)
        {
            //menu kapalı
            bestiaryMenuCG.interactable = false;
            bestiaryMenuCG.blocksRaycasts = false;
            bestiaryMenuCG.alpha = 0;
            bestiaryisActive = false;
        }
       
    }

    private void CraftingTableMenuController()
    {
        if (craftingCont.isActive)
        {
            //menu aktif 
            craftingCG.interactable = true;
            craftingCG.blocksRaycasts = true;
            craftingCG.alpha = 1;
        }
        if (Input.GetButtonDown(staticInteractablesExit) && craftingCont.isActive)
        {
            //menu kapalı
            craftingCG.interactable = false;
            craftingCG.blocksRaycasts = false;
            craftingCG.alpha = 0;
            craftingCont.isActive = false;
        }
       
    }
    //fonksiyonların bitişi....
}
