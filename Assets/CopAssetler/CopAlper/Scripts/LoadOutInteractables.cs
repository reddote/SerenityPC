using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOutInteractables : StaticInteractables
{
    public GameObject loadOutUI;
    public GameObject safeListUI;
    public SafeInventory safeInventory;
    public PlayerController playerController;
    public PlayerLoadOut playerLoadOut;
    public void ListByTypes(string slotType, int slot)
    {
        playerLoadOut.slotNumber = slot;
        //itemların typelarına göre sıralayıp aktif ediyoruz. weapon slota tıkladığında sadece silahları görebilmesi için
        foreach (var x in safeInventory.objUI)
        {
            var tempGO = x.GetComponent<UISafeInventory>();
            if (tempGO.itemobj.itemType.ToString() == slotType)
            {
               x.SetActive(true);
               tempGO.loadOutItemType = slotType;
            }
            else
            {
                x.SetActive(false);
            }
        }
        playerController.safeUI.SetActive(true);
    }

}
