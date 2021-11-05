using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class placementController : MonoBehaviour
{

    
    [SerializeField]
    public Turret[] placeableObjectPrefabs;
    public static List <Turret> turretsInScene = new List<Turret>();
    private Turret currentPlaceableObject;
    private bool isPlaceable=true;

    private float mouseWheelRotation;
    private int currentPrefabIndex = -1;
    private TowDefTool towDefTool;


    private void Start()
    {
        turretsInScene = FindObjectsOfType<Turret>().ToList();
        towDefTool = FindObjectOfType<TowDefTool>();

    }

    private void Update()
    {
        
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
       
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }

    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (PlayType.type==PlayType.playType.towDef && Input.GetKeyDown(KeyCode.Alpha0 + 1 + i)&& placeableObjectPrefabs[i].price<=TowDefTool.Currency)
            {
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentPlaceableObject.gameObject);
                    currentPrefabIndex = -1;
                }
                else
                {
                    if (currentPlaceableObject != null)
                    {
                        Destroy(currentPlaceableObject.gameObject);
                    }

                    currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
                    currentPrefabIndex = i;
                }

                break;
            }
            else if (placeableObjectPrefabs[i].price > TowDefTool.Currency && Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                Debug.Log("Not Enough Money");
            }
        }
    }

    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentPlaceableObject != null && currentPrefabIndex == i;
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {

        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPlaceable = true;
            foreach(Turret turret in turretsInScene)
            {

                if (Vector3.Distance(turret.transform.position,currentPlaceableObject.transform.position)<6.5f)
                {
                    isPlaceable = false;
                    break;
                }
            }

            Debug.Log(isPlaceable);


            if (isPlaceable)
            {
                turretsInScene.Add(currentPlaceableObject);
                towDefTool.CurrencyUpdater(currentPlaceableObject.price);
                TowDefTool.textUpdater();
                currentPlaceableObject.rangeCollider.enabled = true;
                currentPlaceableObject.colliderr.enabled = true;

                TurretLaser turret = currentPlaceableObject as TurretLaser;

                if (turret != null)
                {
                    foreach (LineRenderer l in turret.lasers)
                    {

                        l.SetPosition(0, l.transform.parent.position);
                    }
                }

                currentPlaceableObject = null;
            }
           
        }
       
    }





}
