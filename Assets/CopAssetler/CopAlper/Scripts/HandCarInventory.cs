using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCarInventory : MonoBehaviour
{
    #region Singleton

    public static HandCarInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("birden fazla instance nesnesi var");
        }

        instance = this;
        Start();
    }
    #endregion

    void Start()
    {

    }

    void Update()
    {
        
    }
}
