using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerLoadOut : MonoBehaviour
{
    public int slotNumber;
    [Header("Lists"), Space(5)]
    public Dictionary <int,GameObject> weaponList;
    public GameObject[] weapons;
    public List<GameObject> equippedWeapon;
    public Weapon equipped;
    [Header("Arrays"), Space(5)]
    public LoadoutUI[] weaponSlots;
    public LoadoutUI[] utilitySlots;
    public LoadoutUI[] gearSlots;
    [Header("Buttons"), Space(5)]
    public string reloadButton;
    public string weaponSlot1;
    public string weaponSlot2;
    public string holdWeapon;
    [Header("Weapon Vars"), Space(5)] 
    public int tempClipSize;
    public float reloadTimer;
    private float currentReloadTimer = 0f;
    public SafeInventory safe;
    public bool isReloading = false;
    public int ammoAmount = 100;

    private void Awake()
    {
        reloadButton = ButtonKeyController.reloadInputName;
        weaponSlot1 = ButtonKeyController.weaponSlot1InputName;
        weaponSlot2 = ButtonKeyController.weaponSlot2InputName;
        holdWeapon = ButtonKeyController.InputZ;
        weaponList = new Dictionary<int, GameObject>();
        //silahları dictionary eklemek için kullanıyoruz
        for (int i = 0; i < weapons.Count(); i++)
        {
            weaponList.Add(i+1,weapons[i]);
        }
    }
    private void Update()
    {
        WeaponController();
        if (equipped)
        {
            WeaponSettings();

        }
    }

    public void WeaponSettings()
    {
        if (!isReloading)
        {
            equipped.Shoot();
        }
        Reload();
        if (Input.GetButtonDown(reloadButton) || equipped.clipSize == 0)
        {
            equipped.Reload(true);
            isReloading = true;
        }
        if (isReloading)
        {
            currentReloadTimer += Time.deltaTime;
        }
    }

    //weapon listesinden silahı aktif etmek için kullanıyoruz.
    public void WeaponController()
    {
        if (Input.GetButtonDown(weaponSlot1) && equippedWeapon[0])
        {
            foreach (var a in equippedWeapon)
            {
                if (a == equippedWeapon[0])
                {
                    a.SetActive(true);
                    equipped = a.GetComponent<Weapon>();
                    reloadTimer = equipped.reloadTimer;
                    tempClipSize = equipped.clipSize;
                }
                else if(a)
                {
                    a.SetActive(false);
                }
            }
        }
        if (Input.GetButtonDown(weaponSlot2)&& equippedWeapon[1])
        {
            foreach (var a in equippedWeapon)
            {
                if (a == equippedWeapon[1])
                {
                    a.SetActive(true);
                    equipped = a.GetComponent<Weapon>();
                    reloadTimer = equipped.reloadTimer;
                    tempClipSize = equipped.clipSize;
                }
                else if(a)
                {
                    a.SetActive(false);
                }
            }
        }
        if (Input.GetButtonDown(holdWeapon))
        {
            foreach (var a in equippedWeapon)
            {
                if (a == equippedWeapon[2])
                {
                    a.SetActive(true);
                }
                else if(a)
                {
                    a.SetActive(false);
                }
            }
        }
    }

    //weaponlist gameobj listesi ile itemları linkliyoruz.    
    public void WeaponLinker(int ID)
    {
        var weaponGO = weaponList.FirstOrDefault(x => x.Key == ID).Value;
        equippedWeapon[slotNumber] = weaponGO;
        var weaponTemp = weaponGO.GetComponent<Weapon>();
        weaponSlots[slotNumber].bulletLimit = weaponTemp.bulletLimit;
        weaponSlots[slotNumber].clipSize = weaponTemp.clipSize;
    }
    
    public void EquipItem(Item whichItem)
    {
        if (whichItem.itemType == ItemTypes.Weapon)
        {
            //eğer slotta item varsa kasaya geri koyması için ekliyor...
            if (weaponSlots[slotNumber].equippedItem != null)
            {
                safe.AddItemSafe(weaponSlots[slotNumber].equippedItem, weaponSlots[slotNumber].equippedItem.ID, 1, 
                    weaponSlots[slotNumber].equippedItem.itemCountLimit, weaponSlots[slotNumber].equippedItem.itemName);
            }
            weaponSlots[slotNumber].equippedItem = whichItem;
            WeaponLinker(whichItem.ID);
        }
        else if (whichItem.itemType == ItemTypes.Utility)
        {
            utilitySlots[slotNumber].equippedItem = whichItem;
        }
        else if(whichItem.itemType == ItemTypes.Gagdet)
        {
            gearSlots[slotNumber].equippedItem = whichItem;
        }
    }
    
    void Reload()
    {
        if (currentReloadTimer >= reloadTimer)
        {
            int bulletinClip = tempClipSize - equipped.clipSize;
            weaponSlots[slotNumber].BulletChanger(bulletinClip);
            if ( weaponSlots[slotNumber].hasEnoughBullets)
            {
                equipped.clipSize =  weaponSlots[slotNumber].tempClipSize;
            }
            currentReloadTimer = 0;
            equipped.Reload(false);
            isReloading = false;
        }
    }
    
}
