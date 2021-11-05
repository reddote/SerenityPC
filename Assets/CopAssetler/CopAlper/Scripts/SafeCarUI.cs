using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCarUI : MonoBehaviour
{
    public int itemID;
    public int itemCount;
    public int itemLimit;
    public string itemName;
    public SafeInventory instance;

    void Start()
    {
        instance = FindObjectOfType<SafeInventory>();
    }
}
