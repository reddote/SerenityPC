using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutUI : MonoBehaviour
{
    public string slotType;
    public int loadOutSlotNumber;
    public int numberOfBullets;
    public int bulletLimit;
    public int clipSize;
    public int tempClipSize;
    public Item equippedItem;
    public LoadOutInteractables loadOut;
    public bool hasEnoughBullets;

    public void SlotOnClick()
    {
        loadOut.ListByTypes(slotType, loadOutSlotNumber);
    }

    public void BulletChanger(int bulletsInClip)
    {
        //üzerimizdeki mermi clipSize dan büyükse
        if (bulletLimit > bulletsInClip)
        {
            if (bulletsInClip > 0)
            {
                hasEnoughBullets = true;
                tempClipSize = clipSize;
                bulletLimit -= bulletsInClip;
            }
            else
            {
                hasEnoughBullets = true;
                tempClipSize = clipSize;
                bulletLimit -= clipSize;
            }

        }
        //eğer üzerimizdeki mermi clipSize dam küçükse
        else if (bulletLimit < bulletsInClip)
        {
            if (bulletLimit == 0)
            {
                hasEnoughBullets = false;
                tempClipSize = 0;
            }
            else
            {
                hasEnoughBullets = true;
                tempClipSize = bulletLimit;
                bulletLimit = 0;
            }
        }
        else
        {
            hasEnoughBullets = false;
        }
    }
}
