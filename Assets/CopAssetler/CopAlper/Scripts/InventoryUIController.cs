using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController itemController;
    public int[] itemsID;
    public string[] itemsName;
    public int[] slotNumber;
    public int inventorySlotCap;
    public string[] InventorySlotText = new string[2];
    public bool isSame;
    private int InventorySlot;
    private Item itemsObj;
    public GameObject[] buttonGO = new GameObject[2];
    public List<Item> itemsGameObject;
    public GameObject[] parentGO = new GameObject[2];
    public GameObject secretItemSlotGO;
    public GameObject noneSecretItemSlotGO;
    public List<GameObject> rectTransformsGO = new List<GameObject>();
    public List<GameObject> rectGUITransformsGO = new List<GameObject>();


    void Awake()
    {

        if (itemController == null)
        {
            DontDestroyOnLoad(gameObject);
            itemController = this;
        }
        else if (itemController != this)
        {
            Destroy(gameObject);
        }
        

    }
    void Start()
    {
        StartSettings();
    }

    void StartSettings()
    {
        itemsID = new int[itemsGameObject.Count];
        itemsName = new string[itemsGameObject.Count];
        slotNumber = new int[inventorySlotCap];
        InventorySlot = inventorySlotCap - 1;
        for (int i = 0; i < itemsGameObject.Count; i++)
        {
            itemsObj = itemsGameObject[i];
            itemsID[i] = itemsObj.ID;
            itemsName[i] = itemsObj.itemName;
        }
    }
}
