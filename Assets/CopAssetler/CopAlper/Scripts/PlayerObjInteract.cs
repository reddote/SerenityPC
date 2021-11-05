using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjInteract : MonoBehaviour
{
    public string InteractionButton;
    public GameObject TurretHandler;
    [SerializeField] private Camera playerMainCamera;
    //interact edeceğimiz objelerin bulunduğu layer
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask handCarLayerMask;
    [SerializeField] private LayerMask staticLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float interactTime = 2f;
    [SerializeField] private RectTransform interactionImage;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private float rangeOfInteraction = 8f;
    [SerializeField] private float staticRangeOfInteraction = 3f;
    public SafeCarAI safeCar;

    //interact ettiğimiz objeleri bu class a gönderip bunları ayıklayıp itemleri ona göre yerleştireceğiz
    // deneme eskisi için command sil private ItemController objectBeingInteract;   
    private Item interactItem;
    //craft table gibi objeler için
    private StaticInteractables staticObjectBeingInteract;
    //handcar gibi objeler için
    private HandCarInventory handCarInventory;
    private float currentHoldButtonTimer;
    private ButtonKeyController buttonController;

    public CanvasGroup interactionCanvasGroup;

    private void Awake()
    {
        InteractionButton = ButtonKeyController.InteractionInputName;
        safeCar = FindObjectOfType<SafeCarAI>();
    }

    private void Update()
    {
        UpdateSettings();
    }

    private void UpdateSettings()
    {
        InteractableObjectFromRAY();
        StaticObjectBeingInteract();
        GroundInteract();

        if (HasInteractObject() || HasStaticObjectBeingInteract() || HasHandCarInteract())
        {
            interactionCanvasGroup.interactable = enabled;
            interactionCanvasGroup.blocksRaycasts = enabled;
            interactionCanvasGroup.alpha = 1;
            if (Input.GetButton(InteractionButton))
            {
                InteractionProgress();
            }
            else
            {
                currentHoldButtonTimer = 0f;
            }

        }
        else
        {
            interactionCanvasGroup.interactable = false;
            interactionCanvasGroup.blocksRaycasts = false;
            interactionCanvasGroup.alpha = 0;
            //interactionImage.gameObject.SetActive(false);
            currentHoldButtonTimer = 0f;
        }
    }


    //go ile olan etkileşimin tamamlanıp tamamlanmadığını kontrol ediyor.
    private void InteractionProgress()
    {
        if ( interactItem != null )
        {
            SendInteractiontoItemController();
        }
        else if (staticObjectBeingInteract != null)
        {
            SendInteractiontoStaticController();
        }
        else if(handCarInventory != null)
        {
            SendInteractionHandCar();
        }
    }

    private void InteractableObjectFromRAY()
    {
        //kameranın bakış açısında ortasından ray çizgisi gönderiyoruz.
        Ray ray = playerMainCamera.ViewportPointToRay((Vector3.one / 2f));
        //Debug.DrawRay(ray.origin, ray.direction * 2f, Color.yellow);
        RaycastHit hitInfo;
        //raycast collider ile etkileşime girip girmediğine bakıyoruz
        if (Physics.Raycast(ray, out hitInfo, rangeOfInteraction, layerMask))
        {
            var hitObject = hitInfo.collider.GetComponent<Item>();

            if (hitObject == null)
            {
                interactItem = null;
            }
            else if (hitObject != null && hitObject != interactItem)
            {
                interactItem = hitObject;
            }
        }
        else
        {
            interactItem = null;
        }

    }

    private void GroundInteract()
    {
        
        Ray ray = playerMainCamera.ViewportPointToRay((Vector3.one / 2f));
        RaycastHit hitInfo;
        if(Physics.Raycast(ray,out hitInfo, staticRangeOfInteraction, groundLayerMask))
        {
            
            Turret curretPlaceableTurret=TurretHandler.GetComponentInChildren<Turret>();
            if (curretPlaceableTurret)
            {
                curretPlaceableTurret.transform.position = hitInfo.point;
                curretPlaceableTurret.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                curretPlaceableTurret.transform.localScale=new Vector3(1,1,1);

           
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("hit clicked");
                curretPlaceableTurret.transform.parent = null;
                //Bu kısım queueya atılacak.
                curretPlaceableTurret.turretModeActive = true;
                for (int i = 0; i < curretPlaceableTurret.meshRenderers.Count; i++)
                {
                    curretPlaceableTurret.meshRenderers[i].material = curretPlaceableTurret.startingMaterials[i];
                }
            }
            
            }
        }
    }

    private void StaticObjectBeingInteract()
    {
        Ray ray = playerMainCamera.ViewportPointToRay((Vector3.one / 2f));
        RaycastHit hitInfo;
        if(Physics.Raycast(ray,out hitInfo, staticRangeOfInteraction, staticLayerMask))
        {
            var staticHitObject = hitInfo.collider.GetComponent<StaticInteractables>();

            if (staticHitObject == null)
            {
                staticObjectBeingInteract = null;
            }
            else if (staticHitObject != null && staticHitObject != staticObjectBeingInteract)
            {
                staticObjectBeingInteract = staticHitObject;
            }
        }
        else
        {
            staticObjectBeingInteract = null;
        }
    }

    private void HandCarObjectInteract()
    {
        Ray ray = playerMainCamera.ViewportPointToRay((Vector3.one / 2f));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, staticRangeOfInteraction, handCarLayerMask))
        {
            var handCarObject = hitInfo.collider.GetComponent<HandCarInventory>();

            if (handCarObject == null)
            {
                staticObjectBeingInteract = null;
            }
            else if (handCarObject != null && handCarObject != handCarInventory)
            {
                handCarInventory = handCarObject;
            }
        }
        else
        {
            handCarInventory = null;
        }
    }

    private bool HasStaticObjectBeingInteract()
    {
        return staticObjectBeingInteract != null;
    }

    private bool HasInteractObject()
    {
        return interactItem != null;
    }

    private bool HasHandCarInteract()
    {
        return handCarInventory != null;
    }

    //crafting table, kasa vb. şeyler için kullanacağımız metod
    private void SendInteractiontoStaticController()
    {
        var temp = staticObjectBeingInteract.gameObject.GetComponent<StaticInteractables>();
        temp.staticInteractionController(staticObjectBeingInteract.gameObject);
    }

    //itemleri kontrol edip inventory alacağımız metod
    private void SendInteractiontoItemController()
    {
        if (interactItem.gameObject.GetComponent<Item>().isQueue == false)
        {
            interactItem.gameObject.GetComponent<Item>().isQueue = true;
            safeCar.enqueueTarget(interactItem.gameObject);
            safeCar.setSafeCarDest();
        }
        interactItem.isItemBeingInteract = true;
    }

    //itemleri el arabasına alacağımız metod
    private void SendInteractionHandCar()
    {
        Debug.Log("arabaya attın iyisiiiiiiin");
    }
}
