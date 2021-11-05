using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SafeInventory : StaticInteractables
{
    //kasamızdaki itemlerin bilgilerini içeren Structure
    public class ItemInfos
    {
        public Item itemObj;
        public int ID;
        public int count;
        public int itemLimit;
        public string name;
    }
    
    public List<ItemInfos> safeItems = new List<ItemInfos>();
    public GameObject UIgameObject;
    public GameObject parentGO;
    public List<GameObject> objUI = new List<GameObject>();
    public List<Item> cheaterItem;

    //kasaya item eklerken kullanılan metod
    public void AddItemSafe(Item itemObj, int ID, int giveCount, int itemLimit, string name)
    {
        //item zaten varsa
        if (safeItems.Exists(elements => elements.ID == ID))
        {
            int index = safeItems.FindIndex(elements => elements.ID == ID);
            safeItems[index].count += giveCount;
            var tempGO = objUI[index].GetComponent<UISafeInventory>();
            tempGO.itemID = safeItems[index].itemObj.ID;
            tempGO.itemCount = safeItems[index].count;
            tempGO.itemName = safeItems[index].itemObj.itemName;
            tempGO.itemLimit = safeItems[index].itemObj.itemCountLimit;
            tempGO.SetTextUI();
        }
        //diğer durumlarda
        else
        {
            safeItems.Add(new ItemInfos() { itemObj = itemObj, ID = ID, count = giveCount, name = name, itemLimit = itemLimit });
            var tempGO = Instantiate(UIgameObject, parentGO.transform);
            objUI.Add(tempGO);
            var wululu = tempGO.GetComponent<UISafeInventory>();
            wululu.itemID = itemObj.ID;
            wululu.itemCount = giveCount;
            wululu.itemName = itemObj.itemName;
            wululu.itemLimit = itemObj.itemCountLimit;
            wululu.itemobj = itemObj;
        }
    }

    //kasadan item çıkarırken kullanılan metod
    public void RemoveItemSafe(int ID, int takeCount)
    {
        if (safeItems.Exists(elements => elements.ID == ID))
        {
            int index = safeItems.FindIndex(elements => elements.ID == ID);
            safeItems[index].count -= takeCount;
            if(safeItems[index].count == 0)
            {
                safeItems.RemoveAt(index);
                Destroy(objUI[index]);
                objUI.RemoveAt(index);
            }
            else
            {
                var tempGO = objUI[index].GetComponent<UISafeInventory>();
                tempGO.itemCount = safeItems[index].count;
            }
        }
    }

    void Awake()
    {
        //OYUNUN HILESI OYUN ÇIKTIĞINDA SILMEYI UNUTMA!!!!!!!!!
        foreach (Item cheater in cheaterItem)
        {
            AddItemSafe(cheater, cheater.ID, 99, cheater.itemCountLimit, cheater.name);
        }
    }
}
