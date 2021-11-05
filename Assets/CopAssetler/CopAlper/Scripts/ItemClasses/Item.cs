using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ItemTypes { Default, Weapon, Utility, Gagdet, Craftable, Ammo, Turret };


public class Item : MonoBehaviour
{
    public string itemName = "New Item";
    public int ID;
    public int itemCountLimit;
    public Sprite icon = null;
    public bool isDefault = false;
    public bool isItemBeingInteract = false;
    public ItemTypes itemType;
    public bool isQueue = false;


    public void Update()
    {
        if (isItemBeingInteract)
        {
            isItemBeingInteract = false;
        }
    }



}
