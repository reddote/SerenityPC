using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class CraftingUI : MonoBehaviour
{
    public CraftinWorkOrder workOrder;
    public CraftingItems craftItem;
    public GameObject orderUIGO;
    public GameObject orderUISlot;
    private float timer;
    private CraftingOrderUI craftingOrderUI;
    private bool isSelected;
    private CraftingInteractables craftTable;
    public GameObject tempGO;

    void Start()
    {
        timer = craftItem.craftTime;
        craftTable = FindObjectOfType<CraftingInteractables>();
        workOrder = GetComponentInParent<CraftinWorkOrder>();
    }

    void Update()
    {
        if (isSelected && workOrder.workList[0] == this.gameObject)
        {
            //item craft süresi
            timer -= Time.deltaTime;
            craftingOrderUI.UIText.text = "Item Name: " + craftItem.itemName + "Kalan Süre: " + (int)timer;
            if(timer <= 0)
            {
                craftTable.AddCraft(craftItem);
                Destroy(tempGO);
                isSelected = false;
                timer = craftItem.craftTime;
                workOrder.workList.Remove(this.gameObject);
            }
        }
    }

    public void ButtonOnClick()
    {
        craftTable.ItemCheck(craftItem);

        if (tempGO != null)
        {
            Debug.Log("item craftlıyon ya göt");
        }
        else if (craftTable.enoughItem && tempGO == null)
        {
            //Craft sırasını gösteren UI obj sini oluşturuyoruz.
            tempGO = Instantiate(orderUISlot, orderUIGO.transform);
            craftingOrderUI = tempGO.GetComponent<CraftingOrderUI>();
            isSelected = true;
            workOrder.workList.Add(this.gameObject);
            craftingOrderUI.UIText.text = "Item Name: " + craftItem.itemName + "Kalan Süre: " + (int)timer;
        }
        else
        {
            Debug.Log("UI item yeterli değil");
            isSelected = false;
        }
        
    }
}
