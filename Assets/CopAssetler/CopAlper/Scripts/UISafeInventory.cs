using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISafeInventory : MonoBehaviour
{
    public Item itemobj;
    public int itemID;
    public int itemCount;
    public int itemLimit;
    public string itemName;
    public SafeInventory instance;
    public TextMeshProUGUI UIText;
    public string loadOutItemType;
    private PlayerLoadOut playerLoadOut;

    Button safeUIButton;

    void Start()
    {
        safeUIButton = GetComponent<Button>();
        instance = FindObjectOfType<SafeInventory>();
        playerLoadOut = FindObjectOfType<PlayerLoadOut>();
        SetTextUI();
    }

    public void ButtonOnClick()
    {
        if (itemCount > 0)
        {
            //turret ise eline al ve koy karşim
            if (itemobj.itemType == ItemTypes.Turret)
            {
                instance.RemoveItemSafe(itemID, 1);
                Turret turret = Instantiate(itemobj.gameObject, GameObject.Find("TurretHoldCenter").transform, false).GetComponent<Turret>();
                
                Debug.Log(turret,turret);
              
                if (turret != null)
                {
                    turret.transform.localPosition = new Vector3(0, 0, 0);
                    for (int i = 0; i < turret.meshRenderers.Count; i++)
                    {
                        turret.meshRenderers[i].material = turret.hologramMaterial;
                    }
                    turret.turretModeActive = false;
                }
            }
            if (itemobj.itemType.ToString() == loadOutItemType)
            {
                instance.RemoveItemSafe(itemID, 1);
                playerLoadOut.EquipItem(itemobj);
            }
        }
        
    }

    public void SetTextUI()
    {
        UIText.SetText("Item Name : " + itemName + " Item Count : " + itemCount);
    }
}
