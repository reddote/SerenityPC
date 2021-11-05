using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingInteractables : StaticInteractables
{
    public List<CraftingItems> craftableItems;
    public SafeInventory safeInventory;
    public List<bool> hasItems;
    public bool enoughItem;

    void Update()
    {

    }

    //gerekli şartlar sağlandığında itemi craftlayacak metod
    public void AddCraft(CraftingItems craftables)
    {
        if (!enoughItem)
        {
            Debug.Log("not enough Items");
        }
        else if(enoughItem)
        {
            safeInventory.AddItemSafe(craftables, craftables.ID, 1, craftables.itemCountLimit, craftables.name);
            for(int i = 0; i< craftables.howManyItemNeeD; i++)
            {
                safeInventory.RemoveItemSafe(craftables.NeedItem[i].ID, craftables.numberOfNeeds[i]);
            }
        }
    }

    //itemin craftlanmak için yeterli materyeli olup olmadığını check eden metod
    public void ItemCheck(CraftingItems craft)
    {
        for (int i = 0; i < craft.howManyItemNeeD; i++)
        {
            Debug.Log("crafting b");
            int ID = craft.NeedItem[i].ID;
            if (safeInventory.safeItems.Exists(elements => elements.ID == ID))
            {
                Debug.Log("crafting c");
                int index = safeInventory.safeItems.FindIndex(elements => elements.ID == ID);
                if (safeInventory.safeItems[index].count >= craft.numberOfNeeds[i])
                {
                    hasItems.Add(true);
                }
            }
        }
        if (hasItems.Contains(false) || !hasItems.Any())
        {
            enoughItem = false;
            Debug.Log("not enough Items");
            hasItems.Clear();
        }
        else if (hasItems.Count == craft.howManyItemNeeD )
        {
            Debug.Log("crafting d");
            enoughItem = true;
            hasItems.Clear();
        }
        hasItems.Clear();
    }
}
