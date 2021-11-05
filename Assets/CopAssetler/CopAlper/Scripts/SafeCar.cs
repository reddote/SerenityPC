using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCar : MonoBehaviour
{
    //kasamızdaki itemlerin bilgilerini içeren structer
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
    public SafeInventory inventory;

    //kasaya item eklerken kullanılan metod
    public void AddItemSafe(Item itemObj, int ID, int giveCount, int itemLimit, string name)
    {
        //item zaten varsa
        if (safeItems.Exists(elements => elements.ID == ID))
        {
            int index = safeItems.FindIndex(elements => elements.ID == ID);
            safeItems[index].count += giveCount;
            var tempGO = objUI[index].GetComponent<SafeCarUI>();
            tempGO.itemID = safeItems[index].itemObj.ID;
            tempGO.itemCount = safeItems[index].count;
            tempGO.itemName = safeItems[index].itemObj.itemName;
            tempGO.itemLimit = safeItems[index].itemObj.itemCountLimit;

        }
        //diğer durumlarda
        else
        {
            safeItems.Add(new ItemInfos() { itemObj = itemObj, ID = ID, count = giveCount, name = name, itemLimit = itemLimit });
            var tempGO = Instantiate(UIgameObject, parentGO.transform);
            objUI.Add(tempGO);
            var wululu = tempGO.GetComponent<SafeCarUI>();
            wululu.itemID = itemObj.ID;
            wululu.itemCount = giveCount;
            wululu.itemName = itemObj.itemName;
            wululu.itemLimit = itemObj.itemCountLimit;
        }
    }

    //kasadan item çıkarırken kullanılan metod
    public void RemoveItemSafe(int ID, int takeCount)
    {
        Debug.Log("Sc");
        if (safeItems.Exists(elements => elements.ID == ID))
        {
            Debug.Log("Sb");
            int index = safeItems.FindIndex(elements => elements.ID == ID);
            safeItems[index].count -= takeCount;
            if (safeItems[index].count == 0)
            {
                Debug.Log("S");
                safeItems.RemoveAt(index);
                Destroy(objUI[index]);
                objUI.RemoveAt(index);
            }
            else
            {
                var tempGO = objUI[index].GetComponent<SafeCarUI>();
                tempGO.itemCount = safeItems[index].count;
            }
        }
    }

    public int itemCountCheck(int ID)
    {
        if (safeItems.Exists(elements => elements.ID == ID))
        {
            int index = safeItems.FindIndex(elements => elements.ID == ID);
            return safeItems[index].count;
        }
        else
            return 0;
    }

    public void sendItemSafe()
    {
        foreach(ItemInfos x in safeItems)
        {
            inventory.AddItemSafe(x.itemObj, x.ID, itemCountCheck(x.ID), x.itemLimit, x.name);
            //RemoveItemSafe(x.ID, itemCountCheck(x.ID));
        }
        for(int i = 0; i < safeItems.Count; i++)
        {
            //inventory.AddItemSafe(x.itemObj, x.ID, itemCountCheck(x.ID), x.itemLimit, x.name);
            RemoveItemSafe(safeItems[i].ID, itemCountCheck(safeItems[i].ID));
        }
    }
}
