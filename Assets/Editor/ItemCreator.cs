using System;
using System.Collections;
using System.Collections.Generic;
using Gaia.FullSerializer.Internal.DirectConverters;
using UnityEditor;
using UnityEngine;

public class ItemCreator : ScriptableWizard
{
    public enum ItemTypes
    {
        Default,
        Weapon,
        Utility,
        Gagdet,
        Craftable,
        Ammo,
        Turret
    };

    public bool isCraftable;
    public string itemName =  "New Item";
    public int ID;
    public int itemCountLimit;
    public Sprite icon = null;
    public bool isDefault;
    public bool isItemBeingInteract;
    public ItemTypes itemType;
    public bool isQueue;
    [Space(10)]
    [Header("Craftable Item Variables")]
    public float craftTime;
    public int howManyItemNeeD;
    public List<Item> NeedItem;
    public List<int> numberOfNeeds;
    
    [MenuItem ("My Tools/Item Creator")]
    static void ItemCreatorWizard()
    {
        ScriptableWizard.DisplayWizard<ItemCreator>("Item Create", "Create");
    }

    private void OnWizardCreate()
    {
        //item seçildikten sonra 
        if (Selection.activeTransform != null)
        {
            ItemProperties();
        }
    }

    private void OnWizardUpdate()
    {
        helpString = "Enter Item Details";
    }

    //create butonuna basınca bu özellikler ile beraber Item scriptnini ve prefab klasörüne prefab olarak ekler
    private void ItemProperties()
    {
        GameObject itemGO = Selection.activeTransform.gameObject;

        
        if (isCraftable)
        {
            CraftingItems itemComponent = itemGO.AddComponent<CraftingItems>();
            itemGO.tag = "CollectebleItems";
            itemGO.layer = 12;
            itemComponent.itemName = itemName;
            itemComponent.ID = ID;
            itemComponent.itemCountLimit = itemCountLimit;
            itemComponent.icon = icon;
            itemComponent.isDefault = isDefault;
            itemComponent.isItemBeingInteract = isItemBeingInteract;
            itemComponent.itemType = (global::ItemTypes) itemType;
            itemComponent.isQueue = isQueue;
            itemComponent.craftTime = craftTime;
            itemComponent.NeedItem = NeedItem;
            itemComponent.numberOfNeeds = numberOfNeeds;
            itemComponent.howManyItemNeeD = howManyItemNeeD;
            PrefabUtility.SaveAsPrefabAsset(itemGO, "Assets/Prefabs/Resources/"+ itemComponent.itemType.ToString() + "Prefabs/" + itemGO.name + ".prefab");
            itemGO.tag = "Untagged";
            itemGO.layer = 0;
            DestroyImmediate(itemGO.GetComponent<Item>());
        }
        else 
        {
            Item itemComponent = itemGO.AddComponent<Item>();
            itemGO.tag = "CollectebleItems";
            itemGO.layer = 12;
            itemComponent.itemName = itemName;
            itemComponent.ID = ID;
            itemComponent.itemCountLimit = itemCountLimit;
            itemComponent.icon = icon;
            itemComponent.isDefault = isDefault;
            itemComponent.isItemBeingInteract = isItemBeingInteract;
            itemComponent.itemType = (global::ItemTypes) itemType;
            itemComponent.isQueue = isQueue;
            PrefabUtility.SaveAsPrefabAsset(itemGO, "Assets/Prefabs/Resources/"+ itemComponent.itemType.ToString() + "Prefabs/" + itemGO.name + ".prefab");
            itemGO.tag = "Untagged";
            itemGO.layer = 0;
            DestroyImmediate(itemGO.GetComponent<Item>());
        }
    }
    
}
